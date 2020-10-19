using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool IsGrounded => isGrounded;

    private bool isGrounded;



    private void Awake()
    {
        isGrounded = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 8 || other.gameObject.layer == 9)
        {
            isGrounded = false;
        }
    }
}
