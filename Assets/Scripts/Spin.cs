using UnityEngine;

public class Spin : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, .2f, 0);
    }
}
