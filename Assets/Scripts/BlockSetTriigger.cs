using UnityEngine;
using UnityEngine.Events;

public class BlockSetTriigger : MonoBehaviour
{


    [SerializeField] UnityEvent action;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            action?.Invoke();

        }
    }
}
