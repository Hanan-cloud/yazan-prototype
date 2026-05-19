using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    int nails;

    public int Nails { get => nails; set => nails = value; }

    private void Start()
    {

        nails = 10;
    }



}
