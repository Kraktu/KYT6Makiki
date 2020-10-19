using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndZoneChecker : MonoBehaviour
{
    [SerializeField] private UnityEvent onEndZoneEntered = null;


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Rotator>()!=null)
        {
            onEndZoneEntered.Invoke();
        }
    }
}
