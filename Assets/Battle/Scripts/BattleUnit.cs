using System.Collections;
using UnityEngine;


// Base class for battle unit
public class BattleUnit : MonoBehaviour
{
    [SerializeField] protected int _level;
    [SerializeField] protected float _health;
    [SerializeField] protected float _maxAttackMultiplier;
    [SerializeField] protected float _minAttackMultiplier;
    [SerializeField] protected float _maxBlockMultiplier;
    [SerializeField] protected float _minBlockMultiplier;
    [SerializeField] protected bool _isDead = false;
    [SerializeField] protected bool _isBlocking = false;
    protected BattleUnit _targetUnit;

    public bool IsDead()
    {
        return _isDead;
    }

    public void Ressurect()
    {
        _health = 100;
        _isDead = false;
    }

    public float GetHealth()
    {
        return _health;
    }

    public void SetTargetUnit(BattleUnit target)
    {
        _targetUnit = target;
    }

    public int AttackDamage()
    {
        int attackDamage = CalculateAttackDamage();

        Debug.Log(gameObject.name + " attacks with damage: " + attackDamage);

        return attackDamage;
    }

    public IEnumerator Attack()
    {
        int attackDamage = CalculateAttackDamage();

        Debug.Log(gameObject.name + " attacks with damage: " + attackDamage);

        yield return StartCoroutine(_targetUnit._TakeDamage(attackDamage));
    }

    public void TakeDamage(int damage)
    {
        float takenDamageMultiplier = 1f;

        if (damage < 0)
        {
            Debug.LogError("TakeDamage damage is negative number: " + damage + ". Taking absolute value.");
            damage = Mathf.Abs(damage);
        }

        _health -= damage * takenDamageMultiplier;

        Debug.Log(gameObject.name + " got damage: " + damage + ", health left: " + _health);

        if (_health <= 0)
        {
            Debug.Log(gameObject.name + " has died!");
            _isDead = true;
        }
    }

    public IEnumerator _TakeDamage(int damage)
    {
        float takenDamageMultiplier = 1f;

        if (damage < 0)
        {
            Debug.LogError("TakeDamage damage is negative number: " + damage + ". Taking absolute value.");
            damage = Mathf.Abs(damage);
        }

        if (IsBlocking())
        {
            yield return StartCoroutine(PlayBlockAnimation());
        }
        else
        {
            yield return StartCoroutine(PlayTakeDamageAnimation(damage));

            _health -= damage * takenDamageMultiplier;

            Debug.Log(gameObject.name + " got damage: " + damage + ", health left: " + _health);

            if (_health <= 0)
            {
                Debug.Log(gameObject.name + " has died!");
                // PlayDieAnimation();
                SetDead();
            }
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
        _isDead = true;
    }

    protected virtual IEnumerator PlayTakeDamageAnimation(int damage)
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

    protected virtual int CalculateAttackDamage()
    {
        int attackDamage = 0;
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
        return _isBlocking;
    }
}