using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JetBrains.Annotations;

using UnityEngine;

public class ProxPool<T> where T : MonoBehaviour {

    private List<T> _objects;

    public void Add(T obj) {
        if(_objects == null)
            _objects = new List<T>();
        _objects.Add(obj);
    }

    public void Remove(T obj) {
        for(int i = 0; i < _objects.Count; ++i) {
            if(_objects[i] == obj) {
                _objects.RemoveAt(i);
                return;
            }
        }
    }

    public T Nearest(Vector3 pos, float maxdist = 100000000) {
        int best = -1;
        float bestT = maxdist;
        if(_objects == null)
            return null;

        for(int i = 0; i < _objects.Count; ++i) {
            float dist = (_objects[i].transform.position - pos).magnitude;
            if(dist < bestT) {
                bestT = dist;
                best = i;
            }
        }

        return (best >= 0) ? _objects[best] : null;
    }
}
