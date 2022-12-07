﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Walking : MonoBehaviour
{
    CharacterController controller;
    Input controls;
    Animator anim;
    private Vector2 moveInput;
    private Vector3 move;

    private bool isWalking;
    private bool canWalk;

    [SerializeField] float speed = 5f;

    AudioManager audioManager;

    void Awake()
    {
        controls = new Input();
        controls.Enable();
    }

    public void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        canWalk = true;
    }

    private void FixedUpdate()
    {
        if(!MainMenu.GameStarted || JournalController.inJournal)
        {
            return;
        }

        moveInput = controls.Player.Move.ReadValue<Vector2>();

        if(moveInput != Vector2.zero && canWalk)
        {
            transform.GetChild(0).transform.rotation = Quaternion.Slerp(transform.GetChild(0).transform.rotation, Quaternion.LookRotation(move), 0.1f);
            move = transform.right * moveInput.x + transform.forward * moveInput.y;
            controller.Move(speed * move * Time.deltaTime);
            anim.SetBool("isWalking", true);
            isWalking = true;
            if(!audioManager.IsPlaying("walk2"))
            {
                audioManager.Play("walk2");
            }
        }
        else if(moveInput == Vector2.zero)
        {
            anim.SetBool("isWalking", false);
            audioManager.Stop("walk2");
            isWalking = false;
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
