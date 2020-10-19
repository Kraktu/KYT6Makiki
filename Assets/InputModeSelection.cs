﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum controlType
{
	keyboard,
	oneControler,
	twoControler,
	fourControler
}
public class InputModeSelection : MonoBehaviour
{
	public static InputModeSelection Instance => instance;
	private static InputModeSelection instance;
	public Text displayInputMode;
	public PlayerInput player1, player2, player3, player4;
	[HideInInspector]	
	public PlayerInput.Key key1, key2, key3, key4;
	controlType myCurrentcontrolType=controlType.keyboard;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(this);
	}
	private void Start()
	{
		UpdateString();
		UpdateInput();
	}

	private void Update()
	{

		if (Input.GetKeyDown(KeyCode.Q)||Input.GetKeyDown(KeyCode.LeftArrow))
		{
			BeforeInputMode();
		}
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) 
		{
			NextInputMode();
		}
	}

	public void NextInputMode()
	{
		
		if (myCurrentcontrolType==controlType.fourControler)
		{
			myCurrentcontrolType = controlType.keyboard;
		}
		else
		{
			myCurrentcontrolType++;
		}
		UpdateString();
		UpdateInput();
	}
	public void BeforeInputMode()
	{
		if (myCurrentcontrolType == controlType.keyboard)
		{
			myCurrentcontrolType = controlType.fourControler;
		}
		else
		{
			myCurrentcontrolType--;
		}

		UpdateString();
		UpdateInput();
	}

	public void UpdateInput()
	{
		switch (myCurrentcontrolType)
		{
			case controlType.keyboard:
				key1 = player1.jumpButton = PlayerInput.Key.A;
				key2 = player2.shrinkButton = PlayerInput.Key.Z;
				key3 = player3.breakingWaveButton = PlayerInput.Key.E;
				key4 = player4.freezeButton = PlayerInput.Key.R;
				break;
			case controlType.oneControler:
				key1 = player1.jumpButton = PlayerInput.Key.Joystick1_A;
				key2 = player2.shrinkButton = PlayerInput.Key.Joystick1_B;
				key3 = player3.breakingWaveButton = PlayerInput.Key.Joystick1_X;
				key4 = player4.freezeButton = PlayerInput.Key.Joystick1_Y;
				break;
			case controlType.twoControler:
				key1 = player1.jumpButton = PlayerInput.Key.Joystick1_A;
				key2 = player2.shrinkButton = PlayerInput.Key.Joystick1_B;
				key3 = player3.breakingWaveButton = PlayerInput.Key.Joystick2_A;
				key4 = player4.freezeButton = PlayerInput.Key.Joystick2_B;
				break;
			case controlType.fourControler:
				key1 = player1.jumpButton = PlayerInput.Key.Joystick1_A;
				key2 = player2.shrinkButton = PlayerInput.Key.Joystick2_A;
				key3 = player3.breakingWaveButton = PlayerInput.Key.Joystick3_A;
				key4 = player4.freezeButton = PlayerInput.Key.Joystick4_A;
				break;
			default:
				break;
		}
	}
	public void UpdateString()
	{
		switch (myCurrentcontrolType)
		{
			case controlType.keyboard:
				displayInputMode.text = "Keyboard (AZER)";
				break;
			case controlType.oneControler:
				displayInputMode.text = "1 Controler (ABXY)";
				break;
			case controlType.twoControler:
				displayInputMode.text = "2 Controler (AB)";
				break;
			case controlType.fourControler:
				displayInputMode.text = "4 Controler (A)";
				break;
			default:
				break;
		}
		
	}
}
