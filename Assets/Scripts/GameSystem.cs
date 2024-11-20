using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [SerializeField] private PointSpawn _pointSpawn;
    [SerializeField] private MergeObject _mergePrefab;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var newObj = Instantiate(_mergePrefab);
            newObj.transform.position = _pointSpawn.transform.position;
        }
    }
}