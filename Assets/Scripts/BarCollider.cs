using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarCollider : MonoBehaviour
{

    void Update()
    {
        if(CameraController.inBar)
        {
            foreach(MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
            {
                if(mesh.shadowCastingMode != UnityEngine.Rendering.ShadowCastingMode.On)
                {
                    mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }
        }
    }


    // Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag.Equals("Player"))
        {
            CameraController.inBar = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.tag.Equals("Player"))
        {
            CameraController.inBar = false;
        }
    }

}
