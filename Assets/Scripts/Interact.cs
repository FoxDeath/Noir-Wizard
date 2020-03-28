using UnityEngine;
using TMPro;

public class Interact : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] TextMeshProUGUI nameText;
    public bool inRange;

    public Input controls;

    private GameObject interactableObject;
    private void Start()
    {
        controls = new Input();
        controls.Enable();
        controls.Player.Interact.performed += _ => Interacting();
    }

    void Interacting()
    {
        if (!inRange)
        {
            return;
        }
        interactableObject.GetComponent<InteractWith>().Interaction();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("InteractObject"))
        {
            interactableObject = other.gameObject;
            inRange = true;

            nameText.text = "Use " + interactableObject.name;

            interactText.gameObject.SetActive(true);
            nameText.gameObject.SetActive(true);
        }
        else if(other.gameObject.tag.Equals("InteractNPC"))
        {
            interactableObject = other.gameObject;
            inRange = true;

            nameText.text = "Speak to " + interactableObject.name;

            interactText.gameObject.SetActive(true);
            nameText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag.Equals("InteractObject") || other.gameObject.tag.Equals("InteractNPC"))
        {
            interactableObject = null;
            inRange = false;

            interactText.gameObject.SetActive(false);
            nameText.gameObject.SetActive(false);
        }
    }
}
