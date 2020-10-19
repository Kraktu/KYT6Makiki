using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool CanRotate { get => canRotate; set => canRotate = value; }

    [SerializeField] private float speed = 10;
    private bool canRotate;



    private void Awake()
    {
        canRotate = true;
    }

    private void Update()
    {
        if(canRotate)
        {
            transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
        }
    }
}
