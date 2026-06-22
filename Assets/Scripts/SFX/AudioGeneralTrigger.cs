using UnityEngine;
using UnityEngine.Events;

public class AudioGeneralTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent audEnter;
    [SerializeField] UnityEvent audExit;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audEnter?.Invoke();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audExit?.Invoke();
        }
    }














}
