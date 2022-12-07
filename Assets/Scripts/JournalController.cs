using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;
using UnityEngine.Playables;


[System.Serializable]
public class Page
{
    public string leftText = "";
    public string rightText = "";
}

public class JournalController : MonoBehaviour
{
    private Interact Interact;
    
    [SerializeField] PlayableDirector playable;
    [SerializeField] TimelineAsset openJournalTimeline;
    [SerializeField] TimelineAsset closeJournalTimeline;
    [SerializeField] TimelineAsset nextPageTimeline;
    [SerializeField] TimelineAsset previousPageTimeline;
    private Input controls;
    private bool open = false;

    public static bool inJournal = false;

    [SerializeField] private TMP_Text leftText;
    [SerializeField] private TMP_Text rightText;
    [SerializeField] private TMP_Text leftNextText;
    [SerializeField] private TMP_Text rightNextText;
    [SerializeField] private TMP_Text leftPreviousText;
    [SerializeField] private TMP_Text rightPreviousText;

    private List<Page> pages = new List<Page>();

    private int page = 1;
    
        
    // Start is called before the first frame update
    void Start()
    {
        Interact = FindObjectOfType<Interact>();
        
        controls = new Input();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(!MainMenu.GameStarted || Interact.GetInDialog())
        {
            return;
        }

        if(controls.Player.Journal.triggered && !open && playable.state != PlayState.Playing)
        {
            StartCoroutine(OpenJournalAnimation());
        }
        else if(open && controls.Player.Journal.triggered && playable.state != PlayState.Playing)
        {
            StartCoroutine(CloseJournalAnimation());
        }

        if(open && playable.state != PlayState.Playing)
        {
            if(controls.Player.NextPage.triggered && page != pages.Count)
            {
                StartCoroutine(NextPageAnimation());
            }
            if(controls.Player.PreviousPage.triggered && page != 1)
            {
                StartCoroutine(PreviousPageAnimation());
            }
        }
    }

    public void AddPage(string pageText)
    {
        if(pages.Count == 0)
        {
            Page newPage = new Page();

            newPage.leftText = pageText;

            pages.Add(newPage);

            leftText.text = newPage.leftText;
            
            return;
        }

        foreach(var page in pages)
        {
            if(page.leftText.Equals(pageText) || page.rightText.Equals(pageText))
            {
                return;
            }
        }
        
        if(pages.Last().rightText != String.Empty)
        {
            Page newPage = new Page();

            newPage.leftText = pageText;

            pages.Add(newPage);
        }
        else
        {
            pages.Last().rightText = pageText;

            if(pages.Count == page)
            {
                rightText.text = pageText;
            }
        }
    }

    private IEnumerator NextPageAnimation()
    {
        playable.Play(nextPageTimeline, DirectorWrapMode.Hold);

        page++;

        leftNextText.text = rightText.text;

        rightNextText.text = pages[page - 1].leftText;
        
        yield return new WaitUntil(() => playable.time > (float)nextPageTimeline.duration / 4f);
        rightText.text = pages[page - 1].rightText;

        
        yield return new WaitUntil(() => playable.time > (float)nextPageTimeline.duration / 1.33f);
        
        leftText.text = pages[page - 1].leftText;
        
        yield return new WaitUntil(() => playable.time == nextPageTimeline.duration);

        leftNextText.text = "";

        rightNextText.text = "";

        playable.Stop();
    }
    
    private IEnumerator PreviousPageAnimation()
    {
        playable.Play(previousPageTimeline, DirectorWrapMode.Hold);

        page--;
        
        rightPreviousText.text = leftText.text;

        leftPreviousText.text = pages[page - 1].rightText;

        yield return new WaitUntil(() => playable.time > (float)previousPageTimeline.duration / 4f);
        
        leftText.text = pages[page - 1].leftText;

        yield return new WaitUntil(() => playable.time > (float)previousPageTimeline.duration / 1.33f);
        
        rightText.text = pages[page - 1].rightText;

        yield return new WaitUntil(() => playable.time == previousPageTimeline.duration);

        leftPreviousText.text = "";

        rightPreviousText.text = "";

        playable.Stop();
    }
    

    private IEnumerator OpenJournalAnimation()
    {
        playable.Play(openJournalTimeline, DirectorWrapMode.Hold);

        inJournal = true;

        yield return new WaitForSeconds((float)openJournalTimeline.duration);

        open = true;
        
        playable.Stop();
    }

    private IEnumerator CloseJournalAnimation()
    {
        playable.Play(closeJournalTimeline, DirectorWrapMode.None);

        yield return new WaitForSeconds((float)closeJournalTimeline.duration);

        open = false;
        
        inJournal = false;
        
        playable.Stop();
    }
}
