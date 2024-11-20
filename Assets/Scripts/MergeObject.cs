using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MergeObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Rigidbody2D _rigidbody;

    private int _level;

    public int Level => _level;

    private void Awake()
    {
        _level = 1;
        UpdateObject();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{collision.gameObject.name}");
        if (collision.gameObject.tag == Dict.MergeTag)
        {
            var comp = collision.gameObject.GetComponent<MergeObject>();

            if (comp.Level == _level)
            {
                Destroy(collision.gameObject);
                _level++;
                UpdateObject();
            }
        }
    }

    private void UpdateObject()
    {
        transform.localScale = Vector3.one * (1f + 0.1f * _level);
        _rigidbody.mass = 0.1f * _level;
        _text.text = $"{_level}";
    }
}