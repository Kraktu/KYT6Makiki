using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RasPacJam.Audio;

public class JumpController : MonoBehaviour
{
    public bool CanJump { get => canJump; set => canJump = value; }
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private GroundChecker groundChecker = null;
    private Rigidbody rb;
    private bool canJump;



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

        rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        AudioManager.Instance.Play("jump");
    }



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        canJump = true;
    }
}
