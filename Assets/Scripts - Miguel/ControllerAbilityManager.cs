using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ControllerAbilityManager : MonoBehaviour
{
    public InputActionReference blastAbility;
    public InputActionReference grabAbility;
    public InputActionReference iceAbility;

    public Ability blast;
    public Ability ice;
    public GrabAbility grab;

    [SerializeField] private bool isPlacing = false;

    private void Start()
    {
        blastAbility.action.performed += _ => CastFireBlast();
        iceAbility.action.performed += _ => CastPlacement();
        grabAbility.action.performed += _ => CastGrab();

    }

    public void CastFireBlast()
    {
        if (!isPlacing)
            blast.OnCast();
    }

    public void CastPlacement()
    {
        if (!isPlacing && !ice.cooldown.IsCoolingDown)
        {
            ice.OnCast();
            UpdatePlacement();
        }
    }

    public void CastGrab()
    {
        if (!isPlacing && !grab.cooldown.IsCoolingDown)
        {
            UpdatePlacement();
            grab.OnCast();
        }
    }

    public void UpdatePlacement()
    {
        isPlacing = !isPlacing;
    }
}
