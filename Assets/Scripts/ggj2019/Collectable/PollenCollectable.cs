using System.Collections;
using System.Collections.Generic;
using pdxpartyparrot.ggj2019.Players;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

[RequireComponent(typeof(Collider2D))]
public class PollenCollectable: MonoBehaviour
{

    //TODO Use screen borrders
    [SerializeField]
    private Vector2 FloatUpAmount = new Vector2(5f,10f);

    [SerializeField] private float _upwardSpeed = 0.001f;

    [SerializeField]
    private ParticleSystem _particleSystem;

    private int _pollen = 1;

    private Player followPlayer;
    private bool _hasFollowedPlayer = false;
    private bool _isCollected = false;
    private float _yTargetPosition = 0;

    void Start()
    {
        _yTargetPosition = Random.Range(FloatUpAmount.x, FloatUpAmount.y) + transform.position.y;
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

            transform.position = followPlayer.Position;

            // pollen was deposited
            if (!followPlayer.HasPollen)
            {
                _isCollected = true;
            }
        }

        if(_hasFollowedPlayer)
            return;

        Vector3 newPos = transform.position + new Vector3(0,_upwardSpeed);
        newPos = new Vector3(newPos.x, Mathf.Clamp(newPos.y, -1000000, _yTargetPosition), newPos.z);
        transform.position = newPos;

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
