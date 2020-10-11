using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RasPacJam.Audio;

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
            AudioManager.Instance.Play("destruction");
			player.StuckChecker.CollisionsCount--;
			player.StopPauseDelay();
			player.destroyedObstacles.Add(other.gameObject);
			other.gameObject.SetActive(false);
		}

	}
}
