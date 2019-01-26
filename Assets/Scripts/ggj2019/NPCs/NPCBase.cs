using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using pdxpartyparrot.Core.Actors;


public class NPCBase : Actor
{
    void Start() {
    }

    void Update() {
    }

    [SerializeField] private float height;
    [SerializeField] private float radius;

    public override float Height { get { return height; } }
    public override float Radius { get { return radius; } }

    public override bool IsLocalActor { get { return true; } }

    public override void OnSpawn() { }
    public override void OnReSpawn() { }
}
