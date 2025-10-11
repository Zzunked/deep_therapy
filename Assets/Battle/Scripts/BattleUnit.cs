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
    protected virtual float Block(float damage)
    {
        Debug.LogError("Block method of base BattleUnit class has been called!");
        return 0;
    }

    public virtual void Attack()
    {
        Debug.LogError("Attack method of base BattleUnit class has been called!");
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.LogError("TakeDamage method of base BattleUnit class has been called!");
    }
}
