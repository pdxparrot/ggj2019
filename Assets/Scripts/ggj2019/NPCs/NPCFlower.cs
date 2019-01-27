using pdxpartyparrot.Core.Util;
using UnityEngine;

public class NPCFlower : NPCBase
{
    [SerializeField]
    private int _initialPollen = 10;

    [SerializeField]
    private int _pollenRate = 1;

    [SerializeField]
    [ReadOnly]
    private int _pollen;

    public int Pollen => _pollen;

    public bool HasPollen => Pollen > 0;

    public bool IsDead { get; private set; }

#region Unity Lifecycle
    protected override void Awake() {
        base.Awake();

        _pollen = _initialPollen;
    }

    private void Start() {
        Pool.Add(this);
    }

    protected override void OnDestroy() {
        Pool.Remove(this);

        base.OnDestroy();
    }
#endregion

    public int Harvest() {
        int result =  Mathf.Min(_pollen, _pollenRate);
        _pollen -= result;
        if(_pollen <= 0) {
            Wither();
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
