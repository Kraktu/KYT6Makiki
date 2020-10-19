using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoofChecker : MonoBehaviour
{
    public bool IsRoofed => isRoofed;

    [SerializeField] private UnityEvent onRoofExited = null;
    private bool isRoofed;



    private void Awake()
    {
        isRoofed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            isRoofed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            isRoofed = false;
            onRoofExited.Invoke();
        }
    }
}
