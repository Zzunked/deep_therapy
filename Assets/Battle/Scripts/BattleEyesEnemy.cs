using UnityEngine;


public class BattleEyesEnemy : BattleUnit
{
    [SerializeField] private float baseAttackDamage = 10f;
    [SerializeField] private float blockProbability = 0.5f;
    [SerializeField] private float headDamageMultiplier = 1f;
    [SerializeField] private float bodyDamageMultiplier = 1f;
    [SerializeField] private float eyesDamageMultiplier = 1f;
    private ChosenTarget targetPart;

    protected override float CalculateAttackDamage()
    {
        float attackMultiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);
        float attackDamage = baseAttackDamage * attackMultiplier;

        return attackDamage;
    }

    protected override float CalculateTakenDamgeMultiplier()
    {
        float multiplier;

        switch (targetPart)
        {
            case ChosenTarget.Head:
                multiplier = headDamageMultiplier;
                break;
            case ChosenTarget.Body:
                multiplier = bodyDamageMultiplier;
                break;
            case ChosenTarget.Eyes:
                multiplier = eyesDamageMultiplier;
                break;
            default:
                Debug.LogError("Target part was not chosen! Setting damage multiplier to 1.");
                multiplier = 1f;
                break;
        }

        return multiplier;
    }

    protected override float CalculateBlockedDamage(float damage)
    {
        float blockMultiplier = Random.Range(minBlockMultiplier, maxBlockMultiplier);
        float blockedDamage = damage * blockMultiplier;

        return blockedDamage;
    }

    public void SetTargetPart(ChosenTarget chosenTarget)
    {
        targetPart = chosenTarget;
    }

    public void ResetTargetPart()
    {
        targetPart = ChosenTarget.None;
    }

    protected override bool IsBlocking()
    {
        float blockRate = Random.Range(0f, 1f);
        Debug.Log("Enemy blockRate: " + blockRate + ", blockProbability: " + blockProbability);
        bool isBlocking = (blockRate <= blockProbability) ? true : false;

        return isBlocking;
    }
}