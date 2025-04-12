using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi;

public class WitActivation : MonoBehaviour
{
    [SerializeField] private VoiceService voiceService;

    private void OnValidate()
    {
        if (!voiceService) voiceService = GetComponent<VoiceService>();
    }

    void Update()
    {
        if (voiceService != null && Input.GetKeyDown(KeyCode.Space))
        {
            voiceService.Activate();
        }
    }
}
