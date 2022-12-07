using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class JournalController : MonoBehaviour
{
    [SerializeField] PlayableDirector playable;
    [SerializeField] TimelineAsset timeline;
    [SerializeField] TimelineAsset timelineBackwards;
    private Input controls;
    private bool open = false;

    public static bool inJournal = false;
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
            StartCoroutine(PlayAnimation());
        }
        else if(open && controls.Player.Journal.triggered && playable.state != PlayState.Playing)
        {
            StartCoroutine(PlayAnimationBackwards());
        }
    }

    private IEnumerator PlayAnimation()
    {
        playable.Play(timeline, DirectorWrapMode.Hold);

        inJournal = true;

        yield return new WaitForSeconds(0.875f);

        open = true;
        
        playable.Stop();
    }

    private IEnumerator PlayAnimationBackwards()
    {
        playable.Play(timelineBackwards, DirectorWrapMode.None);

        yield return new WaitForSeconds(0.625f);

        open = false;
        
        inJournal = false;
        
        playable.Stop();
    }
}
