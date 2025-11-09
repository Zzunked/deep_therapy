using UnityEngine;
using System.Collections;


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

    private ChosenTarget _targetPart;

    protected override int CalculateAttackDamage()
    {
        float attackMultiplier = Random.Range(_minAttackMultiplier, _maxAttackMultiplier);
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
        float blockMultiplier = Random.Range(_minBlockMultiplier, _maxBlockMultiplier);
        float blockedDamage = damage * blockMultiplier;

        return blockedDamage;
    }

    protected override IEnumerator PlayTakeDamageAnimation(int damage)
    {
        // play animation
        _damageDisplay.ShowNumber(damage);
        yield return StartCoroutine(PlayAnimation("Blast", _blastAnimator));
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
        float blockRate = Random.Range(0f, 1f);
        Debug.Log("Enemy blockRate: " + blockRate + ", blockProbability: " + _blockProbability);
        _isBlocking = (blockRate <= _blockProbability) ? true : false;

        return _isBlocking;
    }

    public bool DidBlock()
    {
        return _isBlocking;
    }
}