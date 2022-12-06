using UnityEngine;

[System.Serializable]
public class Sentence
{
    public string name;
    [TextArea(3, 10)] public string line;
    public float pitch = 1f;
}
