using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFlower : NPCBase
{
    private static int kRate = 1;
    private static int kInitialPollen = 10;

    public int Pollen { get; private set; }

    public bool HasPollen() {
        return Pollen > 0;
    }

    public int Harvest() {
        int result = 0;
        if(Pollen > 0) {
            result = Mathf.Min(Pollen, kRate);
            Pollen -= result;

            if(Pollen == 0)
                Wither();
        }

        return result;
    }

    private void Wither() {
        // TODO death anim
    }

    private void Start() {
        Pool.Add(this);
    }

    protected override void OnDestroy() {
        Pool.Remove(this);

        base.OnDestroy();
    }

    private void Update() {
    }

    public static ProxPool<NPCFlower> Pool = new ProxPool<NPCFlower>();

    public static NPCFlower Nearest(Vector3 pos, float dist = 1000000.0f) {
        return Pool.Nearest(pos, dist) as NPCFlower;
    }
}
