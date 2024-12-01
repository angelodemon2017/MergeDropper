using TMPro;
using UnityEngine;

public class MergeCar : MergeObject
{
    [SerializeField] private TextMeshProUGUI _text;

    internal override void UpdateObject()
    {
        base.UpdateObject();

        _text.text = $"{_level}";
    }
}