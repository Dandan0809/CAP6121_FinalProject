using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireBlast : Ability
{
    public ParticleSystem blastEffect;
    public int maxCharges = 3;
    public TextMeshProUGUI chargeText;
    public List<GameObject> objectsInTrigger = new List<GameObject>();
    [SerializeField] private Transform fireRay;


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

        if (currentCharges == 0)
        {
            if (!cooldownDisplay.enabled)
            {
                cooldownDisplay.enabled = true;
                icon.color = Color.gray;
            }
            cooldownDisplay.text = NextChargeCooldownTimeLeft().ToString("0.0");
        }
        else
        {
            icon.color = Color.white;
            cooldownDisplay.text = "";
            cooldownDisplay.enabled = false;
        }
    }

    public override void OnCast()
    {
        if (currentCharges > 0)
        {
            Transform target = FindClosestObjectToLine();
            if (target != null)
            {
                blastEffect.transform.LookAt(target);
            }
            else
            {
                blastEffect.transform.rotation = fireRay.transform.rotation;
            }
            blastEffect.Play();
            currentCharges -= 1;
            chargeText.text = $"{currentCharges}";
            cooldown.StartCooldown();
            rechargeQueue.Enqueue(Time.time + cooldown.TimeLeft());
        }
    }
    public float NextChargeCooldownTimeLeft()
    {
        if (rechargeQueue.Count == 0)
            return 0f;

        float nextReadyTime = rechargeQueue.Peek();
        return Mathf.Max(0f, nextReadyTime - Time.time);
    }

    public Transform FindClosestObjectToLine()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject obj in objectsInTrigger)
        {
            if (obj == null) continue;
            Vector3 toPoint = obj.transform.position - fireRay.position;
            Vector3 projection = Vector3.Project(toPoint, fireRay.forward.normalized);
            Vector3 rejection = toPoint - projection;

            float distanceToLine = rejection.magnitude;

            if (distanceToLine < minDistance)
            {
                minDistance = distanceToLine;
                closest = obj.transform;
            }
        }
        if (closest != null)
        {
            return closest.GetComponent<GolemEnemy>().targetLocation;
        }
        return null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!objectsInTrigger.Contains(other.gameObject) && other.gameObject.GetComponent<GolemEnemy>())
        {
            objectsInTrigger.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInTrigger.Contains(other.gameObject) && other.gameObject.GetComponent<GolemEnemy>() && other != null)
        {
            objectsInTrigger.Remove(other.gameObject);
        }
    }

}
