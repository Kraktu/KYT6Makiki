using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RasPacJam.Audio;

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
    private bool mustDie;
    [HideInInspector] public bool isReturningToStart;
    private List<Tween> switchLightTweens;
    private Tween cameraTween;
	Vector3 startingPosition,initialCamerPosition;
    float initialCameraOffset = -15f;
    float farCameraOffset = -40f;


	//Phil joue au codeur

	public int lightStep;
	int currentNbrOfGems;
	public float lightIntesityAdded;
	public float currentLightIntensity = 1;

	// Phil Reprend le contrôle

	public List<GameObject> destroyedObstacles = new List<GameObject>();
	RoofedScript roofedScript;
	public Vector3 sizeWhenDestroying=new Vector3(3,3,3);
	public Light[] firstBatchOfLights,secondBatchOfLights,thirdBatchOfLights;
	Light myLight;
	WaveBreaking waveBreakingScript;


	void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();
        switchLightTweens = new List<Tween>();
        mustDie = false;
        isReturningToStart = false;
        isStoppingTime = false;
		startingPosition = transform.position;
		initialCamerPosition = Camera.main.transform.position;
		roofedScript = GetComponentInChildren<RoofedScript>();
		roofedScript.gameObject.SetActive(false);
		myLight = GetComponentInChildren<Light>();
		waveBreakingScript = GetComponentInChildren<WaveBreaking>();

	}

	// Update is called once per frame
	void Update()
	{
        if(isReturningToStart)
        {
            return;
        }

        if(mustDie)
        {
            Die();
        }

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
		GetComponentInChildren<MeshRenderer>().material = redMat;
		float time = 0;
		float tRatio;
		//Vector3 initialSize = gameObject.transform.localScale;
		waveBreakingScript.gameObject.GetComponent<SphereCollider>().enabled = true;
		while (time<breakingWallTime)
		{
			tRatio = time / breakingWallTime;
			myLight.intensity = Mathf.Lerp(0, 80, tRatio);
			myLight.gameObject.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2, tRatio);
			//transform.localScale = Vector3.Lerp(initialSize, sizeWhenDestroying, tRatio);
			time += Time.deltaTime;
			yield return null;
		}
		waveBreakingScript.gameObject.GetComponent<SphereCollider>().enabled = false;
		myLight.intensity = 0;
	//	transform.localScale = initialSize;
		GetComponentInChildren<MeshRenderer>().material = basicMat;
		isBreakingWall = false;
	}

	void Slide()
	{
		StartCoroutine(Sliding());
	}

	IEnumerator Sliding()
	{
		roofedScript.gameObject.SetActive(true);
		isSliding = true;
		transform.localScale = new Vector3(transform.localScale.x/2, transform.localScale.y / 2, transform.localScale.z/2);
		GetComponentInChildren<MeshRenderer>().material = greenMat;
		float time = 0;
		while (time < slidingTime)
		{
			time += Time.deltaTime;
			yield return null;
		}
		if (roofedScript.isRoofed==true)
		{
			roofedScript.detectExit = true;
		}
		else
		{
			ExitSlide();
		}

	}
	public void ExitSlide()
	{
		if (isSliding)
		{
			GetComponentInChildren<MeshRenderer>().material = basicMat;
			transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, transform.localScale.z * 2);
			isSliding = false;
		}

	}

    private void StopTime()
    {
        if(StuckChecker.CollisionsCount > 0)
        {
            return;
        }
        FollowingCamera camera = Camera.main.GetComponent<FollowingCamera>();
        if(cameraTween != null)
        {
            cameraTween.Kill();
        }
        cameraTween = DOTween.To(() => camera.offset, value => camera.offset = value, new Vector3(camera.offset.x, camera.offset.y, farCameraOffset), 1f).SetEase(Ease.OutQuint);
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

        FollowingCamera camera = Camera.main.GetComponent<FollowingCamera>();
        if(cameraTween != null)
        {
            cameraTween.Kill();
        }
        cameraTween = DOTween.To(() => camera.offset, value => camera.offset = value, new Vector3(camera.offset.x, camera.offset.y, initialCameraOffset), 1f).SetEase(Ease.OutQuint);
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
            Tween tween = light.DOIntensity(0f, timeStoppedDelay).OnComplete(() => mustDie = true);
            switchLightTweens.Add(tween);
        }

        switchLightTweens.Add(AudioManager.Instance.musicReverb.DOFade(1f, timeStoppedDelay));
        switchLightTweens.Add(AudioManager.Instance.music.DOFade(0f, timeStoppedDelay));
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
            Tween tween = light.DOIntensity(currentLightIntensity, 0.3f).SetEase(Ease.OutQuint);
            switchLightTweens.Add(tween);
        }

        switchLightTweens.Add(AudioManager.Instance.musicReverb.DOFade(0f, 0.3f).SetEase(Ease.OutQuint));
        switchLightTweens.Add(AudioManager.Instance.music.DOFade(0.5f, 0.3f).SetEase(Ease.OutQuint));
    }

    private void Die()
    {
        StuckChecker.CollisionsCount = 0;
        mustDie = false;
        foreach(Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        GetComponent<Rigidbody>().isKinematic = true;
        isReturningToStart = true;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveZ(-3f, 2f).SetEase(Ease.InQuint))
        .Append(transform.DOMove(startingPosition + new Vector3(0,0,-3), 2f).SetEase(Ease.InQuint))
        .Append(transform.DOMove(startingPosition, 2f).SetEase(Ease.InQuint).OnComplete(() =>
                {
                    foreach(Collider collider in GetComponentsInChildren<Collider>())
                    {
                        collider.enabled = true;
                    }
                    GetComponent<Rigidbody>().isKinematic = false;
                    isReturningToStart = false;
                    RestartTime();
                    AudioManager.Instance.music.DOFade(0.5f, 0.3f);
                    AudioManager.Instance.musicReverb.DOFade(0f, 0.3f);

                    for (int i = 0; i < destroyedObstacles.Count; i++)
                    {
                        destroyedObstacles[i].SetActive(true);
                    }
                }));
	}

	private void CollectGem()
	{
		currentNbrOfGems++;
		if (currentNbrOfGems%lightStep==0)
		{
			//currentLightIntensity = 1 + currentNbrOfGems / lightStep * lightIntesityAdded;
			//foreach (Light light in lights)
			//{
			//	Tween tween = light.DOIntensity(currentLightIntensity, 0.3f).SetEase(Ease.OutQuint);
			//	switchLightTweens.Add(tween);
			//}
			switch (currentNbrOfGems/lightStep)
			{
				case 1:
					for (int i = 0; i < firstBatchOfLights.Length; i++)
					{
						firstBatchOfLights[i].gameObject.SetActive(true);
					}
					break;
				case 2:
					for (int i = 0; i < secondBatchOfLights.Length; i++)
					{
						secondBatchOfLights[i].gameObject.SetActive(true);
					}
					break;
				case 3:
					for (int i = 0; i < thirdBatchOfLights.Length; i++)
					{
						thirdBatchOfLights[i].gameObject.SetActive(true);
					}
					break;
				default:
					break;
			}

		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer==11)
		{
			Die();
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 8)
		{
			isJumping = false;
		}
		if (collision.gameObject.layer ==10)
		{
			CollectGem();
			Destroy(collision.gameObject);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		//if (collision.gameObject.layer == 9 && isBreakingWall)
		//{
        //    StuckChecker.CollisionsCount--;
        //    StopPauseDelay();
		//	//destroyedObstacles.Add(collision.gameObject);
		//	//collision.gameObject.SetActive(false);
		//}
	}

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
