
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonSound : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Random Pitch")]
    private float minPitch = 1.1f;
    private float maxPitch = 1.4f;

    public void OnSelect(BaseEventData eventData)
    {
        PlaySound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.Play();
        }
    }
}
