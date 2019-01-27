﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using pdxpartyparrot.ggj2019.Players;

public class NPCWasp : NPCEnemy
{
    [SerializeField] private float MaxVel;
    [SerializeField] private float Accel;
   
    private Vector3 _accel;
    private Vector3 _velocity;

    void Start() {
        float dir = (transform.position.x > 0) ? -1 : 1;
        _accel = new Vector3(1,0,0) * Accel * dir;
    }

    void Update() {
        _velocity += _accel * Time.deltaTime;
        _velocity = Vector3.ClampMagnitude(_velocity, MaxVel);
        transform.position += _velocity * Time.deltaTime;

        var hive = Hive.Nearest(transform.position);
        if(hive.Collides(transform.position)) {
            hive.TakeDamage(transform.position);
            Die();
        }
    }

    void Die() {
        // TODO effects
        Destroy(gameObject, 0.01f);
    }
}
