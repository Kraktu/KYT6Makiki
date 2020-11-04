using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RasPacJam.Audio;

public class EndZoneChecker : MonoBehaviour
{
    [SerializeField] private UnityEvent onEndZoneEntered = null;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 12)
        {
            AudioManager.Instance.Play("nextLightingLevel");
            onEndZoneEntered.Invoke();
            Destroy(other.gameObject);
            transform.GetComponentInParent<AutoRun>().enabled = false;
            transform.GetComponentInParent<PlayerInput>().enabled = false;
            transform.GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
            transform.parent.GetComponentInChildren<Rotator>().enabled = false;

            foreach(Collider childCollider in transform.parent.GetComponentsInChildren<Collider>())
            {
                if(!childCollider.isTrigger)
                {
                    continue;
                }
                childCollider.enabled = false;
            }
        }
    }
}
