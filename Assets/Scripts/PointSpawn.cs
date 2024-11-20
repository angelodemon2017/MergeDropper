using UnityEngine;

public class PointSpawn : MonoBehaviour
{
    private void Update()
    {
        var temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        var tempPos = transform.position;
        tempPos.x = temp.x;
        transform.position = tempPos;
    }
}