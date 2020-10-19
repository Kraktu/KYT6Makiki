using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectiblesChecker : MonoBehaviour
{
    [SerializeField] private UnityEvent onCollected = null;



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            onCollected.Invoke();
            Destroy(other.gameObject);
        }
    }
}
