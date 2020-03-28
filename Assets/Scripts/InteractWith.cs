using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWith : MonoBehaviour
{
    bool firstInteraction;

    //epending on the bools of what we did we can write different shit
    public void Interaction() 
    {
        if (!firstInteraction)
        {
            print("Yo!");
            firstInteraction = true;
        }
        else
        {
            print("Fuck off!");
            firstInteraction = false;
        }
    }
}
