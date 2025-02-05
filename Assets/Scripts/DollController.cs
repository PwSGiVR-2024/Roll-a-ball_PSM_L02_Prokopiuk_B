using UnityEngine;

public class DollController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip greenLightSound;
    public AudioClip redLightSound;

    public float greenLightDuration = 3f;
    public float redLightDuration = 2f;

    private bool isGreenLight = false;

    void Start()
    {
        StartCoroutine(LightCycle());
    }

    System.Collections.IEnumerator LightCycle()
    {
        while (true)
        {
            // Green Light
            isGreenLight = true;
            audioSource.PlayOneShot(greenLightSound);
            yield return new WaitForSeconds(greenLightDuration);

            // Red Light
            isGreenLight = false;
            audioSource.PlayOneShot(redLightSound);
            yield return new WaitForSeconds(redLightDuration);
        }
    }

    public bool IsGreenLight()
    {
        return isGreenLight;
    }
}