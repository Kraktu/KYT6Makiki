using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Player player = null;
    public Vector3 offset = new Vector3(0f, 5f, -40f);

    public void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
