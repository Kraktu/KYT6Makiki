using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaloonBehaviour : MonoBehaviour
{
	public Vector3 addedHeight;
	public float fallTime;
	Coroutine falling;
	public void Despawn()
	{
		if (falling!=null)
		{
			StopCoroutine(falling);
		}
		
		transform.position = transform.position + addedHeight;
		falling = StartCoroutine(Fall());
	}


	IEnumerator Fall()
	{
		float time = 0;
		Vector3 startingPos = transform.position;
		Vector3 endingPos = transform.position - addedHeight;
		float tRatio;
		while (time<fallTime)
		{
			tRatio = time / fallTime;
			transform.position = Vector3.Lerp(startingPos, endingPos, tRatio);
			time += Time.deltaTime;
			yield return null;
		}
		transform.position = endingPos;
	}
}
