using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RasPacJam.Audio;

public class LettersSwitch : MonoBehaviour
{
    public bool CanSwitch { get => canSwitch; set => canSwitch = value; }

    [SerializeField] private MeshRenderer pLetter = null;
    [SerializeField] private MeshRenderer lLetter = null;
    [SerializeField] private MeshRenderer aLetter = null;
    [SerializeField] private MeshRenderer yLetter = null;
    [SerializeField] private Material emissiveMaterial = null;
    [SerializeField] private SceneLoader sceneLoader = null;
    private Material defaultMaterial;
    private bool isPLetterOn;
    private bool isLLetterOn;
    private bool isALetterOn;
    private bool isYLetterOn;
    private bool canSwitch;



    public void SwitchOnPLetter(InputAction.CallbackContext action)
    {
        if(action.started && canSwitch)
        {
            pLetter.material = emissiveMaterial;
            isPLetterOn = true;
        }
        else if(action.canceled)
        {
            pLetter.material = defaultMaterial;
            isPLetterOn = false;
        }
    }

    public void SwitchOnLLetter(InputAction.CallbackContext action)
    {
        if(action.started && canSwitch)
        {
            lLetter.material = emissiveMaterial;
            isLLetterOn = true;
        }
        else if(action.canceled)
        {
            lLetter.material = defaultMaterial;
            isLLetterOn = false;
        }
    }

    public void SwitchOnALetter(InputAction.CallbackContext action)
    {
        if(action.started && canSwitch)
        {
            aLetter.material = emissiveMaterial;
            isALetterOn = true;
        }
        else if(action.canceled)
        {
            aLetter.material = defaultMaterial;
            isALetterOn = false;
        }
    }

    public void SwitchOnYLetter(InputAction.CallbackContext action)
    {
        if(action.started)
        {
            yLetter.material = emissiveMaterial;
            isYLetterOn = true;
        }
        else if(action.canceled)
        {
            yLetter.material = defaultMaterial;
            isYLetterOn = false;
        }
    }



    private void Update()
    {
        if(isPLetterOn && isLLetterOn && isALetterOn && isYLetterOn)
        {
            AudioManager.Instance.Play("start");
            isPLetterOn = false;
            isLLetterOn = false;
            isALetterOn = false;
            isYLetterOn = false;
            sceneLoader.LoadLevel(1);
        }
    }

    private void Awake()
    {
        isPLetterOn = false;
        isLLetterOn = false;
        isALetterOn = false;
        isYLetterOn = false;
        canSwitch = true;
    }

    private void Start()
    {
        defaultMaterial = pLetter.material;
    }
}
