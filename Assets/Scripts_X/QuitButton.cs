using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}

