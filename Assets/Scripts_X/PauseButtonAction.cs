using UnityEngine;

public class PauseButtonAction : MonoBehaviour
{
    [SerializeField] private GameObject wit;
    [SerializeField] private GameObject witText;
    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;
    [SerializeField] private GameObject playerManager;

    public void OnPauseButtonPressed()
    {
        // Pause the game
        Time.timeScale = 0f;

        // Deactivate Wit and its text
        if (wit != null) wit.SetActive(false);
        if (witText != null) witText.SetActive(false);

        // Deactivate player manager
        if (playerManager != null) playerManager.SetActive(false);

        // Switch panels
        if (panel1 != null) panel1.SetActive(false);
        if (panel2 != null) panel2.SetActive(true);
    }
}
