using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
	public SceneLoader sceneLoader;
	public GameObject player1, player2, player3, player4;
	Rigidbody rbPlayer1;
	public MeshRenderer pLetter, lLetter, aLetter, yLetter, somethingToDestroy;
	public MenuBackgroundSpin background;
	bool playerOneReady, playerTwoReady, playerThreeReady, playerFourReady;

	public Material redMat, greenMat, basicMat,emissivMat;
	private void Start()
	{
		rbPlayer1 = player1.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			rbPlayer1.velocity = new Vector3(rbPlayer1.velocity.x, 10, rbPlayer1.velocity.z);
			pLetter.material = emissivMat;
			playerOneReady = true;
		}
		else if (Input.GetKeyUp(KeyCode.A))
		{
			pLetter.material = basicMat;
			playerOneReady = false;
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			playerTwoReady = true;
			lLetter.material = emissivMat;
			player2.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
		}
		else if (Input.GetKeyUp(KeyCode.Z))
		{
			playerTwoReady = false;
			player2.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
			lLetter.material = basicMat;
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			playerThreeReady = true;
			aLetter.material = emissivMat;
			background.speed = 0;

		}
		else if (Input.GetKeyUp(KeyCode.E))
		{
			playerThreeReady = false;
			aLetter.material = basicMat;
			background.speed = 10;
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			playerFourReady = true;
			yLetter.material = emissivMat;
			somethingToDestroy.enabled = false;
			player4.GetComponent<MeshRenderer>().material = redMat;
		}
		else if (Input.GetKeyUp(KeyCode.R))
		{
			playerFourReady = false;
			somethingToDestroy.enabled = true;
			player4.GetComponent<MeshRenderer>().material = basicMat;
			yLetter.material = basicMat;
		}

		if (playerOneReady&&playerTwoReady&&playerThreeReady&&playerFourReady)
		{
			
			sceneLoader.LoadLevel(1);
			Destroy(this);
		}
	}
}
