using pdxpartyparrot.Core;
using pdxpartyparrot.ggj2019.Players;
using pdxpartyparrot.Game.Effects;
using pdxpartyparrot.Game.State;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PollenCollectable: MonoBehaviour
{

    [SerializeField]
    private float _sideDistance = 0.25f;

    [SerializeField]
    private float _sideSpeed = 0.5f;

    [SerializeField]
    private float _upwardSpeed = 0.001f;

    [SerializeField]
    private ParticleSystem _particleSystem;

    [SerializeField]
    private EffectTrigger _pickupEffect;

    [SerializeField]
    private EffectTrigger _collectEffect;

    [SerializeField] public float _height = 4f;

    private int _pollen = 1;

    private Player followPlayer;

    private bool _isCollected = false;

    private float _signTime = 0;

    private Vector3 _startPont = new Vector3();

    private Hive _hive;

    void Start()
    {
        _startPont = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (PartyParrotManager.Instance.IsPaused)
            return;

        if (_isCollected)
        {
            if(!_particleSystem.isPlaying)
                Destroy(gameObject);

            GoToHive();
            return;
        }

        if (FollowPlayer())
        {
            return;
        }

        Move(Time.deltaTime);

        if (transform.position.y  - _height / 2.0f > GameStateManager.Instance.GameManager.GameData.GameSize2D)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    private void GoToHive()
    {
        transform.position = Vector3.Lerp(transform.position, _hive.Position, 10f * Time.deltaTime);
    }

    private bool FollowPlayer()
    {
        if ( followPlayer == null)
        {
            return false;
        }

        if (followPlayer.IsDead)
        {
            followPlayer = null;
            return false;
        }

        transform.position = Vector3.Lerp(transform.position, followPlayer.Position + new Vector3(0.25f,0f), 20f*Time.deltaTime);

        // pollen was deposited
        if (!followPlayer.HasPollen)
        {
            Collect();
        }

        return true;
    }

    private void Move(float dt)
    {
        _signTime += dt * _sideSpeed;

        transform.position = new Vector3((Mathf.Sin(_signTime) * _sideDistance) + _startPont.x,
            transform.position.y + (_upwardSpeed * dt));
    }

    public void SetPollenAmt(int amt)
    {
        _pollen = amt;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_isCollected)
            return;

        Player player = other.gameObject.GetComponent<Player>();
        if (null == player)
        {
            return;
        }

        if (player.HasPollen)
            return;

        followPlayer = player;

        player.AddPollen(_pollen);
        _pickupEffect.Trigger();
    }

    private void Collect()
    {
        _isCollected = true;
        _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        _hive = Hive.Nearest(transform.position);

        _collectEffect.Trigger(() => {
            Destroy(gameObject);
        });
    }
}
