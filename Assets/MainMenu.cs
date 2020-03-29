using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static bool GameStarted = false;

    [SerializeField] PlayableDirector playable;
    [SerializeField] TimelineAsset timeline;

    
    public void StartGame()
    {
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        playable.Play(timeline, DirectorWrapMode.Hold);

        yield return new WaitForSeconds(5f);

        GameObject.FindGameObjectWithTag("CameraFollow").GetComponent<CameraController>().GetValues();

        playable.Stop();

        GameStarted = true;
        gameObject.SetActive(false);
    }
}
