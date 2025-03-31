using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlast : Ability
{
    public ParticleSystem blastEffect;

    // Start is called before the first frame update

    public override void OnCast()
    {
        blastEffect.Play();
        cooldown.StartCooldown();
    }
}
