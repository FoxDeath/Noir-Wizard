using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarCollider : MonoBehaviour
{


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
