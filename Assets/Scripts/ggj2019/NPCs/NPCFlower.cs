using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core;
using Spine;
using UnityEngine;

public class NPCFlower : NPCBase
{
    [SerializeField] private int _initialPollen = 5;
    [SerializeField] private int _pollenRate = 1;
    private static float _pollenDelayMin = 3;
    private static float _pollenDelayMax = 6;

    [SerializeField] private GameObject _floatingPollenObj;

    [SerializeField] private GameObject _pollenObj;

    [SerializeField]
    [ReadOnly]
    private int _pollen;

    public int Pollen => _pollen;

    public bool HasPollen => Pollen > 0 && _hasPollen;
    private bool _hasPollen = false;


    public bool IsReady { get; private set; }

    public bool CanSpawnPollen = true;

    private static int _pollenTimerFrame;
    private static Timer _pollenTimer = new Timer();

#region Unity Lifecycle
    protected override void Awake() {
        base.Awake();

        _pollen = _initialPollen;
    }

    private void Start() {
        Pool.Add(this);

        _pollenTimer.Start(PartyParrotManager.Instance.Random.NextSingle(_pollenDelayMin, _pollenDelayMax));
    }

    private void Update() {

        if(!CanSpawnPollen || !IsReady)
            return;

        if(_pollenTimerFrame != Time.frameCount) {
            _pollenTimerFrame = Time.frameCount;
            _pollenTimer.Update(Time.deltaTime);
            if(!_pollenTimer.IsRunning) {
                for(int i = 0; i < 10; ++i) {
                    // -- give it to a random flower
                    var f = Pool.Random() as NPCFlower;
                    if(f && f.IsReady && !f._hasPollen) {
                        f._hasPollen = true;
                        break;
                    }
                }
            _pollenTimer.Start(PartyParrotManager.Instance.Random.NextSingle(_pollenDelayMin, _pollenDelayMax));
            }
        }

        if(HasPollen)
        {
            Instantiate(_floatingPollenObj, transform.position,
                Quaternion.identity);

            _hasPollen = false;
        }

        if (HasPollen)
        {
            _pollenObj.SetActive(true);
        }
    }


    protected override void OnDestroy() {
        Pool.Remove(this);

        base.OnDestroy();
    }
#endregion

    public override void OnSpawn(GameObject spawnpoint) {
        base.OnSpawn(spawnpoint);

        IsReady = false;

        TrackEntry track = SetAnimation(0, "flower_grow", false);
        track.Complete += x => {
            IsReady = true;
            SetAnimation(0, "flower_idle", true);
        };
    }

    public override void Kill() {
        IsDead = true;

        TrackEntry track = SetAnimation(0, "flower_death", false);
        track.Complete += x => {
            base.Kill();
        };
    }

    public int Harvest(bool beetle = false) {
        int result = 0;

        if(_hasPollen || beetle) {
            _hasPollen = false;
            result =  Mathf.Min(_pollen, _pollenRate);
            if(beetle)
                _pollen -= result;
            _pollenObj.SetActive(false);

            if(_pollen <= 0) {
                Kill();
            }
        }

        return result;
    }

    public static ProxPool<NPCFlower> Pool = new ProxPool<NPCFlower>();

    public static NPCFlower Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist) as NPCFlower;
    }
}
