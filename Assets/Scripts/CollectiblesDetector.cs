using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectiblesDetector : MonoBehaviour
{
    [SerializeField] private UnityEvent onNewCollectible = null;



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            onNewCollectible.Invoke();
            if(other.TryGetComponent<AnimatorCollectible>(out AnimatorCollectible collectible))
            {
                collectible.Explode();
            }
        }
    }
}
