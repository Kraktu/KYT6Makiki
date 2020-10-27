using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private bool isInitializedByPlayer = true;
    [SerializeField] private KeyCode jumpKey = KeyCode.Joystick1Button0;
    [SerializeField] private KeyCode shrinkKey = KeyCode.Joystick1Button1;
    [SerializeField] private KeyCode breakingWaveKey = KeyCode.Joystick1Button2;
    [SerializeField] private KeyCode freezeKey = KeyCode.Joystick1Button3;
    [SerializeField] private UnityEvent onJumpKeyPressed = null;
    [SerializeField] private UnityEvent onJumpKeyReleased = null;
    [SerializeField] private UnityEvent onShrinkKeyPressed = null;
    [SerializeField] private UnityEvent onShrinkKeyReleased = null;
    [SerializeField] private UnityEvent onBreakingWaveKeyPressed = null;
    [SerializeField] private UnityEvent onBreakingWaveKeyReleased = null;
    [SerializeField] private UnityEvent onFreezeKeyPressed = null;
    [SerializeField] private UnityEvent onFreezeKeyReleased = null;
    private Inputs inputs;



    private void Awake()
    {
        if(isInitializedByPlayer)
        {
            inputs = Inputs.Init();
        }
        else
        {
            inputs = Inputs.Init(jumpKey, shrinkKey, breakingWaveKey, freezeKey);
        }
    }

    private void Update()
    {
        if(inputs.IsJumpKeySelected && Input.GetKeyDown(inputs.JumpKey))
        {
            onJumpKeyPressed.Invoke();
        }

        if(inputs.IsJumpKeySelected && Input.GetKeyUp(inputs.JumpKey))
        {
            onJumpKeyReleased.Invoke();
        }

        if(inputs.IsShrinkKeySelected && Input.GetKeyDown(inputs.ShrinkKey))
        {
            onShrinkKeyPressed.Invoke();
        }

        if(inputs.IsShrinkKeySelected && Input.GetKeyUp(inputs.ShrinkKey))
        {
            onShrinkKeyReleased.Invoke();
        }

        if(inputs.IsBreakingWaveKeySelected && Input.GetKeyDown(inputs.BreakingWaveKey))
        {
            onBreakingWaveKeyPressed.Invoke();
        }

        if(inputs.IsBreakingWaveKeySelected && Input.GetKeyUp(inputs.BreakingWaveKey))
        {
            onBreakingWaveKeyReleased.Invoke();
        }

        if(inputs.IsFreezeKeySelected && Input.GetKeyDown(inputs.FreezeKey))
        {
            onFreezeKeyPressed.Invoke();
        }

        if(inputs.IsFreezeKeySelected && Input.GetKeyUp(inputs.FreezeKey))
        {
            onFreezeKeyReleased.Invoke();
        }
    }
}
