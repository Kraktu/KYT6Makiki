using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovable : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * 5 * Time.deltaTime);
    }
}
