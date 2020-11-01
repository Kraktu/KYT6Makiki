using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndZoneChecker : MonoBehaviour
{
    [SerializeField] private UnityEvent onEndZoneEntered = null;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 12)
        {
            onEndZoneEntered.Invoke();
            Destroy(other.gameObject);
        }
    }
}
