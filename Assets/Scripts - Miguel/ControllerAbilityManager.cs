using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ControllerAbilityManager : MonoBehaviour
{
    public InputActionReference blastAbility;
    // public InputActionReference grabAbility; Grabbing can be done with grab interactables I guess?
    public InputActionReference placementAbility;

    public Ability blast;
    public Ability placement;

    private void Start()
    {
        blastAbility.action.performed += _ => CastFireBlast();
        placementAbility.action.performed -= _ => CastPlacement();
    }

    public void CastFireBlast()
    {
        blast.OnCast();
    }

    public void CastPlacement()
    {
        placement.OnCast();
    }
}
