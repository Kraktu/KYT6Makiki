using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RasPacJam.Audio;

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

	Vector3 playerSize;
	public Material redMat, greenMat, basicMat,emissivMat,matPlay;
	private void Start()
	{
		rbPlayer1 = player1.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(Player.GetKeyCode(jumpKey)))
		{
            AudioManager.Instance.Play("jump");
			if (player1.GetComponent<MenuJumpScript>().isJumping==false)
			{
				rbPlayer1.velocity = new Vector3(rbPlayer1.velocity.x, 10, rbPlayer1.velocity.z);
			}

			pLetter.material = emissivMat;
			playerOneReady = true;
		}
		else if (Input.GetKeyUp(Player.GetKeyCode(jumpKey)))
		{
			pLetter.material = matPlay;
			playerOneReady = false;
		}
		if (Input.GetKeyDown(Player.GetKeyCode(slideKey)))
		{
            AudioManager.Instance.Play("unscale");
			playerTwoReady = true;
			lLetter.material = emissivMat;
			player2.transform.localScale = new Vector3(player2.transform.localScale.x/2, player2.transform.localScale.y / 2, player2.transform.localScale.z/2);
		}
		else if (Input.GetKeyUp(Player.GetKeyCode(slideKey)))
		{
            AudioManager.Instance.Play("rescale");
			playerTwoReady = false;
			player2.transform.localScale = player2.transform.localScale*2;
			lLetter.material = matPlay;
		}

		if (Input.GetKeyDown(Player.GetKeyCode(stopTimeKey)))
		{
            AudioManager.Instance.Play("stop");
			playerThreeReady = true;
			aLetter.material = emissivMat;
			background.speed = 0;

		}
		else if (Input.GetKeyUp(Player.GetKeyCode(stopTimeKey)))
		{
			playerThreeReady = false;
			aLetter.material = matPlay;
			background.speed = 50;
		}
		if (Input.GetKeyDown(Player.GetKeyCode(punchKey)))
		{
            AudioManager.Instance.Play("breakingWave");
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
			yLetter.material = matPlay;
		}

		if (playerOneReady&&playerTwoReady&&playerThreeReady&&playerFourReady)
		{
            AudioManager.Instance.Play("start");
			sceneLoader.LoadLevel(1);
			Destroy(this);
		}
	}
}
