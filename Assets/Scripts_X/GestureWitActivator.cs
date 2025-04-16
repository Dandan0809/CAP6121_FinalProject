using UnityEngine;
using Meta.WitAi;
using System.Collections;
using TMPro;

public class GestureWitActivator : MonoBehaviour
{
    [Header("Wit Voice Settings")]
    [SerializeField] private VoiceService voiceService;

    [Tooltip("How long the mic stays open after a gesture (in seconds)")]
    [SerializeField] private float listenDuration = 5f;

    [Header("UI Countdown Display")]
    [SerializeField] private TextMeshProUGUI countdownText;

    private Coroutine deactivateCoroutine;

    public void OnGesturePerformed()
    {
        if (voiceService == null)
        {
            Debug.LogWarning("VoiceService is not assigned!");
            return;
        }

        //Debug.Log("Gesture detected! Activating voice.");
        voiceService.Activate();  // Starts recording

        if (deactivateCoroutine != null)
        {
            StopCoroutine(deactivateCoroutine);
        }

        deactivateCoroutine = StartCoroutine(StopListeningAfterDelay());
    }

    private IEnumerator StopListeningAfterDelay()
    {
        float timeLeft = listenDuration;

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        while (timeLeft > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = timeLeft.ToString("0");
            }

            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }

        if (voiceService != null && voiceService.Active)
        {
            //Debug.Log("Time's up. Stopping mic, sending to Wit.ai.");
            voiceService.Deactivate();  // Mic stops, audio is sent for processing
        }

        if (countdownText != null)
        {
            countdownText.text = "";
            countdownText.gameObject.SetActive(false);
        }
    }

    private void OnValidate()
    {
        if (!voiceService) voiceService = FindObjectOfType<VoiceService>();
    }
}
