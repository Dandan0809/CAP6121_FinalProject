using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    public Cooldown cooldown;
    [SerializeField] protected int manaCost;
    [SerializeField] protected Image icon;
    [SerializeField] protected TMP_Text cooldownDisplay;

    public abstract void OnCast();

    public IEnumerator UpdateSprite()
    {
        cooldownDisplay.enabled = true;
        icon.color = Color.gray;
        while (cooldown.IsCoolingDown == true)
        {
            cooldownDisplay.text = cooldown.TimeLeft().ToString("0.0");
            yield return null;
        }
        icon.color = Color.white;
        cooldownDisplay.text = "";
        cooldownDisplay.enabled = false;
    }
}
