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
    [SerializeField] private List<MergeObject> _mergeObjects;
    [SerializeField] private float _forcePowerSpawn = 1f;
    [SerializeField] private GameObject _failPanel;
    [SerializeField] private TextMeshProUGUI _failPanelScore;
    [SerializeField] private TextMeshProUGUI _failPanelHiScore;
    [SerializeField] private Image _failSignal;
    [SerializeField] private SpriteRenderer _backGroundImage;
    [SerializeField] private List<Transform> _queuePositions;
    [SerializeField] private float _powerBust1;
    [SerializeField] private Button _buttonMix1;
    [SerializeField] private Button _buttonMix2;
    [SerializeField] private Button _mainTap;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Animator _mix2;

    private bool _isAudioEnable = true;
    private bool _isVFXEnable = true;

    private List<MergeObject> _queue = new();
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
    private float _failTime = 6f;//999999f;//6f;
    private float _nextRot = 0f;

    public LibraryObjects LO => _libraryObjects;
    public List<MergeObject> Rigidbody2s => _mergeObjects;

    private void Awake()
    {
        Instance = this;
        _mix2.speed = 0;
        _buttonMix1.onClick.AddListener(Bust1);
        _buttonMix2.onClick.AddListener(Bonus2);
        _mainTap.onClick.AddListener(OnTouch);
        _libraryObjects.IsInit = false;
        ResetGame();
        _backGroundImage.sprite = _libraryObjects.BackGround;
    }

    private void Update()
    {
        if (!_isFail)
        {
            CheckInput();
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
        if (_mergeObjects.Any(x => x.transform.position.y > _failHight))
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

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            OnTouch();
        }
    }

    private void OnTouch()
    {
        if (_intervalSpawn <= 0)
        {
//            _intervalSpawn = 0.2f;

            Spawn(nextLevel, _pointSpawn.transform.position);

            NextSpawn(_queu[0]);

            _queu.RemoveAt(0);
            _queu.Add(_libraryObjects.GetNextIndex(_currentMaxIndex));
            _queuText.text = $"{_queu[0]},{_queu[1]},{_queu[2]}";
            CalcQueue();
        }
    }

    public void Spawn(int level, Vector3 posit, bool isMerge = false)
    {
        if (isMerge)
        {
            if (_isVFXEnable)
            {
                Instantiate(_libraryObjects.GetVFXMergeEffect, posit, Quaternion.identity);
            }
            if (_isAudioEnable)
            {
                _audioSource.pitch = (float)level / (float)_libraryObjects.MaxLvl * 2 + 1;
                _audioSource.PlayOneShot(_libraryObjects.GetAFXMergeEffect);
            }
        }
        if (_mergeObjects.Count > 100)
        {
            return;
        }

        var newObj = Instantiate(_libraryObjects.GetObject(level));
        newObj.transform.position = posit;
        newObj.transform.rotation = Quaternion.Euler(0, 0, _nextRot);
        _mergeObjects.Add(newObj);
        newObj.RB.AddForce(Vector2.down * _forcePowerSpawn);
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

    private void CalcQueue()
    {
        Destroy(_queue[0].gameObject);
        _queue.RemoveAt(0);
        _queue[0].transform.position = _queuePositions[0].position;
        _queue[1].transform.position = _queuePositions[1].position;

        AddQueueObj(2);
    }

    private void AddQueueObj(int ind)
    {
        var miniPref = Instantiate(_libraryObjects.GetObject(_queu[ind]));
        miniPref.Deactivate();
        miniPref.transform.position = _queuePositions[ind].position;
        miniPref.transform.localScale = Vector3.one * 0.4f;
        _queue.Add(miniPref);
    }

    public void ResetGame()
    {
        _isFail = false;
        _failPanel.SetActive(false);
        _currentMaxIndex = 3;

        _mergeObjects.ForEach(x => Destroy(x.gameObject));
        _mergeObjects.Clear();

        _currentScore = 0;
        _scoreText.text = $"score:{_currentScore}";

        NextSpawn(1);

        _queu.Clear();
        for (int i = 0; i < 3; i++)
        {
            _queu.Add(_libraryObjects.GetNextIndex(_currentMaxIndex));
        }
        for (int i = 0; i < _queuePositions.Count; i++)
        {
            AddQueueObj(i);
        }
        _queuText.text = $"{_queu[0]},{_queu[1]},{_queu[2]}";
    }

    public void MergedObject(MergeObject mergeObject)
    {
        _mergeObjects.Remove(mergeObject);
        _currentScore += mergeObject.Level * mergeObject.Level;
        _scoreText.text = $"score:{_currentScore}";
        _failPanelScore.text = _scoreText.text;

        if (_currentMaxIndex < mergeObject.Level)
        {
            _currentMaxIndex = mergeObject.Level;
        }
    }

    public void Bust1()
    {
        _mergeObjects.ForEach(x => x.Bonus1(_powerBust1));//AddForce(Vector2.up * _powerBust1));
    }

    public void Bonus2()
    {
        _mix2.speed = 1;
        _mix2.Play(0);
    }

    public void SwitchAudio(bool isEnbl)
    {
        _isAudioEnable = isEnbl;
    }

    public void SwitchVFX(bool isEnbl)
    {
        _isVFXEnable = isEnbl;
    }
}