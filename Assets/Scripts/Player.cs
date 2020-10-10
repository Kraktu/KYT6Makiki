using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public StuckChecker StuckChecker {get; set;}
    public Key jumpKey = Key.Joystick1_A;
    public Key punchKey = Key.Joystick1_B;
    public Key slideKey = Key.Joystick1_X;
    public Key stopTimeKey = Key.Joystick1_Y;
    public float speed = 10f;
	public float jumpHeight = 5f;
	public float breakingWallTime, slidingTime;
	bool isJumping = false, isBreakingWall=false, isSliding=false;
	public Material redMat, greenMat, basicMat;
    public List<Light> lights;
    public float timeStoppedDelay = 1f;
	Rigidbody myRigidbody;
    private bool isStoppingTime;
    private List<Tween> switchLightTweens;



	void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();
        switchLightTweens = new List<Tween>();
        isStoppingTime = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(GetKeyCode(jumpKey)) && !isJumping)
		{
			Jump();
		}
		if (Input.GetKeyDown(GetKeyCode(punchKey))&&!isBreakingWall)
		{
			Punch();
		}
		if (Input.GetKeyDown(GetKeyCode(slideKey))&&!isSliding)
		{
			Slide();
		}
        if(Input.GetKeyDown(GetKeyCode(stopTimeKey)))
        {
            StopTime();
        }
        else if(Input.GetKeyUp(GetKeyCode(stopTimeKey)))
        {
            RestartTime();
        }
        if(!isStoppingTime)
        {
            myRigidbody.velocity = new Vector3(speed, myRigidbody.velocity.y, myRigidbody.velocity.z);
        }
	}

	void Jump()
	{
		isJumping = true;
        myRigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
	}

	void Punch()
	{
		StartCoroutine(BreakingWall());
	}
	IEnumerator BreakingWall()
	{
		isBreakingWall = true;
		GetComponent<MeshRenderer>().material = redMat;
		float time = 0;
		while (time<breakingWallTime)
		{
			time += Time.deltaTime;
			yield return null;
		}
		GetComponent<MeshRenderer>().material = basicMat;
		isBreakingWall = false;
	}

	void Slide()
	{
		StartCoroutine(Sliding());
	}

	IEnumerator Sliding()
	{
		isSliding = true;
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
		GetComponent<MeshRenderer>().material = greenMat;
		float time = 0;
		while (time < slidingTime)
		{
			time += Time.deltaTime;
			yield return null;
		}
		GetComponent<MeshRenderer>().material = basicMat;
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
		isSliding = false;
	}

    private void StopTime()
    {
        if(StuckChecker.CollisionsCount > 0)
        {
            return;
        }
        myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, myRigidbody.velocity.z);
        isStoppingTime = true;
        StartPauseDelay();
    }

    private void RestartTime()
    {
        if(StuckChecker.CollisionsCount > 0)
        {
            return;
        }
        isStoppingTime = false;
        StopPauseDelay();
    }

    public void StartPauseDelay()
    {
        foreach(Tween tween in switchLightTweens)
        {
            tween.Kill();
        }
        switchLightTweens.Clear();

        foreach(Light light in lights)
        {
            Tween tween = light.DOIntensity(0f, timeStoppedDelay).OnComplete(() => EndGame());
            switchLightTweens.Add(tween);
        }
    }

    public void StopPauseDelay()
    {
        foreach(Tween tween in switchLightTweens)
        {
            tween.Kill();
        }
        switchLightTweens.Clear();

        foreach(Light light in lights)
        {
            Tween tween = light.DOIntensity(1f, 0.3f).SetEase(Ease.OutQuint);
            switchLightTweens.Add(tween);
        }
    }

    private void EndGame()
    {

    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 8)
		{
			isJumping = false;
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.layer == 9 && isBreakingWall)
		{
            StuckChecker.CollisionsCount--;
            StopPauseDelay();
			Destroy(collision.gameObject);
		}
	}

    private KeyCode GetKeyCode(Key key)
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
