using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    Renderer rendererGlow;
    bool flick = false;
    // Start is called before the first frame update
    void Start()
    {
        rendererGlow = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!flick)
        {
            StartCoroutine(RandomFlicker());
        }
    }

    IEnumerator RandomFlicker()
    {
        float random = Random.Range(0.9f, 1.1f);
        float randomTime = Random.Range(0.2f, 0.5f);
        rendererGlow.sharedMaterial.SetFloat("_Glow", random);

        flick = true;
        yield return new WaitForSeconds(randomTime);
        flick = false;
    }
}
