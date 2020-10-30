using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool CanRotate { get => canRotate; set => canRotate = value; }

    [SerializeField] private Vector3 rotationSpeed = Vector3.zero;
    [SerializeField] private Vector3 rotationVariations = Vector3.zero;
    private bool canRotate;
    private Vector3 variationsTimers;



    private void Awake()
    {
        canRotate = true;
        variationsTimers = rotationVariations;
    }

    private void Update()
    {
        if(canRotate)
        {
            if(rotationVariations.x != 0)
            {
                variationsTimers.x -= Time.deltaTime;
                if(variationsTimers.x <= 0)
                {
                    rotationSpeed.x = - rotationSpeed.x;
                    variationsTimers.x = rotationVariations.x;
                }
            }
            if(rotationVariations.y != 0)
            {
                variationsTimers.y -= Time.deltaTime;
                if(variationsTimers.y <= 0)
                {
                    rotationSpeed.y = - rotationSpeed.y;
                    variationsTimers.y = rotationVariations.y;
                }
            }
            if(rotationVariations.z != 0)
            {
                variationsTimers.z -= Time.deltaTime;
                if(variationsTimers.z <= 0)
                {
                    rotationSpeed.z = - rotationSpeed.z;
                    variationsTimers.z = rotationVariations.z;
                }
            }
            transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
