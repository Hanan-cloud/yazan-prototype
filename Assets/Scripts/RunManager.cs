using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;



    Directions runDirection;
    Directions correctDirection;

    public Directions RunDirection { get => runDirection; }
    public Directions CorrectDirection { get => correctDirection; }

    private void Awake()
    {
        Instance = this;
    }

    public void SetRunDir(Directions dir)
    {
        runDirection = dir;
    }
    public void SetCorrectDir(Directions dir)
    {
        correctDirection = dir;

    }

}
