using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Girl : MonoBehaviour
{
    [SerializeField] AudioSource girlSingingAudioSource;
    [SerializeField] AudioSource rotationAudioSource;
    [SerializeField] AudioClip girlSinging;
    [SerializeField] AudioClip rotateSoundClip;
    [SerializeField] float totalTime = 70f; // 70 seconds
    [SerializeField] float breakTime = 4f; // 4-second break
    readonly float initialSoundDuration = 5f; // Initial duration of the sound in seconds
    readonly float finalSoundDuration = 2.5f; // Final duration of the sound in seconds
    float elapsedTime = 0f; // Tracks the elapsed time
    bool isPlaying = false;
    Coroutine rotationCoroutine = null;
    Coroutine killCoroutine = null;
    Player player;
    List<NPC> npcs = new List<NPC>();
    Transform head;
    bool scanning = false;
    bool gameStarted = false;

    void Awake()
    {
        if (girlSingingAudioSource == null || girlSinging == null || rotationAudioSource == null || rotateSoundClip == null)
        {
            Debug.LogError("Audio sources or Sound clips not assigned!");
            return;
        }

        girlSingingAudioSource.clip = girlSinging;
        girlSingingAudioSource.loop = false;

        rotationAudioSource.clip = rotateSoundClip;
        rotationAudioSource.loop = false;

        // Get the head object
        head = transform.Find("DollHead");

        // Find GameObject with tag Player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // Find all GameObjects with tag NPC
        npcs.AddRange(GameObject.FindGameObjectsWithTag("NPC").Select(npc => npc.GetComponent<NPC>()));
    }

    void Update()
    {
        if (!gameStarted)
        {
            return;
        }
        if (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;

            if (!isPlaying)
            {
                killCoroutine = null;
                float currentSoundDuration = Mathf.Lerp(initialSoundDuration, finalSoundDuration, elapsedTime / totalTime);
                girlSingingAudioSource.pitch = initialSoundDuration / currentSoundDuration; // Adjust pitch to change speed
                EventManager.Instance.TriggerEvent("GirlSinging", currentSoundDuration);
                girlSingingAudioSource.Play();
                isPlaying = true;

                // Schedule stopping the sound after its current duration
                Invoke(nameof(StopSound), currentSoundDuration);
            }
            // remaining time in seconds and milliseconds and show it in red in Text mesh pro
            string remainingTime = (totalTime - elapsedTime).ToString("F2");
            EventManager.Instance.TriggerEvent("Timer", remainingTime);
        }

        if (elapsedTime >= totalTime)
        {
            killCoroutine = null;
            EventManager.Instance.TriggerEvent("TimeIsUp", null);
            // Game Over!
            if (!player.PlayerIsDead())
            {
                player.KillPlayer();
            }
            killCoroutine = StartCoroutine(KillNPCs(npcs.ToList()));
            return;
        }
        if (scanning)
        {
            if (player.IsMoving)
            {
                player.KillPlayer();
            }
            if (killCoroutine == null)
            {
                killCoroutine = StartCoroutine(KillNPCs(npcs.Where(npc => npc.IsMoving).ToList()));
            }

        }
    }

    private void OnEnable()
    {
        EventManager.Instance.Subscribe("CountdownFinished", StartGame);
    }

    IEnumerator KillNPCs(List<NPC> npcs)
    {
        foreach (var npc in npcs)
        {
            if (npc.PlayerIsDead()) continue;
            npc.KillPlayer();
            yield return new WaitForSeconds(Random.Range(0f, 0.4f));
        }
    }

    void RotateHead(bool roateBack = false)
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
        rotationAudioSource.Play();
        rotationCoroutine = StartCoroutine(RotateHeadOverTime(0.2f, roateBack));
    }

    void StopSound()
    {
        girlSingingAudioSource.Stop();
        RotateHead();

        // Ensure the next play happens after the break time
        Invoke(nameof(ResumePlayback), breakTime);
    }

    void ResumePlayback()
    {
        isPlaying = false;
        scanning = false;
        RotateHead(true);
    }

    IEnumerator RotateHeadOverTime(float seconds, bool rotateBack = false)
    {
        float elapsedTime = 0;
        Quaternion startRotation = head.rotation;
        Quaternion endRotation = Quaternion.Euler(0, rotateBack ? 0 : 180, 0);

        while (elapsedTime < seconds)
        {
            head.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / seconds);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        scanning = !rotateBack;
    }

    public void StartGame(object message)
    {
        gameStarted = true;
    }
}
