using UnityEngine;
using System.Collections;


public class BattlePlayer : BattleUnit
{
    [SerializeField] private float baseAttackDamage = 10f;
    [SerializeField] private Animator tentacleAnimator;
    [SerializeField] private Animator crackAnimator;
    [SerializeField] private Animator shieldAnimator;

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

    protected override void PlayTakeDamageAnimation()
    {
        // play animation
        StartCoroutine(PlayTentacleAndCrack());

    }

    IEnumerator PlayTentacleAndCrack()
    {
        tentacleAnimator.Play("Tentacle");

        yield return new WaitForSeconds(0.5f);

        crackAnimator.Play("Crack");
    }

    protected override void PlayBlockAnimation()
    {
        // play animation
        StartCoroutine(PlayTentacleAndShield());
    }

    IEnumerator PlayTentacleAndShield()
    {
        tentacleAnimator.Play("Tentacle");

        yield return new WaitForSeconds(0.5f);

        shieldAnimator.Play("Shield");
    }

    public void SetBlocking()
    {
        isBlocking = true;
    }
}