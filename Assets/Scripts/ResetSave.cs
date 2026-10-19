using UnityEngine;

public class ResetSave : MonoBehaviour
{



    [SerializeField] GameObject resets;


    void Start()
    {
        resets.SetActive(false);

    }
    public void Reset()
    {
        if (ES3.FileExists()){
            ES3.DeleteFile();
            resets.SetActive(true);
            
            
            }


    }


}
