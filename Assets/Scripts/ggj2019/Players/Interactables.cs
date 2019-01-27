using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Interactables : MonoBehaviour
{

    private Collider2D _trigger;

    private List<NPCBee> _bees = new List<NPCBee>();


    #region Unity Life Cycle

    private void Start()
    {
        _trigger = GetComponent<Collider2D>();

        _trigger.isTrigger = true;
    }

    #endregion

    public NPCBee GetBee()
    {
        if (_bees.Count < 1)
            return null;

        for (int i = 0; i < _bees.Count; i++)
        {
            if (_bees[i].CanJoinSwarm)
                return _bees[i];
        }

        return null;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        NPCBee npcBee = collider.GetComponent<NPCBee>();

        if(npcBee != null && !npcBee.IsInSwarm)
            _bees.Add(npcBee);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        NPCBee npcBee = collider.GetComponent<NPCBee>();

        if (npcBee != null)
            _bees.Remove(npcBee);
    }

}
