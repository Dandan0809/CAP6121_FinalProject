using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int healthPoints = 3;
    public GameObject[] hpImages;
    public EnemySpawner enemySpawner;

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
