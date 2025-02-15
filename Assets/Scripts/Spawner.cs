using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;

    private float _repeatRate = .1f;
    private int _poolCapacity = 100;
    private int _poolMaxSize = 100;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cube),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void ActionOnGet(Cube cube)
    {
        int width = 7;
        int higth = 20;

        cube.transform.position = new Vector3(Random.Range(-width, width + 1), higth, Random.Range(-width, width + 1));
        cube.gameObject.SetActive(true);
        cube.Renderer.material.color = Color.white;
        cube.Bang += OnCollisionContact;
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0f, _repeatRate);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void OnCollisionContact(Cube cube)
    {
        float minDelay = 2;
        float maxDelay = 5;

        cube.Bang -= OnCollisionContact;

        StartCoroutine(WaitForSeconds(Random.Range(minDelay, maxDelay), cube));
    }

    IEnumerator WaitForSeconds(float seconds, Cube cube)
    {
        cube.Renderer.material.color = Random.ColorHSV();

        yield return new WaitForSeconds(seconds);

        if (cube.gameObject.activeSelf)
            _pool.Release(cube);
    }
}
