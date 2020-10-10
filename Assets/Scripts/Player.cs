using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
	public float jumpHeight = 5f;
	public float breakingWallTime, slidingTime;
	bool isJumping = false, isBreakingWall=false, isSliding=false;
	public Material redMat, greenMat, basicMat;
	Rigidbody myRigidbody;
    public CameraMovable mainCamera = null;



	void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();
        mainCamera.Speed = speed;
	}

    void OnValidate()
    {
        mainCamera.Speed = speed;
    }

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A) && !isJumping)
		{
			Jump();
		}
		if (Input.GetKeyDown(KeyCode.Z)&&!isBreakingWall)
		{
			Punch();
		}
		if (Input.GetKeyDown(KeyCode.E)&&!isSliding)
		{
			Slide();
		}
        if(Input.GetKey(KeyCode.R))
        {
            myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, myRigidbody.velocity.z);
        }
        else
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
			Destroy(collision.gameObject);
		}
	}
}
