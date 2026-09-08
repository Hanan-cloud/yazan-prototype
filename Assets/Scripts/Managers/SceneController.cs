using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{

    [SerializeField] string sceneName;

    [SerializeField] GameObject blackCanvas;
    [SerializeField] Image blackImg;

    private void OnEnable()
    {
        blackCanvas.SetActive(true);
        blackImg.DOFade(0, 2).OnComplete(() => blackCanvas.SetActive(false));
    }


    public void SetScene(string sceneName)
    {
        Time.timeScale = 1;

        DOTween.KillAll();

        blackCanvas.SetActive(true);

        blackImg.DOFade(1, 1).OnComplete(() => 
            { 
                //blackCanvas.SetActive(false);
                SceneManager.LoadScene(sceneName);
        
        
        
            }
        
        
        );

      


    }


    public void CloseGame()
    {

        Application.Quit();
    }



    private void OnDisable()
    {
        DOTween.KillAll();
    }

}
