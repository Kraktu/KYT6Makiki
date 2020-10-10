using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckChecker : MonoBehaviour
{
    Player player;
    public int CollisionsCount {get; set;}

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        player.StuckChecker = this;
        CollisionsCount = 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == 8 || collider.gameObject.layer == 9)
        {
            CollisionsCount++;
            if(CollisionsCount == 1)
            {
                player.StartPauseDelay();
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.layer == 8 || collider.gameObject.layer == 9)
        {
            CollisionsCount--;
            if(CollisionsCount == 0)
            {
                player.StopPauseDelay();
            }
        }
    }
}
