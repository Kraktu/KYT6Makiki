using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayersManager : MonoBehaviour
{
    [SerializeField] private GameObject playerInputPrefab = null;
    [SerializeField] private GameObject player = null;
    private Default controls;

    private void Start()
    {
        PlayerInput player1 = PlayerInput.Instantiate(playerInputPrefab, controlScheme : "Player1");
        player1.transform.SetParent(this.transform);
        player1.neverAutoSwitchControlSchemes = true;
        player1.actions.FindAction("Jump").started += _ => player.GetComponent<JumpController>().Jump();
        // controls.Player.Jump.started += _ => player.GetComponent<JumpController>().Jump();
    }

    private void OnEnable()
    {
        controls = new Default();
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
