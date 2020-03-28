using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class Interact : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI interactText;
    private DialogTrigger currentDialogTrigger;
    private DialogManager dialogManager;
    [HideInInspector] public List<Dialog> currentDialogs = new List<Dialog>();
    public Input controls;

    private GameObject interactableObject;

    private bool interactedWithDumpster;
    private bool inDialog;
    private bool inRange;

    void Awake()
    {
        controls = new Input();
        controls.Enable();
        controls.Player.Interact.performed += _ => Interacting();
    }

    void Start()
    {
        nameText = GameObject.Find("UI").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        interactText = GameObject.Find("UI").transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        dialogManager = FindObjectOfType<DialogManager>();

        interactedWithDumpster = false;
        inDialog = false;
    }

    void Interacting()
    {
        if(inDialog)
        {
            dialogManager.DisplayNextSentence();
        }
        else
        {
            if (!inRange)
            {
                return;
            }

            if (currentDialogs.Count != 0)
            {
                currentDialogTrigger.SetDialogs(currentDialogs.ToArray());
            }

            interactText.gameObject.SetActive(false);
            nameText.gameObject.SetActive(false);

            currentDialogTrigger.TriggerDialogs();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("InteractObject"))
        {
            currentDialogs.Clear();
            interactableObject = other.gameObject;
            currentDialogTrigger = interactableObject.GetComponent<DialogTrigger>();
            inRange = true;

            nameText.text = "Interact with " + interactableObject.name;

            interactText.gameObject.SetActive(true);
            nameText.gameObject.SetActive(true);

            SetCurrentDialogs(other.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag.Equals("InteractObject") || other.gameObject.tag.Equals("InteractNPC"))
        {
            interactableObject = null;
            inRange = false;

            interactText.gameObject.SetActive(false);
            nameText.gameObject.SetActive(false);
        }
    }

    private void SetCurrentDialogs(string name)
    {
        if (name == "Dumpster")
        {
            interactedWithDumpster = true;
        }
        else if (name == "Dimitri")
        {
            currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);

            if (interactedWithDumpster)
            {
                foreach (Dialog dialog in currentDialogTrigger.GetDialogs())
                {
                    if (dialog.dependance == "interactedWithDumpster" && interactedWithDumpster)
                    {
                        currentDialogs.Add(dialog);
                    }
                }
            }
        }
    }

    public void SetInDialog(bool state)
    {
        inDialog = state;
    }
}
