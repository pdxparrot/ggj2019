using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using pdxpartyparrot.ggj2019.Players;
using pdxpartyparrot.Game.State;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Collider2D))]
public class PollenCollectable: MonoBehaviour
{

    //TODO Use screen borrders
    [SerializeField]
    private float _sideDistance = 0.25f;
    [SerializeField]
    private float _sideSpeed = 0.5f;

    [SerializeField]
    private float _upwardSpeed = 0.001f;

    [SerializeField]
    private ParticleSystem _particleSystem;

    [SerializeField] public float _height = 4f;

    private int _pollen = 1;

    private Player followPlayer;
    private bool _hasFollowedPlayer = false;
    private bool _isCollected = false;

    private float _signTime = 0;

    private Vector3 _startPont = new Vector3();

    void Start()
    {
        _startPont = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isCollected)
        {

            Destroy(gameObject);
            return;
        }

        if (followPlayer != null)
        {
            if (followPlayer.IsDead)
            {
                followPlayer = null;
                return;
            }

            transform.position = Vector3.Lerp(transform.position, followPlayer.Position + new Vector3(0.25f,0f), 20f*Time.deltaTime);

            // pollen was deposited
            if (!followPlayer.HasPollen)
            {
                _isCollected = true;
            }
        }

        if(_hasFollowedPlayer)
            return;

        Move(Time.deltaTime);

        if (transform.position.y + -_height > GameStateManager.Instance.GameManager.GameData.GameSize2D)
        {
            Destroy(gameObject);
        }
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

        _hasFollowedPlayer = true;
        followPlayer = player;
        player.AddPollen(_pollen);
    }

}
