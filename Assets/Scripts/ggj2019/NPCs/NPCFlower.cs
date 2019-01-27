using pdxpartyparrot.Core.Util;
using UnityEngine;

public class NPCFlower : NPCBase
{
    [SerializeField] private int _initialPollen = 5;
    [SerializeField] private int _pollenRate = 1;
    [SerializeField] private float _pollenFirstTime = 5;
    [SerializeField] private float _pollenDelayTime = 5;

    [SerializeField] private GameObject _pollenObj;

    [SerializeField]
    [ReadOnly]
    private int _pollen;

    public int Pollen => _pollen;

    public bool HasPollen => Pollen > 0 && !_pollenTimer.IsRunning;

    public bool IsDead { get; private set; }

    private Timer _pollenTimer;

#region Unity Lifecycle
    protected override void Awake() {
        base.Awake();

        _pollen = _initialPollen;
    }

    private void Start() {
        Pool.Add(this);
        _pollenTimer = new Timer();
        _pollenTimer.Start(_pollenFirstTime);
    }

    private void Update() {
        _pollenTimer.Update(Time.deltaTime);
        if(HasPollen) {
            _pollenObj.SetActive(true);
        }
    }

    protected override void OnDestroy() {
        Pool.Remove(this);

        base.OnDestroy();
    }
#endregion

    public int Harvest() {
        int result =  Mathf.Min(_pollen, _pollenRate);
        _pollen -= result;
        _pollenObj.SetActive(false);

        if(_pollen <= 0) {
            Wither();
        }
        else {
            _pollenTimer.Start(_pollenDelayTime);
        }

        return result;
    }

    private void Wither() {
        IsDead = true;
        Destroy(gameObject, 0.1f);
    }

    public static ProxPool<NPCFlower> Pool = new ProxPool<NPCFlower>();

    public static NPCFlower Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist) as NPCFlower;
    }
}
