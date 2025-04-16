using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.Events;

public class ConVoiceSpellCaster : MonoBehaviour
{
    public Ability fireBlast;
    public Ability iceSpike;
    public GrabAbility grabAbility;

    [Header("Wit")]
    [SerializeField] private VoiceService voiceService;

    [Header("Debug Options")]
    [SerializeField] private bool logTranscript = true;

    private string lastTranscript = "";
    private bool isPlacing = false;

    private void OnEnable()
    {
        if (voiceService != null)
        {
            voiceService.VoiceEvents.OnFullTranscription.AddListener(OnTranscription);
        }
    }

    private void OnDisable()
    {
        if (voiceService != null)
        {
            voiceService.VoiceEvents.OnFullTranscription.RemoveListener(OnTranscription);
        }
    }

    private void OnTranscription(string transcription)
    {
        lastTranscript = transcription;
        if (logTranscript)
        {
            Debug.Log("Wit.ai heard: \"" + transcription + "\"");
        }
    }

    // Called from Wit.ai using the "String[] values" pattern
    public void SpellCast(string[] values)
    {
        if (values == null || values.Length == 0)
        {
            Debug.LogWarning("No recognized spell keyword. You said: \"" + lastTranscript + "\"");
            return;
        }

        string keyword = values[0].ToLower();
        Debug.Log("Recognized keyword: " + keyword);

        switch (keyword)
        {
            case "fire":
                if (fireBlast != null)
                    fireBlast.OnCast();
                else
                    Debug.LogWarning("FireBlast reference is missing!");
                break;

            case "ice":
                if (iceSpike != null)
                    iceSpike.OnCast();
                else
                    Debug.LogWarning("IceSpike reference is missing!");
                break;

            case "grab":
                CastGrab();
                break;

            default:
                Debug.LogWarning("Unrecognized keyword: \"" + keyword + "\" (You said: \"" + lastTranscript + "\")");
                break;
        }
    }

    private void CastGrab()
    {
        if (grabAbility != null)
        {
            if (!isPlacing && !grabAbility.cooldown.IsCoolingDown)
            {
                UpdatePlacement();
                grabAbility.OnCast();
            }
        }
        else
        {
            Debug.LogWarning("GrabAbility reference is missing!");
        }
    }

    private void UpdatePlacement()
    {
        isPlacing = !isPlacing;
    }
}
