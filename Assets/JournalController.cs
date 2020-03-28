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
    // Start is called before the first frame update
    void Start()
    {
        controls = new Input();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(controls.Player.Journal.triggered && !open)
        {
            StartCoroutine(PlayAnimation());
        }
        else if(open && controls.Player.Journal.triggered)
        {
            StartCoroutine(PlayAnimationBackwards());
        }
    }

    private IEnumerator PlayAnimation()
    {
        playable.Play(timeline, DirectorWrapMode.Hold);

        yield return new WaitForSeconds(7f);

        open = true;
    }

    private IEnumerator PlayAnimationBackwards()
    {
        playable.Play(timelineBackwards, DirectorWrapMode.None);

        yield return new WaitForSeconds(5f);

        open = false;
    }
}
