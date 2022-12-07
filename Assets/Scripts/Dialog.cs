[System.Serializable]
public class Dialog
{
    //if it isn't dependant on anything: "initial"
    //if it is dependant on something:   the name of the bool in the player controller that in depends on
    public string dependance;
    //if it is the 1st interaction: "Could you walk me through what happened again?"
    //otherwise it is the question that leads up to the dialog
    public string optionText;

    public string journalAddition;
    
    public Sentence[] sentences;
}
