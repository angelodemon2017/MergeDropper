using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Library Objects", order = 1)]
public class LibraryObjects : ScriptableObject
{
    [SerializeField] private int _maxLevelSpawn;
    [SerializeField] private int _rangeRandom;
    [SerializeField] private Sprite _backGround;
    [SerializeField] private List<MergeObject> _objects;
    [SerializeField] private GameObject _VFXMergeEffect;
    [SerializeField] private AudioClip _mergeClip;

    private Dictionary<int, MergeObject> _cashObjects = new();
    public bool IsInit = false;
    public float DefForce = 2f;
    public float DefStepRandRotat = 4;

    public float GetRandomRot => Random.Range(0, DefStepRandRotat) * 360 / DefStepRandRotat;
    public int NextIndex => Random.Range(1, _maxLevelSpawn);
    public Sprite BackGround => _backGround;
    public int MaxLvl => _objects.Count;
    public GameObject GetVFXMergeEffect => _VFXMergeEffect;
    public AudioClip GetAFXMergeEffect => _mergeClip;

    public int GetNextIndex(int cap)
    {
        var max = cap <= _maxLevelSpawn ? cap : _maxLevelSpawn;

        var min = max > _rangeRandom ? max - _rangeRandom : 1;

        return Random.Range(min, max);
    }

    public MergeObject GetRandomObject()
    {
        Init();

        var index = NextIndex;

        if (index > _cashObjects.Count)
        {
            index = _cashObjects.Count;
        }

        return _cashObjects[index];
    }

    public MergeObject GetObject(int level)
    {
        Init();

        if (level > _cashObjects.Count)
        {
            level = _cashObjects.Count;
        }

        return _cashObjects[level];
    }

    private void Init()
    {
        if (!IsInit)//_cashObjects.Count != _objects.Count)
        {
            IsInit = true;
            _cashObjects.Clear();

            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].SetLevel(i + 1);

                _objects[i].IsFinal = i == _objects.Count - 1;

                _cashObjects.Add(i + 1, _objects[i]);                
            }
        }
    }
}