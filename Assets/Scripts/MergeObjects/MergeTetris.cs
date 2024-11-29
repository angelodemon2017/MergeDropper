using TMPro;
using UnityEngine;

public class MergeTetris : MergeObject
{
    [SerializeField] private TextMeshProUGUI _text;

    private float AddedMulti = 0.4f;
    private float _subStep = 0.01f;

    internal override void UpdateObject()
    {
        base.UpdateObject();

        float totalMulti = AddedMulti;
        for (int i = 0; i < _level; i++)
        {
            totalMulti += _subStep;
        }
        transform.localScale = Vector3.one * totalMulti;
        _text.text = $"{_level}";
    }
}