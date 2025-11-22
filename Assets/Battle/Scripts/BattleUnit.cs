using System;
using UnityEngine;


// Base class for battle unit
public class BattleUnit : MonoBehaviour
{
    [SerializeField] protected float _health = 10000;
    [SerializeField] private float _baseAttackDamage = 1000f;
    [SerializeField] protected float _maxAttackMultiplier;
    [SerializeField] protected float _minAttackMultiplier;
    [SerializeField] protected bool _isDead = false;
    [SerializeField] private float _blockProbability = 0.5f;

    public bool IsDead
    {
        get => _isDead;
        private set => _isDead = value;
    }

    public float Health
    {
        get => _health;
        private set => _health = value;
        
    }

    public void Ressurect()
    {
        Health = 100;
        IsDead = false;
    }

    protected float CalculateAttackDamage()
    {
        float attackMultiplier = UnityEngine.Random.Range(_minAttackMultiplier, _maxAttackMultiplier);

        return _baseAttackDamage * attackMultiplier;
    }

    public float AttackDamage()
    {
        float attackDamage = CalculateAttackDamage();

        Debug.Log(gameObject.name + " attacks with damage: " + attackDamage);

        return attackDamage;
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            throw new ArgumentException("Damage must be positive!");
        }

        Health -= damage;

        Debug.Log(gameObject.name + " got damage: " + damage + ", health left: " + Health);

        if (Health <= 0)
        {
            Debug.Log(gameObject.name + " has died!");
            IsDead = true;
        }
    }

    public bool IsBlocking()
    {
        float blockRate = UnityEngine.Random.Range(0f, 1f);
        Debug.Log(gameObject.name + " blockRate: " + blockRate + ", blockProbability: " + _blockProbability);

        return (blockRate <= _blockProbability) ? true : false;;
    }
}