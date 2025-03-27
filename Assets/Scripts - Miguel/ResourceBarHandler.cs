using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResourceBarHandler : MonoBehaviour
{
    [SerializeField] private RectTransform resourceBar;
    [SerializeField] private float maxResourceValue;
    [SerializeField] private float resourceBarMaxWidth;


    private void Start()
    {
        resourceBar = GetComponent<RectTransform>();
        resourceBarMaxWidth = resourceBar.rect.width;
    }

    public void SetStartingResourceValues(float resourceValue)
    {
        maxResourceValue = resourceValue;
    }

    public void UpdateResourceBar(float newValue)
    {
        resourceBar.sizeDelta = new Vector2((newValue / maxResourceValue) * resourceBarMaxWidth, resourceBar.sizeDelta.y);
    }
}
