using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Walking : MonoBehaviour
{
    CharacterController controller;

    [SerializeField] float speed = 5f;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }
    private void Update()
    {
        Move();
    }

    private void Move() 
    {
        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        if (kb.wKey.isPressed)
        {
            controller.Move(speed * transform.forward * Time.deltaTime);
        }
        if (kb.sKey.isPressed)
        {
            controller.Move(speed * transform.forward * Time.deltaTime * -1);
        }
        if (kb.aKey.isPressed)
        {
            controller.Move(speed * transform.right * Time.deltaTime * -1);
        }
        if (kb.dKey.isPressed)
        {
            controller.Move(speed * transform.right * Time.deltaTime);
        }
    }
}
