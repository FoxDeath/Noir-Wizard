using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private Camera cam;
    private Transform obstruction;
    private Vector3 target_Offset;
    private float cameraRotation;
        
    List<Transform> fade = new ();

    public static bool inBar;
    
    [SerializeField] LayerMask barLayer;
    [SerializeField] LayerMask allLayer;
    [SerializeField] LayerMask hideLayer;

    private Input controls;

    private Vector3 defaultPossition;
    private Quaternion defaultRotation;
    private bool valuesSet = false;

    [SerializeField] private float cameraRotateSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = transform.GetChild(0).GetComponent<Camera>();
        cameraRotation = 4f;
        controls = new Input();
        controls.Enable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!MainMenu.GameStarted)
        {
            return;
        }

        if(transform.position != defaultPossition && !valuesSet)
        {
            SetValues();
            return;
        }

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
            Rotate(controls.Player.CameraRotate.ReadValue<float>() * cameraRotateSpeed);

            if(player.GetComponent<Walking>().GetIsWalking())
            {
                player.rotation = Quaternion.Slerp(player.rotation, Quaternion.Euler(0f, cameraRotation, 0f), 0.1f);
            }

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraRotation, transform.eulerAngles.z);
            transform.position = Vector3.Lerp(transform.position, player.position + target_Offset, 0.1f);
        }

        FadeWall();
    }

    private void FadeWall()
    {
        if(inBar)
        {
            return;
        }

        Vector3 n1 = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane));
        Vector3 n2 = cam.ViewportToWorldPoint(new Vector3(1,0,cam.nearClipPlane));
        Vector3 n3 = cam.ViewportToWorldPoint(new Vector3(0,1,cam.nearClipPlane));
        Vector3 n4 = cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane));

        Debug.DrawRay(player.position, (n1 - player.position));
        Debug.DrawRay(player.position, (n2 - player.position));
        Debug.DrawRay(player.position, (n3 - player.position));
        Debug.DrawRay(player.position, (n4 - player.position));
        
        RaycastHit[] hit1 = Physics.RaycastAll(player.position, (n1 - player.position) * 4f, 500f, hideLayer);
        RaycastHit[] hit2 = Physics.RaycastAll(player.position, (n2 - player.position) * 4f, 500f, hideLayer);
        RaycastHit[] hit3 = Physics.RaycastAll(player.position, (n3 - player.position) * 4f, 500f, hideLayer);
        RaycastHit[] hit4 = Physics.RaycastAll(player.position, (n4 - player.position) * 4f, 500f, hideLayer);

        List<RaycastHit> hits = new();

        if(hit1.Length != 0)
        {
            hits.AddRange(hit1);
        }
        
        if(hit2.Length != 0)
        {
            hits.AddRange(hit2);
        }
        
        if(hit3.Length != 0)
        {
            hits.AddRange(hit3);
        }
        
        if(hit4.Length != 0)
        {
            hits.AddRange(hit4);
        }

        if(fade.Count != 0)
        {
            List<Transform> newFade = fade;

            foreach(var f in fade)
            {
                bool b = false;
                
                foreach(var hit in hits)
                {
                    if(hit.collider && hit.collider.transform == f)
                    {
                        b = true;
                    }
                }

                if(b == false)
                {
                    foreach(MeshRenderer mesh in f.GetComponentsInChildren<MeshRenderer>())
                    {
                        mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    }

                    newFade.Remove(f);
                }
            }

            fade = newFade;
        }
        
        foreach(var hit in hits)
        {
            if(hit.collider && !fade.Contains(hit.collider.transform))
            {
                fade.Add(hit.collider.transform);
            }
        }
        
        if(fade.Count != 0)
        {
            foreach(MeshRenderer mesh in fade.SelectMany(f => f.GetComponentsInChildren<MeshRenderer>()))
            {
                mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
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

    public void GetValues()
    {
        defaultPossition = transform.position;
        defaultRotation = transform.rotation;
    }

    public void SetValues()
    {
        transform.position = defaultPossition;
        transform.rotation = Quaternion.Euler(30f, 4f, 0f);
        target_Offset = transform.position - player.position;
        valuesSet = true;
    }
}
