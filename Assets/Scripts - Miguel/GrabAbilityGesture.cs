using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAbilityGesture : Ability
{
    [SerializeField] private GameObject grabFX;
    [SerializeField] private LayerMask layermask;
    [SerializeField] private GameObject laserFX;
    [SerializeField] private Transform fireBlastCone;

    private GameObject enemyVFXObject;
    public Transform hand;
    public float delay = 0.5f;

    public IceSpikeGesture spike;

    private bool isGesturing = false;

    // Start is called before the first frame update

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
        RaycastHit hit = new RaycastHit();
        Rigidbody enemyBody = null;
        fireBlastCone.position = new Vector3(fireBlastCone.position.x, fireBlastCone.position.y + 10, fireBlastCone.position.z);
        laserFX.SetActive(true);
        while (isGesturing)
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

        if (!isGesturing && enemyBody == null)
        {
            laserFX.SetActive(false);
            fireBlastCone.position = new Vector3(fireBlastCone.position.x, fireBlastCone.position.y - 10, fireBlastCone.position.z);
            yield break;
        }

        bool isGrabbing = true;

        while (isGrabbing)
        {
            if (!isGesturing)
            {
                isGrabbing = false;
                enemyBody.GetComponent<Rigidbody>().isKinematic = false;
                hit.transform.parent = null;
                Vector3 knockbackDirection = Camera.main.transform.forward;
                knockbackDirection.y = 0;

                // Add an upward component
                knockbackDirection += Vector3.up * 0.3f;

                // Normalize direction to maintain consistent force scaling
                knockbackDirection.Normalize();

                // Apply knockback force
                enemyBody.AddForce(knockbackDirection * 10f, ForceMode.Impulse);
                StartCoroutine(enemyBody.GetComponent<GolemEnemy>().RotateTowardsTarget());
                Destroy(enemyVFXObject);
            }
            yield return null;
        }
        laserFX.SetActive(false);
        fireBlastCone.position = new Vector3(fireBlastCone.position.x, fireBlastCone.position.y - 10, fireBlastCone.position.z);
        cooldown.StartCooldown();
        StartCoroutine(UpdateSprite());
        StartCoroutine(SpikeDelay());
    }

    IEnumerator SpikeDelay()
    {
        spike.canPlace = false;
        yield return new WaitForSeconds(delay);
        spike.canPlace = true;
    }


    public void EndGesture()
    {
        isGesturing = false;
    }
}
