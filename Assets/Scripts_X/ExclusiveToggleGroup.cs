using UnityEngine;
using UnityEngine.UI;
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
}
