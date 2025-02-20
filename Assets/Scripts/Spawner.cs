using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;

    private readonly float _repeatRate = .1f;
    private readonly int _poolCapacity = 100;
    private readonly int _poolMaxSize = 100;

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
        cube.Rigidbody.velocity = Vector3.zero;
        cube.gameObject.SetActive(true);
        cube.Collided += OnCollisionContact;
    }

    private void Start()
    {
        StartCoroutine(WaitForUse(_repeatRate));
    }

    private void OnCollisionContact(Cube cube)
    {
        cube.Collided -= OnCollisionContact;
        _pool.Release(cube);
    }

    private IEnumerator WaitForUse(float seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            _pool.Get();
        }
    }
}
