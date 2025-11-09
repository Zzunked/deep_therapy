using UnityEngine;
using System.Collections;


public class BattlePlayer : BattleUnit
{
    [SerializeField] private float _baseAttackDamage = 10f;
    [SerializeField] private float _runAwayProbability = 0.5f;
    [SerializeField] private Animator _tentacleAnimator;
    [SerializeField] private Animator _crackAnimator;
    [SerializeField] private Animator _shieldAnimator;
    private Vector2 _centerPosition;
    private Vector2 _rightCornerPosition;
    private Transform _cardTransform;
    private float _cardSpeed = 5f;
    


    private void Start()
    {
        _cardTransform = GetComponent<Transform>();
        _rightCornerPosition = _cardTransform.position;
        _centerPosition = new Vector2(0, _cardTransform.position.y);
    }

    protected override float CalculateAttackDamage()
    {
        float attackMultiplier = Random.Range(_minAttackMultiplier, _maxAttackMultiplier);
        float attackDamage = _baseAttackDamage * attackMultiplier;

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

    protected override IEnumerator PlayTakeDamageAnimation()
    {
        // play animation
        yield return StartCoroutine(PlayTentacleAndCrack());

    }

    private IEnumerator PlayTentacleAndCrack()
    {
        yield return StartCoroutine(PlayAnimation("Tentacle", _tentacleAnimator));

        yield return StartCoroutine(PlayAnimation("Crack", _crackAnimator));
    }

    protected override IEnumerator PlayBlockAnimation()
    {
        // play animation
        yield return StartCoroutine(PlayAnimation("Shield", _shieldAnimator));
    }

    public void SetBlocking()
    {
        _isBlocking = true;
    }

    public void ResetBlocking()
    {
        _isBlocking = false;
    }

    public IEnumerator MoveCardToCenter()
    {
        Debug.Log("Card is moving towads center");

        yield return StartCoroutine(MoveCard(_centerPosition));

        Debug.Log("Card is in the center");
    }

    public IEnumerator MoveCardToRight()
    {
        Debug.Log("Card is moving to right corner");

        yield return StartCoroutine(MoveCard(_rightCornerPosition));

        Debug.Log("Card is in the right corner");
    }

    private IEnumerator MoveCard(Vector2 targetPosition)
    {
        while (Vector2.Distance(_cardTransform.position, targetPosition) > 0.05f)
        {
            _cardTransform.position = Vector2.MoveTowards(_cardTransform.position, targetPosition, _cardSpeed * Time.deltaTime);

            yield return null;
        }

        // Snap to final position to avoid small offset
        _cardTransform.position = targetPosition;
    }

    public void SetDefaultPosition()
    {
        _cardTransform.position = _rightCornerPosition;
    }

    public bool CanRunAway()
    {
        float runAwyRate = Random.Range(0f, 1f);
        bool canRunAway;

        Debug.Log("Players runAwayProbability: " + _runAwayProbability + ", runAwyRate: " + runAwyRate);
        canRunAway = (runAwyRate <= _runAwayProbability) ? true : false;

        return canRunAway;
    }
}