using TMPro;
using UnityEngine;

public class MergeCircle : MergeObject
{
    [SerializeField] private TextMeshProUGUI _text;

    internal override void UpdateObject()
    {
        base.UpdateObject();

        transform.localScale = Vector3.one * _distanceAction;
        _text.text = $"{_level}";
    }
}