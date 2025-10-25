using System.Collections;
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
    // [SerializeField] protected Animator attackAnimator;
    // [SerializeField] protected Animator takeDamageAnimator;
    // [SerializeField] protected Animator blockAnimator;
    // [SerializeField] protected Animator dieAnimator;
    // [SerializeField] protected Animator winAnimator;
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

    public IEnumerator Attack()
    {
        float attackDamage = CalculateAttackDamage();

        Debug.Log(gameObject.name + " attacks with damage: " + attackDamage);

        yield return StartCoroutine(targetUnit.TakeDamage(attackDamage));
    }

    public IEnumerator TakeDamage(float damage)
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
            yield return StartCoroutine(PlayBlockAnimation());
            isBlocking = false;
        }
        else
        {
            takenDamageMultiplier = CalculateTakenDamgeMultiplier();
            yield return StartCoroutine(PlayTakeDamageAnimation());
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

    protected virtual IEnumerator PlayTakeDamageAnimation()
    {
        yield return null;
        // play animation
    }

    protected virtual IEnumerator PlayBlockAnimation()
    {
        yield return null;
        // play animation
    }

    protected virtual void PlayDieAnimation()
    {
        // play animation
    }

    protected virtual void PlayWinAnimation()
    {
        // play animation
    }

    protected IEnumerator PlayAnimation(string animation, Animator animator)
    {
        animator.Play(animation);

        // Wait until the animation starts
        yield return null;

        // Get info about the current animation
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        Debug.Log("Playing " + animation + " animation for " + info.length + "s");

        // Wait for the duration of the animation
        yield return new WaitForSeconds(info.length);
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

    public virtual bool IsBlocking()
    {
        return isBlocking;
    }
}