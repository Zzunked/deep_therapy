using UnityEngine;


// Base class for battle unit
public class BattleUnit : MonoBehaviour
{
    [SerializeField] protected int level;
    [SerializeField] protected float health;
    [SerializeField] protected float maxAttackMultiplier;
    [SerializeField] protected float minAttackMultiplier;
    [SerializeField] protected float maxBlockMultiplier;
    [SerializeField] protected float minBlockMultiplier;
    [SerializeField] protected Animator attackAnimator;
    [SerializeField] protected Animator takeDamageAnimator;
    [SerializeField] protected Animator blockAnimator;
    [SerializeField] protected Animator dieAnimator;
    [SerializeField] protected Animator winAnimator;
    [SerializeField] protected bool isDead = false;
    [SerializeField] protected bool isBlocking = false;
    protected BattleUnit targetUnit;

    public bool IsDead()
    {
        return isDead;
    }

    public void Ressurect()
    {
        health = 100;
        isDead = false;
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetTargetUnit(BattleUnit target)
    {
        targetUnit = target;
    }

    public void Attack()
    {
        float attackDamage = CalculateAttackDamage();

        PlayAttackAnimation();

        Debug.Log(gameObject.name + " attacks with damage: " + attackDamage);

        targetUnit.TakeDamage(attackDamage);
    }

    public void TakeDamage(float damage)
    {
        float takenDamageMultiplier = 1f;

        if (damage < 0)
        {
            Debug.LogError("TakeDamage damage is negative number: " + damage + ". Taking absolute value.");
            damage = Mathf.Abs(damage);
        }

        if (IsBlocking())
        {
            damage = Block(damage);
            PlayBlockAnimation();
            isBlocking = false;
        }
        else
        {
            takenDamageMultiplier = CalculateTakenDamgeMultiplier();
            PlayTakeDamageAnimation();
        }

        health -= damage * takenDamageMultiplier;

        Debug.Log(gameObject.name + " got damage: " + damage + ", health left: " + health);

        if (health <= 0)
        {
            Debug.Log(gameObject.name + " has died!");
            PlayDieAnimation();
            SetDead();
        }
    }

    private float Block(float damage)
    {
        float damageAfterBlock;
        float blockedDamage;

        if (damage < 0)
        {
            Debug.LogError("Block damage is negative number: " + damage + ". Taking absolute value.");
            damage = Mathf.Abs(damage);
        }

        blockedDamage = CalculateBlockedDamage(damage);

        damageAfterBlock = damage - blockedDamage;

        Debug.Log(gameObject.name + " blocks " + blockedDamage + " damage!");

        return damageAfterBlock;
    }

    protected void SetDead()
    {
        isDead = true;
    }

    protected void PlayAttackAnimation()
    {
        // play animation
    }

    protected void PlayTakeDamageAnimation()
    {
        // play animation
    }

    protected void PlayBlockAnimation()
    {
        // play animation
    }

    protected void PlayDieAnimation()
    {
        // play animation
    }

    protected void PlayWinAnimation()
    {
        // play animation
    }

    protected virtual float CalculateAttackDamage()
    {
        float attackDamage = 0;
        Debug.LogError("Base CalculateAttackDamage method has been called!");
        return attackDamage;
    }

    protected virtual float CalculateTakenDamgeMultiplier()
    {
        float takenDamageMultiplier = 1f;
        Debug.LogError("Base CalculateAttackDamage method has been called!");
        return takenDamageMultiplier;

    }

    protected virtual float CalculateBlockedDamage(float damage)
    {
        float blockedDamage = 0;
        Debug.LogError("Base CalculateAttackDamage method has been called!");
        return blockedDamage;
    }

    protected virtual bool IsBlocking()
    {
        return isBlocking;
    }
}