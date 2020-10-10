using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public float jumpHeight;
	public float breakingWallTime,slidingTime;
	bool isJumping = false, isBreakingWall=false,isSliding=false;
	float gameSpeed;
	Rigidbody2D myRigidbody;

	public MovingObjects environment;



	void Start()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
		environment = FindObjectOfType<MovingObjects>();
		gameSpeed = environment.speed;
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
		if (Input.GetKeyDown(KeyCode.R))
		{
			ChangeGameSpeed(0);
		}
		if (Input.GetKeyUp(KeyCode.R))
		{
			ChangeGameSpeed(gameSpeed);
		}
	}

	void Jump()
	{
		isJumping = true;
		myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpHeight);
	}
	void Punch()
	{
		
		StartCoroutine(BreakingWall());

	}
	IEnumerator BreakingWall()
	{
		isBreakingWall = true;
		GetComponent<SpriteRenderer>().color = Color.red;
		float time = 0;
		while (time<breakingWallTime)
		{
			time += Time.deltaTime;
			yield return null;
		}
		GetComponent<SpriteRenderer>().color = Color.white;
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
		GetComponent<SpriteRenderer>().color = Color.green;
		float time = 0;
		while (time < slidingTime)
		{
			time += Time.deltaTime;
			yield return null;
		}
		GetComponent<SpriteRenderer>().color = Color.white;
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
		isSliding = false;
	}
	void ChangeGameSpeed(float speed)
	{
		environment.speed = speed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer==8)
		{
			isJumping = false;
		}

	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 9 && isBreakingWall)
		{
			Destroy(collision.gameObject);
		}
	}
}
