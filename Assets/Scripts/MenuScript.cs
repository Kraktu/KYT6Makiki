using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public Player.Key jumpKey = Player.Key.Joystick1_A;
    public Player.Key punchKey = Player.Key.Joystick1_B;
    public Player.Key slideKey = Player.Key.Joystick1_X;
    public Player.Key stopTimeKey = Player.Key.Joystick1_Y;
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
		if (Input.GetKeyDown(Player.GetKeyCode(jumpKey)))
		{
			rbPlayer1.velocity = new Vector3(rbPlayer1.velocity.x, 10, rbPlayer1.velocity.z);
			pLetter.material = emissivMat;
			playerOneReady = true;
		}
		else if (Input.GetKeyUp(Player.GetKeyCode(jumpKey)))
		{
			pLetter.material = basicMat;
			playerOneReady = false;
		}
		if (Input.GetKeyDown(Player.GetKeyCode(slideKey)))
		{
			playerTwoReady = true;
			lLetter.material = emissivMat;
			player2.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
		}
		else if (Input.GetKeyUp(Player.GetKeyCode(slideKey)))
		{
			playerTwoReady = false;
			player2.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
			lLetter.material = basicMat;
		}

		if (Input.GetKeyDown(Player.GetKeyCode(stopTimeKey)))
		{
			playerThreeReady = true;
			aLetter.material = emissivMat;
			background.speed = 0;

		}
		else if (Input.GetKeyUp(Player.GetKeyCode(stopTimeKey)))
		{
			playerThreeReady = false;
			aLetter.material = basicMat;
			background.speed = 10;
		}
		if (Input.GetKeyDown(Player.GetKeyCode(punchKey)))
		{
			playerFourReady = true;
			yLetter.material = emissivMat;
			somethingToDestroy.enabled = false;
			player4.GetComponent<MeshRenderer>().material = redMat;
		}
		else if (Input.GetKeyUp(Player.GetKeyCode(punchKey)))
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
