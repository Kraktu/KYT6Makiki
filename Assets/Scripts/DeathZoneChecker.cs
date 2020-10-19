using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathZoneChecker : MonoBehaviour
{
    [SerializeField] private UnityEvent onDeathZoneEntered = null;


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Rotator>()!=null)
        {
            onDeathZoneEntered.Invoke();
        }
    }
}
