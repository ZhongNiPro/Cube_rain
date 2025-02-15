using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private Rigidbody _rigidbody;

    public Renderer Renderer => _renderer;
    public Rigidbody Rigidbody => _rigidbody;

    public event Action<Cube> Bang;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool haveContact = false;

        foreach (ContactPoint contact in collision.contacts)
        {
            Surface surface = contact.otherCollider.GetComponent<Surface>();

            if (surface != null && haveContact == false)
            {
                Bang?.Invoke(this);
                haveContact = true;
            }
        }
    }
}
