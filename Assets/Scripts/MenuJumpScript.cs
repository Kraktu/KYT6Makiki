using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuJumpScript : MonoBehaviour
{
	public bool isJumping;
		private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 8)
		{
			isJumping = false;
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.layer == 8)
		{
			isJumping = true;
		}
	}
}
