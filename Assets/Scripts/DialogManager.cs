using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogManager : MonoBehaviour
{
    private AudioManager AudioManager;
    private JournalController JournalController;
    
    private Animator uiAnim;
    private Queue<Sentence> sentences;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI dialogText;
    private Coroutine currentTypeSentenceCoroutine;
    private Transform dialogFrame;
    private Dialog neverMindDialog;
    private Dialog[] currentDialogs;
    private Walking playerWalking;
    private Interact interactController;
    private Transform pileOfAsh;
    private Dialog currentDialog;

    [SerializeField] GameObject choicePrefab;
    [SerializeField] GameObject phoenix;
    private GameObject choiceGrid;
    private GameObject continueButton;

    // Start is called before the first frame update
    void Start()
    {
        JournalController = FindObjectOfType<JournalController>();
        AudioManager = AudioManager.instance;

        uiAnim = GameObject.Find("UI").GetComponent<Animator>();
        dialogFrame = GameObject.Find("UI").transform.GetChild(0);
        dialogFrame.gameObject.SetActive(false);
        sentences = new Queue<Sentence>();
        nameText = dialogFrame.GetChild(0).GetComponent<TextMeshProUGUI>();
        dialogText = dialogFrame.GetChild(1).GetComponent<TextMeshProUGUI>();
        choiceGrid = dialogFrame.GetChild(2).GetChild(0).gameObject;
        continueButton = dialogFrame.GetChild(3).gameObject;
        continueButton.GetComponent<Button>().onClick.AddListener(() => DisplayNextSentence());
        currentTypeSentenceCoroutine = null;
        neverMindDialog = new Dialog();
        neverMindDialog.optionText = "Never mind.";
        playerWalking = GameObject.Find("Player").GetComponent<Walking>();
        interactController = GameObject.Find("Player").GetComponent<Interact>();
        pileOfAsh = GameObject.Find("Ash").transform;
    }

    public void InitializeDialogs(Dialog[] dialogs)
    {
        //resets choice grid
        foreach (Transform child in choiceGrid.transform)
        {
            Destroy(child.gameObject);
        }

        //sets the current dialogs and populates the choices grid
        currentDialogs = new Dialog[dialogs.Length];

        for(int i = 0; i < dialogs.Length; i++)
        {
            currentDialogs[i] = dialogs[i];

            AddChoice(dialogs[i]);
        }

        //if only one dialog was passed along it starts that automatically
        if(dialogs.Length == 1)
        {
            StartDialog(dialogs[0]);
        }
        //otherwise it creates buttons with the current options
        else
        {
            AddChoice(neverMindDialog);
            playerWalking.SetCanWalk(false);
            interactController.SetInDialog(true);
            uiAnim.SetBool("Enabled", true);
            uiAnim.SetTrigger("ChangeToChoices");
            uiAnim.SetFloat("OnChoices", 1f);
        }
    }

    private void AddChoice(Dialog dialog)
    {
        GameObject currentChoice;
        currentChoice = Instantiate(choicePrefab, choiceGrid.transform);
        currentChoice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dialog.optionText;
        currentChoice.GetComponent<Button>().onClick.AddListener(() => StartDialog(dialog));
    }

    private void StartDialog(Dialog dialog)
    {
        currentDialog = dialog;

        if(dialog.dependance == "talkedToEveryone")
        {
            EndDialog();

            if(dialog.sentences[0].name == "true")
            {
                interactController.SetMadeRightChoice(true);
            }
            else
            {
                interactController.SetMadeRightChoice(false);
            }

            StartCoroutine(EndGame());
        }
        else if (dialog.optionText == "Never mind.")
        {
            playerWalking.SetCanWalk(true);
            interactController.SetInDialog(false);
            uiAnim.SetBool("Enabled", false);
        }
        else
        {
            playerWalking.SetCanWalk(false);
            interactController.SetInDialog(true);
            uiAnim.SetBool("Enabled", true);
            uiAnim.SetTrigger("ChangeToDialog");
            uiAnim.SetFloat("OnChoices", -1f);

            this.sentences.Clear();

            foreach (Sentence sentence in dialog.sentences)
            {
                this.sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        Sentence sentence = sentences.Dequeue();
        nameText.text = sentence.name;

        if(currentTypeSentenceCoroutine != null)
        {
            StopCoroutine(currentTypeSentenceCoroutine);
        }
        
        currentTypeSentenceCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    private void EndDialog()
    {
        currentDialogs = interactController.SetCurrentDialogs();

        if(currentDialogs.Length != 0 && currentDialogs[0].optionText == "phoenix")
        {
            EndEndGame();
        }
        else
        {
            if (currentDialogs.Length > 1f && (currentDialog != null && currentDialog.dependance != "talkedToEveryone"))
            {
                InitializeDialogs(currentDialogs);
            }
            else
            {
                playerWalking.SetCanWalk(true);
                interactController.SetInDialog(false);
                uiAnim.SetBool("Enabled", false);

                StopCoroutine(currentTypeSentenceCoroutine);
            }
        }

        if(currentDialog.journalAddition != String.Empty)
        {
            JournalController.AddPage(currentDialog.journalAddition);
        }

        string objective = "";

        if(Interact.peopleTalkedTo != 11 && !Interact.talkedToBarBarOwnerOrDimitriAboutCat)
        {
            objective = "Objectives" + "\n" + $"Investigate the scene [{Interact.peopleTalkedTo}/11]" + "\n";
        }
        else if(Interact.peopleTalkedTo == 11 && !Interact.talkedToBarBarOwnerOrDimitriAboutCat)
        {
            objective = "Objectives" + "\n" + $"<s>Investigate the scene [{Interact.peopleTalkedTo}/11]</s>" + "\n";
        }
        else if(Interact.peopleTalkedTo == 11 && Interact.talkedToBarBarOwnerOrDimitriAboutCat)
        {
            objective = "Time to investigate the ashes and make a decision";
            
            JournalController.AddObjective(objective);

            return;
        }

        if(Interact.talkedToSilvester && !Interact.talkedToBarBarOwnerOrDimitriAboutCat)
        {
            objective += "Ask around about the cat";
        }
        else if(Interact.talkedToSilvester && Interact.talkedToBarBarOwnerOrDimitriAboutCat)
        {
            objective += "<s>Ask around about the cat</s>";
        }
        
        JournalController.AddObjective(objective);
    }

    private void EndEndGame()
    {
        FindObjectOfType<CameraController>().EndGame(phoenix.transform);
        
        
        interactController.SetInDialog(false);
        uiAnim.SetBool("Enabled", false);
    }


    IEnumerator TypeSentence(Sentence sentence)
    {
        dialogText.text = "";

        int i = 0;
        
        foreach(char letter in sentence.line)
        {
            i++;

            if(i == 6)
            {
                i = 0;
                
                AudioManager.SetPitch("Talk", Random.Range(sentence.pitch - 0.1f, sentence.pitch + 0.1f));
                
                AudioManager.Play("Talk");
            }
            dialogText.text += letter;
            yield return new WaitForSecondsRealtime(0.015f);
        }
    }

    IEnumerator EndGame()
    {
        phoenix.SetActive(true);
            
        pileOfAsh.gameObject.SetActive(false);
        
        playerWalking.SetCanWalk(false);

        yield return new WaitForSecondsRealtime(6f);
        
        playerWalking.SetCanWalk(true);
    }
}
