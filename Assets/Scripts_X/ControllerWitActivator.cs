using UnityEngine;
using Meta.WitAi;
using UnityEngine.InputSystem;

public class ControllerWitActivator : MonoBehaviour
{
    [Header("Wit Voice Settings")]
    [SerializeField] private VoiceService voiceService;

    [Header("Controller Input")]
    [SerializeField] private InputActionReference voiceActivateAction;

    private void OnEnable()
    {
        if (voiceActivateAction != null)
        {
            voiceActivateAction.action.started += OnButtonDown;   // Button pressed
            voiceActivateAction.action.canceled += OnButtonUp;    // Button released
            voiceActivateAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (voiceActivateAction != null)
        {
            voiceActivateAction.action.started -= OnButtonDown;
            voiceActivateAction.action.canceled -= OnButtonUp;
            voiceActivateAction.action.Disable();
        }
    }

    private void OnButtonDown(InputAction.CallbackContext context)
    {
        if (voiceService != null && !voiceService.Active)
        {
            voiceService.Activate();
            //Debug.Log("Voice activated (button held)");
        }
    }

    private void OnButtonUp(InputAction.CallbackContext context)
    {
        if (voiceService != null && voiceService.Active)
        {
            voiceService.Deactivate();
            //Debug.Log("Voice deactivated (button released)");
        }
    }

    private void OnValidate()
    {
        if (!voiceService) voiceService = FindObjectOfType<VoiceService>();
    }
}
