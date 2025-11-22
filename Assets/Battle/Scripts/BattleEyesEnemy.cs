using UnityEngine;
using System.Collections;
using System;

public class BattleEyesEnemy : BattleUnit
{
    [SerializeField] private float _headDamageMultiplier = 1f;
    [SerializeField] private float _bodyDamageMultiplier = 1f;
    [SerializeField] private float _eyesDamageMultiplier = 1f;

    private float GetTakenDamgeMultiplier(ChosenTarget target)
    {
        float multiplier;

        switch (target)
        {
            case ChosenTarget.Head:
                multiplier = _headDamageMultiplier;
                break;
            case ChosenTarget.Body:
                multiplier = _bodyDamageMultiplier;
                break;
            case ChosenTarget.Eyes:
                multiplier = _eyesDamageMultiplier;
                break;
            default:
                throw new ArgumentException("Unexpected target", nameof(target));
        }

        return multiplier;
    }

    public void TakeDamage(float damage, ChosenTarget target)
    {
        float takenDamageMultiplier;

        if (damage < 0)
        {
            throw new ArgumentException("Damage must be positive!");
        }

        takenDamageMultiplier = GetTakenDamgeMultiplier(target);

        _health -= damage * takenDamageMultiplier;

        if (_health <= 0)
        {
            Debug.Log(gameObject.name + " has died!");
            _isDead = true;
        }
    }
}