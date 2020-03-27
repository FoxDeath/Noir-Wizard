using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Walking : MonoBehaviour
{
    CharacterController controller;
    Input controls;
    private Vector2 moveInput;
    private Vector3 move;

    private bool isWalking;

    public bool GetIsWalking()
    {
        return isWalking;
    }

    [SerializeField] float speed = 5f;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        controls = new Input();
        controls.Enable();
    }
    private void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(speed * move * Time.deltaTime);

        if(moveInput != Vector2.zero)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
}
