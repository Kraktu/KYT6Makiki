using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjects : MonoBehaviour
{
	public float speed;

	private void Start()
	{
		InitializeMe();
	}
	private void Update()
	{
		AutoMove();
	}
	protected virtual void InitializeMe()
	{

	}
	protected void AutoMove()
	{
		transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
		
	}
}
