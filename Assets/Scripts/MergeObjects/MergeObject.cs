using UnityEngine;

public class MergeObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _forcePower = 1;
    [SerializeField] private Transform _rootColliders;

    [SerializeField] internal int _level = 1;

    [SerializeField] internal float _distanceAction = 1f;
    [SerializeField] internal float _distancePulse = 1f;

    public bool isWorking = true;
    public int Level => _level;
    public Rigidbody2D RB => _rigidbody;

    private void Awake()
    {
        UpdateObject();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _distanceAction);
    }

    public void SetLevel(int level)
    {
        _level = level;
        CalcDistanceAction();
        UpdateObject();
    }

    internal virtual void CalcDistanceAction()
    {
        /*        float tempMulti = 0.1f;
                for (int i = 0; i < _level - 1; i++)
                {
                    tempMulti += 0.03f * (_level + 1);
                }/**/
        _distanceAction = 0.2f + 0.2f * _level;
        _distancePulse = _distanceAction * 0.6f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isWorking)
        {
            return;
        }
        if (collision.gameObject.tag == Dict.MergeTag)
        {
            var comp = collision.gameObject.GetComponent<MergeObject>();
            if (comp.Level == _level && comp.isWorking)
            {
                isWorking = false;
                comp.isWorking = false;
                _isInitial = true;

                GameSystem.Instance.MergedObject(comp);
                GameSystem.Instance.MergedObject(this);

                var newPos = 
                    transform.position.y > comp.transform.position.y ? comp.transform.position : transform.position;
//                    (transform.position + comp.transform.position) / 2;

                SetPosMerged(newPos);
                comp.SetPosMerged(newPos);
            }
        }
    }

    private bool _isInitial = false;
    private Vector3 _startPos;
    private Vector3 _targetMerged = Vector3.zero;
    private float _timerMerged = 0f;
    private float _timeMerge = 0.2f;

    private void SetPosMerged(Vector3 newPos)
    {
        _startPos = transform.position;
        _targetMerged = newPos;
        if (!_isInitial)
        {
            Destroy(_rigidbody);
        }
    }

    private void Update()
    {
        Merged();
    }

    private void Merged()
    {
        if (_targetMerged != Vector3.zero)
        {
            _timerMerged += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPos, _targetMerged, _timerMerged / _timeMerge);
            if (_timerMerged >= _timeMerge)
            {
                Destroy(gameObject);
                if (_isInitial)
                {
                    Impulse();
                    GameSystem.Instance.Spawn(_level + 1, _targetMerged);
                }
            }
        }
    }

    private void Impulse()
    {
        foreach (var rb in GameSystem.Instance.Rigidbody2s)
        {
            if (rb == _rigidbody)
            {
                continue;
            }
            var dist = Vector3.Distance(rb.transform.position, transform.position);
            if (dist > _distanceAction)
            {
                continue;
            }

            var vectorTemp = rb.transform.position - transform.position;

            var totalPulse = _level * _forcePower * (_distanceAction - dist + 1) * GameSystem.Instance.LO.DefForce;

            rb.AddForce(vectorTemp * totalPulse);
        }
    }

    public void Deactivate()
    {
        isWorking = false;
        Destroy(_rigidbody);
        foreach (Transform child in _rootColliders)
        {
            child.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    internal virtual void UpdateObject()
    {
        _rigidbody.mass = 0.1f * _level;
    }
}