using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private Vector3 target_Offset;
    private float cameraRotation;

    private Input controls;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cameraRotation = transform.eulerAngles.y;
        target_Offset = transform.position - player.position;
        controls = new Input();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate(controls.Player.CameraRotate.ReadValue<float>());

        if(player.GetComponent<Walking>().GetIsWalking())
        {
            player.eulerAngles = Vector3.Lerp(player.eulerAngles, new Vector3(0f, cameraRotation, 0f), 0.1f);
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraRotation, transform.eulerAngles.z);
        transform.position = Vector3.Lerp(transform.position, player.position + target_Offset, 0.05f);
        
        cameraRotation = Mathf.Clamp(cameraRotation, 1f, 91f);
    }

    private void Rotate(float context)
    {
        cameraRotation += context;
    }
}
