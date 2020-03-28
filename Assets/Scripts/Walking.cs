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
    private bool canWalk;

    [SerializeField] float speed = 5f;

    void Awake()
    {
        controls = new Input();
        controls.Enable();
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        canWalk = true;
    }

    private void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        if(moveInput != Vector2.zero && canWalk)
        {
            transform.GetChild(0).transform.rotation = Quaternion.Slerp(transform.GetChild(0).transform.rotation, Quaternion.LookRotation(move), 0.1f);
            move = transform.right * moveInput.x + transform.forward * moveInput.y;
            controller.Move(speed * move * Time.deltaTime);
            isWalking = true;
        }
    }

    public void SetCanWalk(bool state)
    {
        canWalk = state;
    }
    
    public bool GetIsWalking()
    {
        return isWalking;
    }
}
