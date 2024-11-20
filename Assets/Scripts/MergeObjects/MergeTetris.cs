using TMPro;
using UnityEngine;

public class MergeTetris : MergeObject
{
    [SerializeField] private TextMeshProUGUI _text;

    private float AddedMulti = 0.85f;

    internal override void UpdateObject()
    {
        base.UpdateObject();

        float totalMulti = AddedMulti;
        for (int i = 0; i < _level; i++)
        {
            totalMulti *= AddedMulti;
        }
        transform.localScale = Vector3.one * _distanceAction * totalMulti;
        _text.text = $"{_level}";
    }
}