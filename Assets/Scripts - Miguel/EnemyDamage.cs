using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ControllerAbilityManager>())
        {
            other.GetComponent<ControllerAbilityManager>().TakeDamage();
        }
    }
}
