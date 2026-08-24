using UnityEngine;
using System.Collections.Generic;
public class BlockManager : MonoBehaviour
{
    // [SerializeField] List<GameObject> Blocks = new List<GameObject>();
    [SerializeField] GameObject road;
    [SerializeField] GameObject saveArea;

    int runCounter;

    //[SerializeField] float offset=94.7f;
    //[SerializeField] float saveAreaOffset=94.7f;


    private void Start()
    {

        
        //PositionBlock();
    }

    //[ContextMenu("setPos")]
    //public void PositionBlock()
    //{
    //   //road.transform.position = new Vector3(saveArea.transform.position.x + offset, 0, transform.position.z);

    //    currentPathDir();
    //}




    public void SetSaveAreaPos(Transform pos)
    {


        saveArea.transform.position = pos.position;
        currentPathDir();

    } 
    
    //public void SetSaveAreaPosLeft()
    //{


    //    saveArea.transform.position = new Vector3(road.transform.position.x - saveAreaOffset, 0, transform.position.z);


    //}

    public void SetBlockPos(Transform pos)
    {

        road.transform.position = pos.position;

    }

  public void currentPathDir()
    {

        float i = saveArea.transform.position.x - road.transform.position.x;

       // print("iiiiiiiiiiiiiiii : "+i);


    }


    //GUIStyle style = new GUIStyle();

    //void OnGUI()
    //{

    //    style.fontSize = 30;
    //    style.normal.textColor = Color.white;
    //    GUI.Label(
    //        new Rect(20, 20, 300, 50),
    //        "Nails: " + GameManager.Instance.Nails,
    //        style
    //    );
    //}



}
