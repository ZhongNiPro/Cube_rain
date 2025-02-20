using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private bool _haveContact = false;

    public event Action<Cube> Collided;

    public Rigidbody Rigidbody => _rigidbody;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Surface surface = contact.otherCollider.GetComponent<Surface>();

            if (surface != null && _haveContact == false)
            {
                OnCollisionContact();
                _haveContact = true;
            }
        }
    }

    private void OnCollisionContact()
    {
        float minDelay = 2;
        float maxDelay = 5;

        StartCoroutine(WaitForRelease(UnityEngine.Random.Range(minDelay, maxDelay)));
    }

    private IEnumerator WaitForRelease(float seconds)
    {
        _renderer.material.color = UnityEngine.Random.ColorHSV();

        yield return new WaitForSeconds(seconds);

        if (gameObject.activeSelf)
            Collided?.Invoke(this);

        _renderer.material.color = Color.white;
        _haveContact = false;
    }
}
