using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IceSpikeGesture : Ability
{
    [SerializeField] private GameObject spikes;
    [SerializeField] private GameObject placementFX;
    [SerializeField] private LayerMask layermask;

    public Transform hand;
    private bool isGesturing = false;

    public override void OnCast()
    {
        if (!cooldown.IsCoolingDown)
        {
            isGesturing = true;
            StartCoroutine(PlaceAbility());
        }
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
                if (!isGesturing)
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
                if (!isGesturing)
                {
                    Instantiate(spikes, indicator.transform.position, Quaternion.identity);
                    Destroy(indicator);
                    isPlacing = false;
                }
            }
            yield return null;
        }
        cooldown.StartCooldown();
        StartCoroutine(UpdateSprite());
    }

    public void EndGesture()
    {
        isGesturing = false;
    }
}