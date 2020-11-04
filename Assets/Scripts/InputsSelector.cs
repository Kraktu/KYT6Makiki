using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class InputsSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputSelectorLabel = null;
    [SerializeField] private List<UnityEvent> onKeysSelected = null;
    [SerializeField] private Color primaryColor = Color.white;
    [SerializeField] private Color jumpColor = Color.white;
    [SerializeField] private Color shrinkColor = Color.white;
    [SerializeField] private Color breakingWaveColor = Color.white;
    [SerializeField] private Color freezeColor = Color.white;
    [SerializeField] private float pulsePeriod = 0.7f;
    [SerializeField] private float pulseIntensity = 1.05f;
    private Inputs inputs;
    private List<KeyCode> assignedKeys;
    private int keyIndex;
    private bool isSelectionDone;

    private void Awake()
    {
        assignedKeys = new List<KeyCode>();
        keyIndex = 0;
        inputSelectorLabel.text = "press jump button";
        inputSelectorLabel.colorGradient = new VertexGradient(primaryColor, primaryColor, jumpColor, jumpColor);
        Vector3 finalScale = inputSelectorLabel.transform.localScale * pulseIntensity;
        inputSelectorLabel.transform.DOScale(finalScale, pulsePeriod).SetLoops(-1, LoopType.Yoyo);
        isSelectionDone = false;
        inputs = Inputs.Instance;
        inputs.IsJumpKeySelected = false;
        inputs.IsShrinkKeySelected = false;
        inputs.IsBreakingWaveKeySelected = false;
        inputs.IsFreezeKeySelected = false;
    }

    private void Update()
    {
        if(isSelectionDone)
        {
            return;
        }

        if(Input.anyKeyDown)
        {
            foreach(KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if(
                        keyCode == KeyCode.JoystickButton0 ||
                        keyCode == KeyCode.JoystickButton1 ||
                        keyCode == KeyCode.JoystickButton2 ||
                        keyCode == KeyCode.JoystickButton3 ||
                        keyCode == KeyCode.JoystickButton4 ||
                        keyCode == KeyCode.JoystickButton5 ||
                        keyCode == KeyCode.JoystickButton6 ||
                        keyCode == KeyCode.JoystickButton7 ||
                        keyCode == KeyCode.JoystickButton8 ||
                        keyCode == KeyCode.JoystickButton9 ||
                        keyCode == KeyCode.JoystickButton10 ||
                        keyCode == KeyCode.JoystickButton11 ||
                        keyCode == KeyCode.JoystickButton12 ||
                        keyCode == KeyCode.JoystickButton13 ||
                        keyCode == KeyCode.JoystickButton14 ||
                        keyCode == KeyCode.JoystickButton15 ||
                        keyCode == KeyCode.JoystickButton16 ||
                        keyCode == KeyCode.JoystickButton17 ||
                        keyCode == KeyCode.JoystickButton18 ||
                        keyCode == KeyCode.JoystickButton19
                )
                {
                    continue;
                }

                if(Input.GetKeyDown(keyCode))
                {
                    foreach(KeyCode assignedKey in assignedKeys)
                    {
                        if(keyCode == assignedKey)
                        {
                            return;
                        }
                    }

                    switch(keyIndex)
                    {
                        case 0 :
                            if(inputs.IsJumpKeySelected == true)
                            {
                                break;
                            }
                            inputs.JumpKey = keyCode;
                            inputs.IsJumpKeySelected = true;
                            inputSelectorLabel.text = "press shrink button";
                            inputSelectorLabel.colorGradient = new VertexGradient(primaryColor, primaryColor, shrinkColor, shrinkColor);
                            break;
                        case 1 :
                            if(inputs.IsShrinkKeySelected == true)
                            {
                                break;
                            }
                            inputs.ShrinkKey = keyCode;
                            inputs.IsShrinkKeySelected = true;
                            inputSelectorLabel.text = "press breaking wave button";
                            inputSelectorLabel.colorGradient = new VertexGradient(primaryColor, primaryColor, breakingWaveColor, breakingWaveColor);
                            break;
                        case 2 :
                            if(inputs.IsBreakingWaveKeySelected == true)
                            {
                                break;
                            }
                            inputs.BreakingWaveKey = keyCode;
                            inputs.IsBreakingWaveKeySelected = true;
                            inputSelectorLabel.text = "press freeze";
                            inputSelectorLabel.colorGradient = new VertexGradient(primaryColor, primaryColor, freezeColor, freezeColor);
                            break;
                        case 3 :
                            if(inputs.IsFreezeKeySelected == true)
                            {
                                break;
                            }
                            inputs.FreezeKey = keyCode;
                            inputs.IsFreezeKeySelected = true;
                            inputSelectorLabel.gameObject.SetActive(false);
                            isSelectionDone = true;
                            break;
                    }
                    assignedKeys.Add(keyCode);
                    onKeysSelected[keyIndex].Invoke();
                    keyIndex++;
                }
            }
        }
    }
}
