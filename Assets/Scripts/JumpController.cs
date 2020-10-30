using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RasPacJam.Audio;

public class JumpController : MonoBehaviour
{
    public bool CanJump { get => canJump; set => canJump = value; }

    [SerializeField] private ShrinkController shrinkController = null;
    [SerializeField] private GroundChecker groundChecker = null;
    [SerializeField] private float maxJumpHeight = 10f;
    [SerializeField] private float maxJumpTime = 2f;
    [SerializeField] private bool hasMomentum = false;
    private Rigidbody rb;
    private float jumpTimeCounter;
    private float jumpIntensity;
    private bool canJump;
    private bool isJumping;



    public void Jump()
    {
        if(!groundChecker.IsGrounded)
        {
            return;
        }
        if(!canJump)
        {
            return;
        }

        isJumping = true;
        AudioManager.Instance.Play("jump");
    }

    public void Land()
    {
        if(!isJumping)
        {
            return;
        }

        isJumping = false;
        if(!hasMomentum)
        {
            rb.velocity = Vector3.zero;
        }
        jumpTimeCounter = maxJumpTime;
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpTimeCounter = maxJumpTime;
        jumpIntensity = maxJumpHeight / maxJumpTime;
        canJump = true;
        isJumping = false;
    }

    private void Update()
    {
        if(!isJumping)
        {
            return;
        }

        rb.velocity = Vector3.up * jumpIntensity * (shrinkController.IsShrinked ? 0.5f : 1f);

        jumpTimeCounter -= Time.deltaTime;
        if(jumpTimeCounter <= 0)
        {
            Land();
        }
    }
}
