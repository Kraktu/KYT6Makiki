using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBreaking : MonoBehaviour
{
	Player player;
	private void Start()
	{
		player = GetComponentInParent<Player>();
	}
	private void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.layer == 9)
		{
			player.StuckChecker.CollisionsCount--;
			player.StopPauseDelay();
			player.destroyedObstacles.Add(other.gameObject);
			other.gameObject.SetActive(false);
		}
	
	}
}
