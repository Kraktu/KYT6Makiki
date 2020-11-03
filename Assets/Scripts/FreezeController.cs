using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using RasPacJam.Audio;

public class FreezeController : MonoBehaviour
{
    public bool CanFreeze { get => canFreeze; set => canFreeze = value; }
    [SerializeField] private float freezeTemperature = 0f;
    [SerializeField] private StuckChecker stuckChecker = null;
    [SerializeField] private UnityEvent onFreezed = null;
    [SerializeField] private UnityEvent onUnfreezed = null;
    [SerializeField] private UnityEvent onUnfreezedWhenStuck = null;
    private ColorGrading colorGrading;
    private float initialTemperature;
    private bool isFreezing;
    private bool canFreeze;



    public void Freeze(InputAction.CallbackContext action)
    {
       if(!isFreezing && !canFreeze)
       {
           return;
       }

       if(action.started)
       {
           isFreezing = true;
           AudioManager.Instance.Play("freezing");
           onFreezed.Invoke();
       }
       else if(action.canceled)
       {
           isFreezing = false;
           // AudioManager.Instance.Play("unfreezing");
           if(stuckChecker.IsStuck)
           {
               onUnfreezedWhenStuck.Invoke();
           }
           else
           {
                onUnfreezed.Invoke();
           }
       }
    }

    // old input system
    public void Freeze()
    {
       if(!isFreezing && !canFreeze)
       {
           return;
       }

        isFreezing = true;
        AudioManager.Instance.Play("freezing");
        if(colorGrading)
        {
            colorGrading.temperature.value = freezeTemperature;
        }
        onFreezed.Invoke();
    }

    public void Unfreeze()
    {
        if(!isFreezing && !canFreeze)
        {
            return;
        }

        if(colorGrading)
        {
            colorGrading.temperature.value = initialTemperature;
        }

        isFreezing = false;
        // AudioManager.Instance.Play("unfreezing");
        if(stuckChecker.IsStuck)
        {
            onUnfreezedWhenStuck.Invoke();
        }
        else
        {
            onUnfreezed.Invoke();
        }
    }

    public void Reset()
    {
        isFreezing = false;
        if(colorGrading)
        {
            colorGrading.temperature.value = initialTemperature;
        }
    }



    private void Awake()
    {
        isFreezing = false;
        canFreeze = true;
    }

    private void Start()
    {
        PostProcessVolume postProcessVolume = FindObjectOfType<PostProcessVolume>();
        if(postProcessVolume)
        {
            postProcessVolume.profile.TryGetSettings<ColorGrading>(out colorGrading);
        }
        if(colorGrading)
        {
            initialTemperature = colorGrading.temperature.value;
        }
    }
}
