using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundSpin : MonoBehaviour
{
	public float speed = 10;
	private void Update()
	{
		transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
	}
}
