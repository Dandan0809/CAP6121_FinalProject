using UnityEngine;
using Meta.WitAi;

public class GestureWitActivator : MonoBehaviour
{
    [SerializeField] private VoiceService voiceService;

    public void OnGesturePerformed()
    {
        if (voiceService != null)
        {
            Debug.Log("Gesture detected! Activating voice.");
            voiceService.Activate();
        }
        else
        {
            Debug.LogWarning("VoiceService is not assigned!");
        }
    }

    private void OnValidate()
    {
        if (!voiceService) voiceService = FindObjectOfType<VoiceService>();
    }
}

