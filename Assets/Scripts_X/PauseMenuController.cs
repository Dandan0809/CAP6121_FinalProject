using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject abilityManager;
    [SerializeField] private GameObject wit;
    [SerializeField] private GameObject witText;
    [SerializeField] private InputActionReference pauseButtonAction;

    private bool isPaused = false;

    private void OnEnable()
    {
        if (pauseButtonAction != null)
        {
            pauseButtonAction.action.performed += OnPauseButtonPressed;
            pauseButtonAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (pauseButtonAction != null)
        {
            pauseButtonAction.action.performed -= OnPauseButtonPressed;
            pauseButtonAction.action.Disable();
        }
    }

    private void OnPauseButtonPressed(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        pausePanel.SetActive(isPaused);

        if (abilityManager != null)
            abilityManager.SetActive(!isPaused);

        if (wit != null)
            wit.SetActive(!isPaused);

        if (witText != null)
            witText.SetActive(!isPaused);
    }
}
