using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private Transform cam;
    private Transform fade;
    private Transform obstruction;
    private Vector3 target_Offset;
    private float cameraRotation;

    public static bool inBar;
    
    [SerializeField] LayerMask barLayer;
    [SerializeField] LayerMask allLayer;
    [SerializeField] LayerMask hideLayer;

    private Input controls;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = transform.GetChild(0);
        cameraRotation = transform.eulerAngles.y;
        target_Offset = transform.position - player.position;
        controls = new Input();
        controls.Enable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(inBar)
        {
            GetComponentInChildren<Camera>().cullingMask = barLayer;
        }
        else
        {
            GetComponentInChildren<Camera>().cullingMask =  allLayer;
        }

        if(!JournalController.inJournal)
        {
            Rotate(controls.Player.CameraRotate.ReadValue<float>());

            if(player.GetComponent<Walking>().GetIsWalking())
            {
                player.rotation = Quaternion.Slerp(player.rotation, Quaternion.Euler(0f, cameraRotation, 0f), 0.1f);
            }

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraRotation, transform.eulerAngles.z);
            transform.position = Vector3.Slerp(transform.position, player.position + target_Offset, 0.1f);
        }

        FadeWall();
    }

    private void FadeWall()
    {
        if(inBar)
        {
            return;
        }

        Debug.DrawRay(player.position, (cam.position - player.position) * 4f);
        RaycastHit hit;
        Physics.Raycast(player.position, (cam.position - player.position) * 4f, out hit, 80f, hideLayer);

        if(hit.collider == null && fade != null)
        {
            foreach(MeshRenderer mesh in fade.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }

        if(hit.collider == null)
        {
            return;
        }

        print(hit.collider.name);

        if(fade != hit.collider.transform && fade != null)
        {
            foreach(MeshRenderer mesh in fade.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }

        fade = hit.collider.transform;

        MeshRenderer[] meshRenderers = fade.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer mesh in meshRenderers)
        {
            mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    private void Rotate(float context)
    {
        cameraRotation += context;

        if(cameraRotation >= 360f)
        {
            cameraRotation = 0f;
        }
        else if(cameraRotation <= 0f)
        {
            cameraRotation = 360f;
        }
    }
}
