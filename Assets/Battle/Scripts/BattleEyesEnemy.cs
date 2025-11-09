using UnityEngine;
using System.Collections;
using System;

public class BattleEyesEnemy : BattleUnit
{
    [SerializeField] private float _baseAttackDamage = 10f;
    [SerializeField] private float _blockProbability = 0.5f;
    [SerializeField] private float _headDamageMultiplier = 1f;
    [SerializeField] private float _bodyDamageMultiplier = 1f;
    [SerializeField] private float _eyesDamageMultiplier = 1f;
    [SerializeField] private Animator _blastAnimator;
    [SerializeField] private Animator _shieldAnimator;
    [SerializeField] private DamageNumberDisplay _damageDisplay;
    [SerializeField] private Blast _blast;

    public event Action OnBlastDamagePhase;
    private ChosenTarget _targetPart;


    public void BlastDamagePhase()
    {
        OnBlastDamagePhase?.Invoke();
    }

    protected override int CalculateAttackDamage()
    {
        float attackMultiplier = UnityEngine.Random.Range(_minAttackMultiplier, _maxAttackMultiplier);
        int attackDamage = (int)(_baseAttackDamage * attackMultiplier);

        return attackDamage;
    }

    protected override float CalculateTakenDamgeMultiplier()
    {
        float multiplier;

        switch (_targetPart)
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
                Debug.LogError("Target part was not chosen! Setting damage multiplier to 1.");
                multiplier = 1f;
                break;
        }

        return multiplier;
    }

    protected override float CalculateBlockedDamage(float damage)
    {
        float blockMultiplier = UnityEngine.Random.Range(_minBlockMultiplier, _maxBlockMultiplier);
        float blockedDamage = damage * blockMultiplier;

        return blockedDamage;
    }

    protected override IEnumerator PlayTakeDamageAnimation(int damage)
    {
        _blast.Damage = damage;
        _blast.BlastDamagePhase += _damageDisplay.ShowNumber;

        yield return StartCoroutine(PlayAnimation("Blast", _blastAnimator));

        _blast.BlastDamagePhase -= _damageDisplay.ShowNumber;
    }

    protected override IEnumerator PlayBlockAnimation()
    {
        // play animation
        yield return StartCoroutine(PlayAnimation("Shield", _shieldAnimator));
    }

    protected override void PlayDieAnimation()
    {
        // play animation
    }

    protected override void PlayWinAnimation()
    {
        // play animation
    }

    public void SetTargetPart(ChosenTarget chosenTarget)
    {
        _targetPart = chosenTarget;
    }

    public void ResetTargetPart()
    {
        _targetPart = ChosenTarget.None;
    }

    public override bool IsBlocking()
    {
        float blockRate = UnityEngine.Random.Range(0f, 1f);
        Debug.Log("Enemy blockRate: " + blockRate + ", blockProbability: " + _blockProbability);
        _isBlocking = (blockRate <= _blockProbability) ? true : false;

        return _isBlocking;
    }

    public bool DidBlock()
    {
        return _isBlocking;
    }
}