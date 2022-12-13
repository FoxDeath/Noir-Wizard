﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    public static bool GameStarted;

    [SerializeField] PlayableDirector playable;
    [SerializeField] TimelineAsset timeline;

    private void Start()
    {
        GameStarted = false;
    }


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

    public void Quit()
    {
        Application.Quit();
    }
}
