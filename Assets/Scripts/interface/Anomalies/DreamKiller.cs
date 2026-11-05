using UnityEngine;

public class DreamKiller : MonoBehaviour
{

    [SerializeField] Transform leftPos;
    [SerializeField] Transform rightPos;
 void SetPos()
    {

        Vector3 temp = transform.position;
        // if player moves to the right the shadow clone appears from the left
        if (PlayerController.Instance.PlayerCurrentDir == Directions.Right)
        {
            temp.x = leftPos.position.x;
            transform.position = temp;
        }
        else // player moving to the left
        {
            temp.x = rightPos.position.x;
            transform.position = temp;


        }

      //  sprite.enabled = true;

    }


}
