using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovable : MonoBehaviour
{
    public float Speed { get; set; }
    void LateUpdate()
    {
        transform.Translate(new Vector3(1, 0, 0) * Speed * Time.deltaTime, Space.World);
    }
}
