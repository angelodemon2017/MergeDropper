using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    [SerializeField] private LibraryObjects _libraryObjects;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _nextLevelText;
    [SerializeField] private TextMeshProUGUI _queuText;
    [SerializeField] private PointSpawn _pointSpawn;
    [SerializeField] private MergeObject _mergePrefab;
    [SerializeField] private List<Rigidbody2D> _mergeObjects;
    [SerializeField] private float _forcePowerSpawn = 1f;
    [SerializeField] private GameObject _failPanel;
    [SerializeField] private TextMeshProUGUI _failPanelScore;
    [SerializeField] private TextMeshProUGUI _failPanelHiScore;
    [SerializeField] private Image _failSignal;
    [SerializeField] private SpriteRenderer _backGroundImage;

    private List<int> _queu = new();

    private MergeObject _tempMO;

    private int nextLevel = 1;
    private float _intervalSpawn = 0.1f;
    private int _currentScore = 0;
    private int _hiScore = 0;
    private int _currentMaxIndex = 1;

    private float _failHight = 3.8f;
    private bool _isFail = false;
    private float _failTimer = 0f;
    private float _failTime = 6f;
    private float _nextRot = 0f;

    public LibraryObjects LO => _libraryObjects;
    public List<Rigidbody2D> Rigidbody2s => _mergeObjects;

    private void Awake()
    {
        Instance = this;
        _libraryObjects.IsInit = false;
        ResetGame();
        _backGroundImage.sprite = _libraryObjects.BackGround;
    }

    private void Update()
    {
        if (!_isFail)
        {
            SpawnObject();
            if (_intervalSpawn > 0)
            {
                _intervalSpawn -= Time.deltaTime;
            }
            CheckFail();
            
            if (_tempMO != null)
            {
                _tempMO.transform.position = _pointSpawn.transform.position;
            }
        }
    }

    private void CheckFail()
    {
        if (_mergeObjects.Any(x => x.position.y > _failHight))
        {
            _failTimer += Time.deltaTime;
            if (_failTimer > _failTime)
            {
                _isFail = true;

                if (_hiScore < _currentScore)
                {
                    _hiScore = _currentScore;
                    _failPanelHiScore.text = $"HI Score:{_hiScore}";
                }
                _failPanel.SetActive(true);
                _failTimer = 0;
            }
        }
        else
        {
            _failTimer = 0;
        }
        _failSignal.enabled = _failTimer > 1f;
    }

    private void SpawnObject()
    {
        if (_intervalSpawn <= 0 && (Input.GetMouseButtonUp(0) || Input.GetKey(KeyCode.Z)))
        {
            _intervalSpawn = 0.2f;

            Spawn(nextLevel, _pointSpawn.transform.position);

            NextSpawn(_queu[0]);

            _queu.RemoveAt(0);
            _queu.Add(_libraryObjects.GetNextIndex(_currentMaxIndex));
            _queuText.text = $"{_queu[0]},{_queu[1]},{_queu[2]},{_queu[3]},{_queu[4]}";
        }
    }

    public MergeObject Spawn(int level, Vector3 posit)
    {
        if (_mergeObjects.Count > 100)
        {
            return null;
        }

        var newObj = Instantiate(_libraryObjects.GetObject(level));
        newObj.transform.position = posit;
        newObj.transform.rotation = Quaternion.Euler(0, 0, _nextRot);
        _mergeObjects.Add(newObj.RB);
        newObj.RB.AddForce(Vector2.down * _forcePowerSpawn);

        return newObj;
    }

    private void NextSpawn(int lvl)
    {
        nextLevel = lvl;
        _nextRot = _libraryObjects.GetRandomRot;
        if (_tempMO != null)
        {
            Destroy(_tempMO.gameObject);
        }
        _tempMO = Instantiate(_libraryObjects.GetObject(lvl));
        _tempMO.Deactivate();
        _tempMO.transform.rotation = Quaternion.Euler(0, 0, _nextRot);
    }

    public void ResetGame()
    {
        _isFail = false;
        _failPanel.SetActive(false);
        _currentMaxIndex = 2;

        _mergeObjects.ForEach(x => Destroy(x.gameObject));
        _mergeObjects.Clear();

        _currentScore = 0;
        _scoreText.text = $"score:{_currentScore}";

        NextSpawn(1);

        _queu.Clear();
        for (int i = 0; i < 5; i++)
        {
            _queu.Add(_libraryObjects.GetNextIndex(_currentMaxIndex));
        }
        _queuText.text = $"{_queu[0]},{_queu[1]},{_queu[2]},{_queu[3]},{_queu[4]}";
    }

    public void MergedObject(MergeObject mergeObject)
    {
        _mergeObjects.Remove(mergeObject.RB);
        _currentScore += mergeObject.Level * mergeObject.Level;
        _scoreText.text = $"score:{_currentScore}";
        _failPanelScore.text = _scoreText.text;

        if (_currentMaxIndex < mergeObject.Level)
        {
            _currentMaxIndex = mergeObject.Level;
        }
    }
}