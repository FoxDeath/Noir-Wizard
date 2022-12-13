using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YEET : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("boom", 1f);
    }

    void boom()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
