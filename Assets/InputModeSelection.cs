using System.Collections;
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
	public Text displayInputMode;
	controlType myCurrentcontrolType=controlType.keyboard;

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
	private void Start()
	{
		UpdateString();
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
