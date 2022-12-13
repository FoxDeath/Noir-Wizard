using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI interactText;
    private DialogTrigger currentDialogTrigger;
    private DialogManager dialogManager;
    [HideInInspector] public List<Dialog> currentDialogs = new List<Dialog>();

    private GameObject interactableObject;

    private string currentInteract;

    private bool inDialog;
    private bool inRange;
    private static bool talkedToDumpster;
    private static bool talkedToPileOfAsh;
    private static bool talkedToPuke;
    private static bool talkedToToiletDoor;
    private static bool talkedToBarTable;
    private static bool talkedToLois;
    private static bool talkedToBarry;
    private static bool talkedToArnold;
    public static bool talkedToSilvester;
    private static bool talkedToDimitri;
    private static bool talkedToPhoenix;
    private static bool talkedToPersonOutside;
    public static bool talkedToBarBarOwnerOrDimitriAboutCat;
    private static bool madeRightChoice;

    public static int peopleTalkedTo = 0;

    void Start()
    {
        nameText = GameObject.Find("UI").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        interactText = GameObject.Find("UI").transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        dialogManager = FindObjectOfType<DialogManager>();
        peopleTalkedTo = 0;
        inDialog = false;
        talkedToDumpster = false;
        talkedToPileOfAsh = false;
        talkedToPuke = false;
        talkedToToiletDoor = false;
        talkedToBarTable = false;
        talkedToLois = false;
        talkedToBarry = false;
        talkedToArnold = false;
        talkedToSilvester = false;
        talkedToDimitri = false;
        talkedToPhoenix = false;
        talkedToPersonOutside = false;
        talkedToBarBarOwnerOrDimitriAboutCat = false;
    }

    public void Interacting(InputAction.CallbackContext context)
    {
        if(context.phase != InputActionPhase.Started || JournalController.inJournal || !MainMenu.GameStarted)
        {
            return;
        }
        
        if (inDialog)
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
        if(!MainMenu.GameStarted)
        {
            return;
        }
        
        if (other.gameObject.tag.Equals("InteractObject"))
        {
            interactableObject = other.gameObject;
            currentInteract = other.name;
            currentDialogTrigger = interactableObject.GetComponent<DialogTrigger>();
            inRange = true;

            nameText.text = "Interact with " + interactableObject.name;

            interactText.gameObject.SetActive(true);
            nameText.gameObject.SetActive(true);

            SetCurrentDialogs();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("InteractObject"))
        {
            interactableObject = null;
            inRange = false;

            interactText.gameObject.SetActive(false);
            nameText.gameObject.SetActive(false);
        }
    }

    public bool GetInDialog()
    {
        return inDialog;
    }

    public void SetMadeRightChoice(bool state)
    {
        madeRightChoice = state;
    }

    public bool GetMadeRightChoice()
    {
        return madeRightChoice;
    }

    public Dialog[] SetCurrentDialogs()
    {
        currentDialogs.Clear();

        switch (currentInteract)
        {
            case "Dumpster":
                if(!talkedToDumpster)
                {
                    talkedToDumpster = true;

                    peopleTalkedTo++;
                }

                if (talkedToDimitri)
                {
                    foreach (Dialog dialog in currentDialogTrigger.GetDialogs())
                    {
                        if (dialog.dependance == "talkedToDimitri")
                        {
                            currentDialogs.Add(dialog);
                        }
                    }
                }
                else
                {
                    currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                }

                break;

            case "Pile Of Ash":
                if(!talkedToPileOfAsh)
                {
                    talkedToPileOfAsh = true;

                    peopleTalkedTo++;
                }

                if(talkedToBarry && talkedToArnold && talkedToBarTable && talkedToDimitri && talkedToDumpster && talkedToLois
                    && talkedToPersonOutside && talkedToPileOfAsh && talkedToPuke && talkedToSilvester && talkedToBarBarOwnerOrDimitriAboutCat
                    && talkedToToiletDoor)
                {
                    foreach (Dialog dialog in currentDialogTrigger.GetDialogs())
                    {
                        if (dialog.dependance == "talkedToEveryone")
                        {
                            currentDialogs.Add(dialog);
                        }
                    }
                }
                else if (talkedToArnold)
                {
                    foreach (Dialog dialog in currentDialogTrigger.GetDialogs())
                    {
                        if (dialog.dependance == "talkedToArnold")
                        {
                            currentDialogs.Add(dialog);
                        }
                    }
                }
                else
                {
                    currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                }

                break;

            case "Puke":
                if(!talkedToPuke)
                {
                    talkedToPuke = true;
                    
                    peopleTalkedTo++;
                }

                if (talkedToBarry)
                {
                    foreach (Dialog dialog in currentDialogTrigger.GetDialogs())
                    {
                        if (dialog.dependance == "talkedToBarry")
                        {
                            currentDialogs.Add(dialog);
                        }
                    }
                }
                else
                {
                    currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                }

                break;

            case "Toilet Door":
                if(!talkedToToiletDoor)
                {
                    talkedToToiletDoor = true;
                    
                    peopleTalkedTo++;
                }
                
                currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                break;

            case "Bar Table":
                if(!talkedToBarTable)
                {
                    talkedToBarTable = true;
                    
                    peopleTalkedTo++;
                }
                currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                break;

            case "Lois":
                if (!talkedToLois)
                {
                    currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                    talkedToLois = true;

                    peopleTalkedTo++;
                }
                else
                {
                    if (talkedToSilvester)
                    {
                        if(!talkedToBarBarOwnerOrDimitriAboutCat)
                        {
                            talkedToBarBarOwnerOrDimitriAboutCat = true;
                        } 
                        currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);

                        foreach (Dialog dialog in currentDialogTrigger.GetDialogs())
                        {
                            if (dialog.dependance == "talkedToSilvester")
                            {
                                currentDialogs.Add(dialog);
                            }
                        }
                    }
                    else
                    {
                        currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                    }
                }

                break;

            case "Barry":
                if(!talkedToBarry)
                {
                    talkedToBarry = true;

                    peopleTalkedTo++;
                }
                currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                break;

            case "Arnold":
                if(!talkedToArnold)
                {
                    talkedToArnold = true;

                    peopleTalkedTo++;
                }
                currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                break;

            case "Silvester":
                if (!talkedToSilvester)
                {
                    currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                    talkedToSilvester = true;

                    peopleTalkedTo++;
                }
                else
                {
                    currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);

                    if (talkedToDimitri)
                    {
                        foreach (Dialog dialog in currentDialogTrigger.GetDialogs())
                        {
                            if (dialog.dependance == "talkedToDimitri")
                            {
                                currentDialogs.Add(dialog);
                            }
                        }
                    }
                    if (talkedToBarBarOwnerOrDimitriAboutCat)
                    {
                        foreach (Dialog dialog in currentDialogTrigger.GetDialogs())
                        {
                            if (dialog.dependance == "talkedToBarBarOwnerOrDimitriAboutCat")
                            {
                                currentDialogs.Add(dialog);
                            }
                        }
                    }
                }

                break;

            case "Dimitri":
                if (!talkedToDimitri)
                {
                    currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                    talkedToDimitri = true;

                    peopleTalkedTo++;
                }
                else
                {
                    if(talkedToSilvester)
                    {
                        if(!talkedToBarBarOwnerOrDimitriAboutCat)
                        {
                            talkedToBarBarOwnerOrDimitriAboutCat = true;
                        }
                        currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);

                        foreach (Dialog dialog in currentDialogTrigger.GetDialogs())
                        {
                            if (dialog.dependance == "talkedToSilvester")
                            {
                                currentDialogs.Add(dialog);
                            }
                        }
                    }
                    else
                    {
                        currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                    }
                }

                break;

            case "Phoenix":
                talkedToPhoenix = true;

                if (madeRightChoice)
                {
                    foreach (Dialog dialog in currentDialogTrigger.GetDialogs())
                    {
                        if (dialog.dependance == "madeRightChoiceTrue")
                        {
                            currentDialogs.Add(dialog);
                        }
                    }
                }
                else
                {
                    foreach (Dialog dialog in currentDialogTrigger.GetDialogs())
                    {
                        if (dialog.dependance == "madeRightChoiceFalse")
                        {
                            currentDialogs.Add(dialog);
                        }
                    }
                }

                break;

            case "Outsider":
                if(!talkedToPersonOutside)
                {
                    peopleTalkedTo++;

                    talkedToPersonOutside = true;
                }
                currentDialogs.Add(currentDialogTrigger.GetDialogs()[0]);
                break;
        }

        return currentDialogs.ToArray();
    }

    public void SetInDialog(bool state)
    {
        inDialog = state;
    }
}
