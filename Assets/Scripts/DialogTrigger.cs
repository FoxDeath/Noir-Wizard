using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog[] dialogs;
    private Dialog[] activeDialogs;
    private DialogManager dialogManager;

    void Awake()
    {
        dialogManager = FindObjectOfType<DialogManager>();

        //by default every dialog is active
        activeDialogs = new Dialog[dialogs.Length];

        for (int i = 0; i < dialogs.Length; i++)
        {
            activeDialogs[i] = dialogs[i];
        }
    }

    public Dialog[] GetDialogs()
    {
        return dialogs;
    }

    //called from the player controller upon interaction
    //player controller iterates through all dialogs and selects the ones that are active based on the dependance string
    public void SetDialogs(Dialog[] dialogs)
    {
        activeDialogs = new Dialog[dialogs.Length];

        for(int i = 0; i < dialogs.Length; i++)
        {
            activeDialogs[i] = dialogs[i];
        }
    }

    //called from the player controller either on its own if there is only one dialog
    //or after SetDialogs() if there are more
    public void TriggerDialogs()
    {
        dialogManager.InitializeDialogs(activeDialogs);
    }
}
