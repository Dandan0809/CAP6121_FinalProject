using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IceSpike : Ability
{
    [SerializeField] private GameObject spikes;
    [SerializeField] private GameObject placementFX;
    [SerializeField] private LayerMask layermask;

    public InputActionReference placementAbility;
    public Transform hand;

    private ControllerAbilityManager controllerAbilityManager;
    // Start is called before the first frame update

    private void Start()
    {
        controllerAbilityManager = FindAnyObjectByType<ControllerAbilityManager>();
    }

    public override void OnCast()
    {
        StartCoroutine(PlaceAbility());
    }

    private IEnumerator PlaceAbility()
    {
        GameObject indicator;
        RaycastHit hit;
        if (Physics.Raycast(hand.position, hand.forward, out hit, 15, layermask))
        {
            Vector3 elevatedPosition = hit.point + new Vector3(0, 0.1f, 0); // Add 0.1 to Y-axis
            indicator = Instantiate(placementFX, elevatedPosition, placementFX.transform.rotation);
        }
        else
        {
Vector3 elevatedPosition = hand.position + (15 * hand.forward) + new Vector3(0, 0.1f, 0); // Add 0.1 to Y-axis
    indicator = Instantiate(placementFX, elevatedPosition, placementFX.transform.rotation);
        }

        bool isPlacing = true;

        Vector3 hoveringIndicatorPosition;

        while (isPlacing)
        {
            if (Physics.Raycast(hand.position, hand.forward, out hit, 15, layermask))
            {
                indicator.transform.position = hit.point + new Vector3(0, 0.1f, 0);
                if (placementAbility.action.WasPerformedThisFrame())
                {
                    Destroy(indicator);
                    Instantiate(spikes, hit.point, Quaternion.identity);
                    isPlacing = false;
                }
            }
            else
            {
                hoveringIndicatorPosition = (15 * hand.transform.forward);
                indicator.transform.position = new Vector3(hand.position.x + hoveringIndicatorPosition.x, 0.1f, hand.position.z + hoveringIndicatorPosition.z);
                if (placementAbility.action.WasPerformedThisFrame())
                {
                    Instantiate(spikes, indicator.transform.position, Quaternion.identity);
                    Destroy(indicator);
                    isPlacing = false;
                }
            }
            yield return null;
        }
        controllerAbilityManager.UpdatePlacement();
        cooldown.StartCooldown();
        StartCoroutine(UpdateSprite());
    }
}