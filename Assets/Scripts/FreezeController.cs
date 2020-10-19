using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using RasPacJam.Audio;

public class FreezeController : MonoBehaviour
{
    public bool IsFreezing { get => isFreezing; set => isFreezing = value; }
    public bool CanFreeze { get => canFreeze; set => canFreeze = value; }
    [SerializeField] private UnityEvent onFreezed = null;
    [SerializeField] private UnityEvent onUnfreezed = null;
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
            onUnfreezed.Invoke();
        }
    }



    private void Awake()
    {
        isFreezing = false;
        canFreeze = true;
    }
}
