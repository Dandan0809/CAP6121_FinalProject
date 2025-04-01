using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabAbility : Ability
{
    [SerializeField] private GameObject grabFX;
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

        RaycastHit hit;
        Rigidbody enemyBody;
        if (Physics.Raycast(hand.position, hand.forward, out hit, 15, layermask))
        {
            hit.transform.parent = hand.transform;
            enemyBody = hit.transform.gameObject.GetComponent<Rigidbody>();
            controllerAbilityManager.UpdatePlacement();
        }
        else
        {
            yield break;
        }

        bool isGrabbing = true;

        while (isGrabbing)
        {
            if (placementAbility.action.WasPerformedThisFrame())
            {
                isGrabbing = false;
                hit.transform.parent = null;
                enemyBody.AddExplosionForce(5f, enemyBody.transform.position, 3f);
            }
            yield return null;
        }
        controllerAbilityManager.UpdatePlacement();
        cooldown.StartCooldown();
    }
}