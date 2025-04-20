using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ExclusiveToggleGroup : MonoBehaviour
{
    public List<Toggle> toggles;

    void Start()
    {
        foreach (Toggle t in toggles)
        {
            t.onValueChanged.AddListener((isOn) => OnToggleChanged(t, isOn));
        }
    }

    void OnToggleChanged(Toggle changedToggle, bool isOn)
    {
        if (!isOn) return;

        foreach (Toggle t in toggles)
        {
            if (t != changedToggle)
                t.isOn = false;
        }
    }

    public void StartExperience()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            if (toggles[i].isOn)
            {
                int sceneIndex = i + 1; // Scene index starts at 1
                Debug.Log("Loading scene index: " + sceneIndex);
                SceneManager.LoadScene(sceneIndex);
                break;
            }
        }
    }
}
