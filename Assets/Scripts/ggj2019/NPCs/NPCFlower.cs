using pdxpartyparrot.Core.Util;

using Spine;

using UnityEngine;

public class NPCFlower : NPCBase
{
    [SerializeField]
    private int _initialPollen = 5;

    [SerializeField]
    private int _pollenRate = 1;

    [SerializeField]
    private GameObject _floatingPollenObj;

    [SerializeField]
    [ReadOnly]
    private int _pollen;

    public int Pollen => _pollen;

    public bool HasPollen => Pollen > 0;

    public bool IsReady { get; private set; }

    [SerializeField]
    [ReadOnly]
    private bool _canSpawnPollen = true;

    public bool CanSpawnPollen
    {
        get => _canSpawnPollen;
        set => _canSpawnPollen = value;
    }

#region Unity Lifecycle
    protected override void Awake()
    {
        base.Awake();

        Pool.Add(this);

        _pollen = _initialPollen;
    }

    protected override void OnDestroy() {
        Pool.Remove(this);

        base.OnDestroy();
    }
#endregion

    public override void OnSpawn(GameObject spawnpoint)
    {
        base.OnSpawn(spawnpoint);

        IsReady = false;

        TrackEntry track = SetAnimation(0, "flower_grow", false);
        track.Complete += x => {
            IsReady = true;
            SetAnimation(0, "flower_idle", true);
        };
    }

    public void SpawnPollen()
    {
        Instantiate(_floatingPollenObj, transform.position, Quaternion.identity);
    }

    public override void Kill()
    {
        IsDead = true;

        TrackEntry track = SetAnimation(0, "flower_death", false);
        track.Complete += x => {
            base.Kill();
        };
    }

    public int BeetleHarvest()
    {
        int result =  Mathf.Min(_pollen, _pollenRate);
        _pollen -= result;

        if(_pollen <= 0) {
            Kill();
        }

        return result;
    }

    public static ProxPool<NPCFlower> Pool = new ProxPool<NPCFlower>();

    public static NPCFlower Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist) as NPCFlower;
    }
}
