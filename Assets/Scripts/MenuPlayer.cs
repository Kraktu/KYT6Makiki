using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuPlayer : MonoBehaviour
{
    [SerializeField] private Player.Key button = Player.Key.Joystick1_A;
    [SerializeField] private UnityEvent onButtonPressed = null;
    [SerializeField] private UnityEvent onButtonReleased = null;

    private void Update()
    {
        if(Input.GetKeyDown(Player.GetKeyCode(button)))
        {
            onButtonPressed.Invoke();
        }

        if(Input.GetKeyUp(Player.GetKeyCode(button)))
        {
            onButtonReleased.Invoke();
        }
    }
}
