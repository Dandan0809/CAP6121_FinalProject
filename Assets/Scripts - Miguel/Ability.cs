using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    protected int abilityIndex;
    public Cooldown cooldown;
    [SerializeField] protected int manaCost;
    [SerializeField] protected Sprite abilityImg;
    [SerializeField] protected Image icon;
    [SerializeField] protected TMP_Text cooldownDisplay;

    public abstract int OnCast(int currentMana, Transform pos, ResourceBarHandler manaBar);

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

    public void SetIconValues(GameObject iconObject)
    {
        icon = iconObject.GetComponent<Image>();
        icon.sprite = abilityImg;
        cooldownDisplay = icon.GetComponentInChildren<TMP_Text>(true);
        icon.color = Color.white;
    }

    public Sprite GetSprite { get { return abilityImg; } }
}
