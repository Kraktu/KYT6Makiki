using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofedScript : MonoBehaviour
{
	public bool isRoofed=false;
	public bool detectExit = false;
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.layer==8||other.gameObject.layer==9)
		{
			isRoofed = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
		{
			isRoofed = false;
			if (detectExit == true && isRoofed == false)
			{
				GetComponentInParent<Player>().ExitSlide();
			}
		}

	}
}
