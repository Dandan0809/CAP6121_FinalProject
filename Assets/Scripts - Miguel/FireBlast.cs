using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlast : Ability
{
    public ParticleSystem blastEffect;
    public int maxCharges = 3;

    private int currentCharges;
    private Queue<float> rechargeQueue = new Queue<float>();

    private void Start()
    {
        currentCharges = maxCharges;
    }

    private void Update()
    {
        // Check if any queued cooldowns are finished and restore charges
        if (rechargeQueue.Count > 0 && Time.time >= rechargeQueue.Peek())
        {
            rechargeQueue.Dequeue();
            currentCharges = Mathf.Min(currentCharges + 1, maxCharges);
            icon.color = Color.white;
        }
    }

    public override void OnCast()
    {
        if (currentCharges > 0)
        {
            blastEffect.Play();

            // Start individual cooldown timer (but not overriding main Cooldown timer logic)
            rechargeQueue.Enqueue(Time.time + cooldown.TimeLeft()); // Time.time + cooldown duration
            cooldown.StartCooldown(); // We can keep this if you want to expose the "general cooldown"
            currentCharges -= 1;
            if (currentCharges == 0)
            {
                StartCoroutine(UpdateSprite());
            }
        }
    }
}
