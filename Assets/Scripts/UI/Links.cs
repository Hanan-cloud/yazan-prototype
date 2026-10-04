using UnityEngine;

public class Links : MonoBehaviour
{

    string playtest = "https://docs.google.com/forms/d/e/1FAIpQLSclu_AD7NjX17dmZHLE_vcVXcAIAJ5g95GTIZnMHxNetF1-Mg/viewform?usp=dialog"; 
    public void OpenPlaytest()
    {
        Application.OpenURL(playtest);
    }
}
