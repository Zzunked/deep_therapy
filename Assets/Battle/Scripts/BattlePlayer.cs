using UnityEngine;


public class BattlePlayer : BattleUnit
{
    [SerializeField] private float baseAttackDamage = 10f;

    protected override float CalculateAttackDamage()
    {
        float attackMultiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);
        float attackDamage = baseAttackDamage * attackMultiplier;

        return attackDamage;
    }

    protected override float CalculateTakenDamgeMultiplier()
    {
        float takenDamageMultiplier = 1f;

        return takenDamageMultiplier;

    }

    protected override float CalculateBlockedDamage(float damage)
    {
        float blockMultiplier = Random.Range(minBlockMultiplier, maxBlockMultiplier);
        float blockedDamage = damage * blockMultiplier;

        return blockedDamage;
    }

    public void SetBlocking()
    {
        isBlocking = true;
    }
}