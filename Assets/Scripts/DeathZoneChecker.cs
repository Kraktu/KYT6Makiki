using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathZoneChecker : MonoBehaviour
{
    [SerializeField] private UnityEvent onDeathZoneEntered = null;



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11)
        {
            onDeathZoneEntered.Invoke();
        }
    }
}
