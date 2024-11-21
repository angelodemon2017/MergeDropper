using UnityEngine;

public class SelfDestroyer : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Destroy(gameObject);
        }
    }
}