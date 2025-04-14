using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabAbility : Ability
{
    [SerializeField] private GameObject laserFX;
    [SerializeField] private GameObject grabFX;
    [SerializeField] private LayerMask layermask;

    private GameObject enemyVFXObject;

    public InputActionReference grabbing;
    public InputActionReference release;
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
        laserFX.SetActive(true);
        RaycastHit hit = new RaycastHit();
        Rigidbody enemyBody = null;
        while (grabbing.action.ReadValue<float>() > 0.4)
        {
            if (Physics.Raycast(hand.position, hand.forward, out hit, 15, layermask))
            {
                if (hit.transform.gameObject.GetComponent<GolemEnemy>() && !hit.transform.gameObject.GetComponent<GolemEnemy>().isDead)
                {
                    hit.transform.parent = hand.transform;
                    hit.transform.localPosition = new Vector3(hand.transform.localPosition.x, hit.transform.localPosition.y, hit.transform.localPosition.z);
                    enemyBody = hit.transform.gameObject.GetComponent<Rigidbody>();
                    enemyBody.GetComponent<GolemEnemy>().OnGrab();
                    enemyVFXObject = Instantiate(grabFX, enemyBody.transform);
                    break;
                }
            }
            yield return null;
        }

        if (enemyBody == null)
        {
            controllerAbilityManager.UpdatePlacement();
            laserFX.SetActive(false);
            yield break;
        }

        bool isHolding = true;

        while (isHolding)
        {
            if (release.action.WasPressedThisFrame())
            {
                isHolding = false;
                hit.transform.parent = null;
                hit.transform.GetComponent<Rigidbody>().isKinematic = false;
                Vector3 knockbackDirection = -enemyBody.transform.forward;
                knockbackDirection.y = 0;

                // Add an upward component
                knockbackDirection += Vector3.up * 1f;

                // Normalize direction to maintain consistent force scaling
                knockbackDirection.Normalize();

                // Apply knockback force
                enemyBody.AddForce(knockbackDirection * 10f, ForceMode.Impulse);
                StartCoroutine(enemyBody.GetComponent<GolemEnemy>().RotateTowardsTarget());
                Destroy(enemyVFXObject);
                laserFX.SetActive(false);
            }
            yield return null;
        }
        controllerAbilityManager.UpdatePlacement();
        cooldown.StartCooldown();
        StartCoroutine(UpdateSprite());
    }
}