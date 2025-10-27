using UnityEngine;
using System.Collections;


public class BattlePlayer : BattleUnit
{
    [SerializeField] private float baseAttackDamage = 10f;
    [SerializeField] private float runAwayProbability = 0.5f;
    [SerializeField] private Animator tentacleAnimator;
    [SerializeField] private Animator crackAnimator;
    [SerializeField] private Animator shieldAnimator;
    private Vector2 centerPosition;
    private Vector2 rightCornerPosition;
    private Transform cardTransform;
    private float cardSpeed = 5f;
    


    private void Start()
    {
        cardTransform = GetComponent<Transform>();
        rightCornerPosition = cardTransform.position;
        centerPosition = new Vector2(0, cardTransform.position.y);
    }

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

    protected override IEnumerator PlayTakeDamageAnimation()
    {
        // play animation
        yield return StartCoroutine(PlayTentacleAndCrack());

    }

    private IEnumerator PlayTentacleAndCrack()
    {
        yield return StartCoroutine(PlayAnimation("Tentacle", tentacleAnimator));

        yield return StartCoroutine(PlayAnimation("Crack", crackAnimator));
    }

    protected override IEnumerator PlayBlockAnimation()
    {
        // play animation
        yield return StartCoroutine(PlayAnimation("Shield", shieldAnimator));
    }

    public void SetBlocking()
    {
        isBlocking = true;
    }

    public void ResetBlocking()
    {
        isBlocking = false;
    }

    public IEnumerator MoveCardToCenter()
    {
        Debug.Log("Card is moving towads center");

        yield return StartCoroutine(MoveCard(centerPosition));

        Debug.Log("Card is in the center");
    }

    public IEnumerator MoveCardToRight()
    {
        Debug.Log("Card is moving to right corner");

        yield return StartCoroutine(MoveCard(rightCornerPosition));

        Debug.Log("Card is in the right corner");
    }

    private IEnumerator MoveCard(Vector2 targetPosition)
    {
        while (Vector2.Distance(cardTransform.position, targetPosition) > 0.05f)
        {
            cardTransform.position = Vector2.MoveTowards(cardTransform.position, targetPosition, cardSpeed * Time.deltaTime);

            yield return null;
        }

        // Snap to final position to avoid small offset
        cardTransform.position = targetPosition;
    }

    public void SetDefaultPosition()
    {
        cardTransform.position = rightCornerPosition;
    }

    public bool CanRunAway()
    {
        float runAwyRate = Random.Range(0f, 1f);
        bool canRunAway;

        Debug.Log("Players runAwayProbability: " + runAwayProbability + ", runAwyRate: " + runAwyRate);
        canRunAway = (runAwyRate <= runAwayProbability) ? true : false;

        return canRunAway;
    }
}