using UnityEngine;


public class BattlePlayer : BattleUnit
{
    [SerializeField] private float baseAttackDamage = 10f;
    private bool isBlocking = false;

    public override void Attack()
    {
        float attackMultiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);
        float attackDamage = baseAttackDamage * attackMultiplier;

        PlayAttackAnimation();

        Debug.Log("Player attacks with damage: " + attackDamage);

        targetUnit.TakeDamage(attackDamage);
    }

    public override void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            Debug.LogError("TakeDamage damage is negative number: " + damage + ". Taking absolute value.");
            damage = Mathf.Abs(damage);
        }

        if (isBlocking)
        {
            damage = Block(damage);
            isBlocking = false;
            PlayBlockAnimation();
        }
        else
        {
            PlayTakeDamageAnimation();
        }

        health -= damage;

        Debug.Log("Player got damage: " + damage + ", health left: " + health);

        if (health <= 0)
        {
            Debug.Log("Player has died!");
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

        Debug.Log("Player blocks " + blockedDamage + " damage!");

        return damageAfterBlock;
    }

    public void SetBlocking()
    {
        isBlocking = true;
    }
}