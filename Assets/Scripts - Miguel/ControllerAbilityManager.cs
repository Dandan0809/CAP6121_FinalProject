using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class ControllerAbilityManager : MonoBehaviour
{
    public InputActionReference blastAbility;
    public InputActionReference grabAbility;
    public InputActionReference placementAbility;

    public Ability blast;
    public Ability placement;
    public Ability grab;

    public int healthPoints = 3;
    public GameObject[] hpImages;
    public EnemySpawner enemySpawner;

    [SerializeField] private bool isPlacing = false;

    private void Start()
    {
        blastAbility.action.performed += _ => CastFireBlast();
        placementAbility.action.performed += _ => CastPlacement();
        grabAbility.action.performed += _ => CastGrab();
    }

    public void CastFireBlast()
    {
        if (!isPlacing)
            blast.OnCast();
    }

    public void CastPlacement()
    {
        if (!isPlacing && !placement.cooldown.IsCoolingDown)
        {
            placement.OnCast();
            UpdatePlacement();
        }
    }

    public void CastGrab()
    {
        if (!isPlacing && !grab.cooldown.IsCoolingDown)
        {
            grab.OnCast();
        }
    }

    public void UpdatePlacement()
    {
        isPlacing = !isPlacing;
    }

    public void TakeDamage()
    {
        healthPoints -= 1;
        hpImages[healthPoints].SetActive(false);
        if (healthPoints <= 0)
        {
            enemySpawner.EndGame();
        }
    }
}
