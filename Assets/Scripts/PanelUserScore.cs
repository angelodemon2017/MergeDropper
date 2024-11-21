using UnityEngine;
using TMPro;

public class PanelUserScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _numPos;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _score;

    public void Init(int place, string name, int score)
    {
        _numPos.text = $"{place}.";
        _name.text = name;
        _score.text = $"{score}";
    }
}