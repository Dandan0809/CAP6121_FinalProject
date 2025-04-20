using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanelButtonsForHands : MonoBehaviour
{
    [SerializeField] private GameObject wit;
    [SerializeField] private GameObject witText;
    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;
    [SerializeField] private GameObject playerManager;

    public void OnContinuePressed()
    {
        Time.timeScale = 1f;

        if (wit != null) wit.SetActive(true);
        if (witText != null) witText.SetActive(true);
        if (panel1 != null) panel1.SetActive(true);
        if (panel2 != null) panel2.SetActive(false);
        if (playerManager != null) playerManager.SetActive(true);
    }

    public void OnRestartPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
