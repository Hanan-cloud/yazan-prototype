
using UnityEngine;
using System.Collections;

public class RavenScream : AnomalyBase
{
    [Header("Audio")]
    [SerializeField] private AudioSource ravenAudioSource;
    [SerializeField] private AudioSource ravenScreamAudioSource;


    [Header("Anomaly Settings")]
    [SerializeField] private bool isAnomaly;

    private int screamCount;
    private Coroutine screamCoroutine;

    void Start()
    {
        SetAnomalyName();
        StartCoroutine(ScreamRoutine());
    }

    public override void SetAnomaly()
    {
        screamCount = 0;
          isAnomaly = true;


    }

    private IEnumerator ScreamRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4f, 6f));

            screamCount++;

            if (isAnomaly && screamCount % 3 == 0)
            {
                ravenScreamAudioSource.Play();
            }
            else
            {
                ravenAudioSource.Play();
            }
        }
    }

    public override void ResetAnomaly()
    {
        isAnomaly = false;
        ravenScreamAudioSource.Stop();
        screamCount = 0;
    }

    private void OnDisable()
    {
        if (screamCoroutine != null)
        {
            StopCoroutine(screamCoroutine);
            screamCoroutine = null;
        }
    }
}
