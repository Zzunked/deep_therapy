using UnityEngine;
using System.Collections;
using System.Threading.Tasks;


public class BattlePlayer : BattleUnit
{
    [SerializeField] private float _baseAttackDamage = 10f;
    [SerializeField] private float _runAwayProbability = 0.5f;
    [SerializeField] private ActionDisplayer _actionDisplayer;


    protected override int CalculateAttackDamage()
    {
        float attackMultiplier = Random.Range(_minAttackMultiplier, _maxAttackMultiplier);
        int attackDamage = (int)(_baseAttackDamage * attackMultiplier);

        return attackDamage;
    }

    protected override float CalculateTakenDamgeMultiplier()
    {
        float takenDamageMultiplier = 1f;

        return takenDamageMultiplier;

    }

    protected override float CalculateBlockedDamage(float damage)
    {
        float blockMultiplier = Random.Range(_minBlockMultiplier, _maxBlockMultiplier);
        float blockedDamage = damage * blockMultiplier;

        return blockedDamage;
    }

    // protected override IEnumerator PlayTakeDamageAnimation(int damage)
    // {
    //     // play animation
    //     _actionDisplayer.Damage = damage;
    //     yield return StartCoroutine(_actionDisplayer.ShowDamageOnPlayer());

    // }

    // private IEnumerator PlayTentacleAndCrack()
    // {
    //     // yield return StartCoroutine(PlayAnimation("Tentacle", _tentacleAnimator));

    //     // yield return StartCoroutine(PlayAnimation("Crack", _crackAnimator));
    // }

    // protected override IEnumerator PlayBlockAnimation()
    // {
    //     // play animation
    //     yield return StartCoroutine(_actionDisplayer.ShowShieldOnPlayer());
    // }

    public bool CanRunAway()
    {
        float runAwyRate = Random.Range(0f, 1f);
        bool canRunAway;

        Debug.Log("Players runAwayProbability: " + _runAwayProbability + ", runAwyRate: " + runAwyRate);
        canRunAway = (runAwyRate <= _runAwayProbability) ? true : false;

        return canRunAway;
    }
}