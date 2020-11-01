using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inputs
{
    public static Inputs Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new Inputs(false);
            }

            return instance;
        }
    }
    private static Inputs instance;

    public KeyCode JumpKey { get => jumpKey; set => jumpKey = value; }
    public KeyCode ShrinkKey { get => shrinkKey; set => shrinkKey = value; }
    public KeyCode BreakingWaveKey { get => breakingWaveKey; set => breakingWaveKey = value; }
    public KeyCode FreezeKey { get => freezeKey; set => freezeKey = value; }
    public bool IsJumpKeySelected { get => isJumpKeySelected; set => isJumpKeySelected = value; }
    public bool IsShrinkKeySelected { get => isShrinkKeySelected; set => isShrinkKeySelected = value; }
    public bool IsBreakingWaveKeySelected { get => isBreakingWaveKeySelected; set => isBreakingWaveKeySelected = value; }
    public bool IsFreezeKeySelected { get => isFreezeKeySelected; set => isFreezeKeySelected = value; }

    private KeyCode jumpKey;
    private KeyCode shrinkKey;
    private KeyCode breakingWaveKey;
    private KeyCode freezeKey;
    private bool isJumpKeySelected;
    private bool isShrinkKeySelected;
    private bool isBreakingWaveKeySelected;
    private bool isFreezeKeySelected;



    public static Inputs Init(KeyCode jumpKey, KeyCode shrinkKey, KeyCode breakingWaveKey, KeyCode freezeKey)
    {
        if(instance == null)
        {
            instance = new Inputs(true);
            instance.jumpKey = jumpKey;
            instance.shrinkKey = shrinkKey;
            instance.breakingWaveKey = breakingWaveKey;
            instance.freezeKey = freezeKey;
        }
        return instance;
    }

    public Inputs(bool isInitialized)
    {
        isJumpKeySelected = isInitialized;
        isShrinkKeySelected = isInitialized;
        isBreakingWaveKeySelected = isInitialized;
        isFreezeKeySelected = isInitialized;
    }
}
