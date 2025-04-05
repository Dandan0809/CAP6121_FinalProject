using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<GolemEnemy>() != null)
        {
            other.gameObject.GetComponent<GolemEnemy>().OnDeath();
        }
    }
}
