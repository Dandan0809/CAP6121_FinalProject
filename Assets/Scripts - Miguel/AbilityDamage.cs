using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDamage : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<GolemEnemy>() != null)
        {
            other.GetComponent<GolemEnemy>().OnDeath();
        }
    }
}
