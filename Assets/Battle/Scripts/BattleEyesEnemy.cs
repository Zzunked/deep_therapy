using UnityEngine;
using System.Collections;
using System;

public class BattleEyesEnemy : BattleUnit
{
    [SerializeField] private float _baseAttackDamage = 10f;
    [SerializeField] private float _headDamageMultiplier = 1f;
    [SerializeField] private float _bodyDamageMultiplier = 1f;
    [SerializeField] private float _eyesDamageMultiplier = 1f;
    [SerializeField] private ActionDisplayer _actionDisplayer;
    [SerializeField] private float _blinkingDuration = 3f;
    [SerializeField] private float _blinkingSpeed = 0.1f;
    private SpriteRenderer _renderer;

    private ChosenTarget _targetPart;


    private void Awake()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
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

    // protected override IEnumerator PlayTakeDamageAnimation(int damage)
    // {
    //     _actionDisplayer.Damage = damage;
    //     _actionDisplayer.Blink = Blink;
    //     yield return StartCoroutine(_actionDisplayer.ShowDamageOnEnemy());

    // }



    // protected override IEnumerator PlayBlockAnimation()
    // {
    //     // play animation
    //     // yield return StartCoroutine(PlayAnimation("Shield", _shieldAnimator));
    //     yield return StartCoroutine(_actionDisplayer.ShowShieldOnEnemy());
        
    //     // yield return new WaitForSeconds(0.1f);
    // }

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

    public void Blink()
    {
        StartCoroutine(BlinkAlpha());
    }

    public IEnumerator BlinkAlpha()
    {
        float elapsed = 0f;
        bool visible = true;

        while (elapsed < _blinkingDuration)
        {
            Color c = _renderer.color;
            c.a = visible ? 0f : 1f;
            _renderer.color = c;

            visible = !visible;

            yield return new WaitForSeconds(_blinkingSpeed);
            elapsed += _blinkingSpeed;
        }

        // restore alpha
        Color final = _renderer.color;
        final.a = 1f;
        _renderer.color = final;
    }

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
                Debug.LogError("Target part was not chosen! Setting damage multiplier to 1.");
                multiplier = 1f;
                break;
        }

        return multiplier;
    }

    public void TakeDamage(int damage, ChosenTarget target)
    {
        float takenDamageMultiplier;

        if (damage < 0)
        {
            Debug.LogError("TakeDamage damage is negative number: " + damage + ". Taking absolute value.");
            damage = Mathf.Abs(damage);
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