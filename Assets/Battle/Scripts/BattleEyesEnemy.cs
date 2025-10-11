using UnityEngine;


public class BattleEyesEnemy : BattleUnit
{
    [SerializeField] private float baseAttackDamage = 10f;
    [SerializeField] private float blockProbability = 0.5f;
    [SerializeField] private float headDamageMultiplier = 1f;
    [SerializeField] private float bodyDamageMultiplier = 1f;
    [SerializeField] private float eyesDamageMultiplier = 1f;
    private ChosenTarget targetPart;

    public override void Attack()
    {
        float attackMultiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);
        float attackDamage = baseAttackDamage * attackMultiplier;

        PlayAttackAnimation();

        Debug.Log("Enemy attacks with damage: " + attackDamage);

        targetUnit.TakeDamage(attackDamage);
    }

    public override void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            Debug.LogError("TakeDamage damage is negative number: " + damage + ". Taking absolute value.");
            damage = Mathf.Abs(damage);
        }

        if (IsBlocking())
        {
            damage = Block(damage);
            PlayBlockAnimation();
        }
        else
        {
            PlayTakeDamageAnimation();
        }

        health -= damage * GetPartDamageMultiplier(targetPart);

        Debug.Log("Enemy got damage: " + damage + ", health left: " + health);

        if (health <= 0)
        {
            Debug.Log("Enemy has died!");
            PlayDieAnimation();
            SetDead();
        }
    }

    protected override float Block(float damage)
    {
        float damageAfterBlock;
        float blockedDamage;
        float blockMultiplier;

        if (damage < 0)
        {
            Debug.LogError("Block damage is negative number: " + damage + ". Taking absolute value.");
            damage = Mathf.Abs(damage);
        }

        blockMultiplier = Random.Range(minBlockMultiplier, maxBlockMultiplier);
        blockedDamage = damage * blockMultiplier;
        damageAfterBlock = damage - blockedDamage;

        Debug.Log("Enemy blocks " + blockedDamage + " damage!");

        return damageAfterBlock;
    }

    public void SetTargetPart(ChosenTarget chosenTarget)
    {
        targetPart = chosenTarget;
    }

    public void ResetTargetPart()
    {
        targetPart = ChosenTarget.None;
    }

    private bool IsBlocking()
    {
        float blockRate = Random.Range(0f, 1f);
        Debug.Log("Enemy blockRate: " + blockRate + ", blockProbability: " + blockProbability);
        bool isBlocking = (blockRate <= blockProbability) ? true : false;

        return isBlocking;
    }

    private float GetPartDamageMultiplier(ChosenTarget target)
    {
        float multiplier;

        switch (target)
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
}