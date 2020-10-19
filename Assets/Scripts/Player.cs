using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RasPacJam.Audio;

public class Player : MonoBehaviour
{
    public Key jumpKey = Key.Joystick1_A;
    public Key punchKey = Key.Joystick1_B;
    public Key slideKey = Key.Joystick1_X;
    public Key stopTimeKey = Key.Joystick1_Y;


	#region Test Change Input Phil

	JumpController jumpController;
	ShrinkController shrinkController;
	BreakingWaveController breakingWaveController;
	FreezeController freezeController;

	private void Start()
	{
		jumpController = gameObject.GetComponent<JumpController>();
		shrinkController = gameObject.GetComponent<ShrinkController>();
		breakingWaveController = gameObject.GetComponent<BreakingWaveController>();
		freezeController = gameObject.GetComponent<FreezeController>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(GetKeyCode(jumpKey)))
		{
			jumpController.Jump();
		}
		if (Input.GetKeyDown(GetKeyCode(punchKey)))
		{
			breakingWaveController.LaunchBreakingWave();
		}
		if (Input.GetKeyDown(GetKeyCode(slideKey)))
		{
			shrinkController.Shrink();
		}
		if (Input.GetKeyDown(GetKeyCode(stopTimeKey)))
		{
			freezeController.Freeze(false);
		}
		if (Input.GetKeyUp(GetKeyCode(stopTimeKey)))
		{
			freezeController.Freeze(true);
		}
	}

	#endregion



	public static KeyCode GetKeyCode(Key key)
    {
        switch(key)
        {
            case Key.A :
                return KeyCode.A;
            case Key.Z :
                return KeyCode.Z;
            case Key.E :
                return KeyCode.E;
            case Key.R :
                return KeyCode.R;
            case Key.Joystick1_A :
                return KeyCode.Joystick1Button0;
            case Key.Joystick1_B :
                return KeyCode.Joystick1Button1;
            case Key.Joystick1_X :
                return KeyCode.Joystick1Button2;
            case Key.Joystick1_Y :
                return KeyCode.Joystick1Button3;
            case Key.Joystick2_A :
                return KeyCode.Joystick2Button0;
            case Key.Joystick2_B :
                return KeyCode.Joystick2Button1;
            case Key.Joystick2_X :
                return KeyCode.Joystick2Button2;
            case Key.Joystick2_Y :
                return KeyCode.Joystick2Button3;
            case Key.Joystick3_A :
                return KeyCode.Joystick3Button1;
            case Key.Joystick3_B :
                return KeyCode.Joystick3Button2;
            case Key.Joystick3_X :
                return KeyCode.Joystick3Button3;
            case Key.Joystick3_Y :
                return KeyCode.Joystick3Button4;
            case Key.Joystick4_A :
                return KeyCode.Joystick4Button0;
            case Key.Joystick4_B :
                return KeyCode.Joystick4Button1;
            case Key.Joystick4_X :
                return KeyCode.Joystick4Button2;
            case Key.Joystick4_Y :
                return KeyCode.Joystick4Button3;
            default :
                return KeyCode.A;
        }
    }

    public enum Key
    {
        A,
        Z,
        E,
        R,
        Joystick1_A,
        Joystick1_B,
        Joystick1_X,
        Joystick1_Y,
        Joystick2_A,
        Joystick2_B,
        Joystick2_X,
        Joystick2_Y,
        Joystick3_A,
        Joystick3_B,
        Joystick3_X,
        Joystick3_Y,
        Joystick4_A,
        Joystick4_B,
        Joystick4_X,
        Joystick4_Y,
    }
}
