using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static bool musicExists;

    void Awake()
    {
        if (!musicExists)
        {
            DontDestroyOnLoad(gameObject);
            musicExists = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

