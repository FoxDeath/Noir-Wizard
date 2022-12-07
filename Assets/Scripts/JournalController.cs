using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class JournalController : MonoBehaviour
{
    [SerializeField] PlayableDirector playable;
    [SerializeField] TimelineAsset openJournalTimeline;
    [SerializeField] TimelineAsset closeJournalTimeline;
    [SerializeField] TimelineAsset nextPageTimeline;
    [SerializeField] TimelineAsset previousPageTimeline;
    private Input controls;
    private bool open = false;

    public static bool inJournal = false;

    private int page = 1;
    private int lastPage = 2;
    
        
    // Start is called before the first frame update
    void Start()
    {
        controls = new Input();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(!MainMenu.GameStarted)
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
            if(controls.Player.NextPage.triggered && page != lastPage)
            {
                StartCoroutine(NextPageAnimation());
            }
            if(controls.Player.PreviousPage.triggered && page != 1)
            {
                StartCoroutine(PreviousPageAnimation());
            }
        }
    }

    private void SetPageText()
    {
        
    }

    private IEnumerator NextPageAnimation()
    {
        playable.Play(nextPageTimeline, DirectorWrapMode.Hold);

        page++;
        
        SetPageText();

        yield return new WaitForSeconds(0.875f);
        
        playable.Stop();
    }
    
    private IEnumerator PreviousPageAnimation()
    {
        playable.Play(previousPageTimeline, DirectorWrapMode.Hold);

        page--;
        
        SetPageText();

        yield return new WaitForSeconds(0.875f);
        
        playable.Stop();
    }
    

    private IEnumerator OpenJournalAnimation()
    {
        playable.Play(openJournalTimeline, DirectorWrapMode.Hold);

        inJournal = true;

        yield return new WaitForSeconds(0.875f);

        open = true;
        
        playable.Stop();
    }

    private IEnumerator CloseJournalAnimation()
    {
        playable.Play(closeJournalTimeline, DirectorWrapMode.None);

        yield return new WaitForSeconds(0.625f);

        open = false;
        
        inJournal = false;
        
        playable.Stop();
    }
}
