using UnityEngine;

[System.Serializable]
public class Cooldown
{
    #region Variables

    [SerializeField] private float cooldownTime;
    private float _nextFireTime;

    #endregion

    public void SetCooldownTime(float cooldownTime) => this.cooldownTime = cooldownTime;
    public void SetNextFireTime(float fireTime) => _nextFireTime = fireTime;
    public float GetNextFireTime() => _nextFireTime;

    public bool IsCoolingDown => Time.time < _nextFireTime;

    public float TimeLeft() => _nextFireTime - Time.time;
    public void StartCooldown() => _nextFireTime = Time.time + cooldownTime;
}