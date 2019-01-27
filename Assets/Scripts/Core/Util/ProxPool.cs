using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pdxpartyparrot.Core;
using pdxpartyparrot.Core.Util;

public class ProxPool<T> where T : MonoBehaviour {

    private readonly List<T> _objects = new List<T>();

    public int Count => _objects.Count;

    public void Add(T obj) {
        _objects.Add(obj);
    }

    public void Remove(T obj) {
        _objects.Remove(obj);
    }

    public T Nearest(Vector3 pos, float maxdist = 100000000) {
        int best = -1;
        float bestT = maxdist;

        for(int i = 0; i < _objects.Count; ++i) {
            float dist = (_objects[i].transform.position - pos).magnitude;
            if(dist < bestT) {
                bestT = dist;
                best = i;
            }
        }

        return (best >= 0) ? _objects[best] : null;
    }

    public T Random() {
        int i = PartyParrotManager.Instance.Random.Next(_objects.Count);
        return _objects[i];
    }
}
