using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireBlast : Ability
{
    public ParticleSystem blastEffect;
    public int maxCharges = 3;
    public TextMeshProUGUI chargeText;

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
            chargeText.text = $"{currentCharges}";
        }
    }

    public override void OnCast()
    {
        if (currentCharges > 0)
        {
            blastEffect.Play();
            currentCharges -= 1;
            chargeText.text = $"{currentCharges}";
            cooldown.StartCooldown();
            rechargeQueue.Enqueue(Time.time + cooldown.TimeLeft());
            if (currentCharges == 0)
            {
                StartCoroutine(UpdateSprite());
            }
        }
    }
    public float NextChargeCooldownTimeLeft()
    {
        if (rechargeQueue.Count == 0)
            return 0f;

        float nextReadyTime = rechargeQueue.Peek();
        return Mathf.Max(0f, nextReadyTime - Time.time);
    }
}
