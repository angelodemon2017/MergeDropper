using UnityEngine;

public class PointSpawn : MonoBehaviour
{
    private float _border = 2.8f;
    private Vector3 _tempPos;

    private void Update()
    {
        var temp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        _tempPos = transform.position;
        _tempPos.x = temp.x < -_border ? -_border : temp.x > _border ? _border : temp.x;
        transform.position = _tempPos;
    }
}