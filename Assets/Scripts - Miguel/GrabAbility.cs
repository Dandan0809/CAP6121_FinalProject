using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabAbility : Ability
{
    [SerializeField] private GameObject grabFX;
    [SerializeField] private LayerMask layermask;
    private GameObject enemyVFXObject;

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
            if (hit.transform.gameObject.GetComponent<GolemEnemy>() && !hit.transform.gameObject.GetComponent<GolemEnemy>().isDead)
            {
                hit.transform.parent = hand.transform;
                enemyBody = hit.transform.gameObject.GetComponent<Rigidbody>();
                controllerAbilityManager.UpdatePlacement();
                enemyBody.GetComponent<GolemEnemy>().OnGrab();
                enemyVFXObject = Instantiate(grabFX, enemyBody.transform);
            }
            else
            {
                yield break;
            }
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
                hit.transform.GetComponent<Rigidbody>().isKinematic = false;
                hit.transform.parent = null;
                Vector3 knockbackDirection = -enemyBody.transform.forward;
                knockbackDirection.y = 0;

                // Add an upward component
                knockbackDirection += Vector3.up * 2f;

                // Normalize direction to maintain consistent force scaling
                knockbackDirection.Normalize();

                // Apply knockback force
                enemyBody.AddForce(knockbackDirection * 5f, ForceMode.Impulse);
                StartCoroutine(enemyBody.GetComponent<GolemEnemy>().RotateTowardsTarget());
                Destroy(enemyVFXObject);
            }
            yield return null;
        }
        controllerAbilityManager.UpdatePlacement();
        cooldown.StartCooldown();
        StartCoroutine(UpdateSprite());
    }
}