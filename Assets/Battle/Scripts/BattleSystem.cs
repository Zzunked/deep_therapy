using UnityEngine;
using System.Collections;


enum BattleState
{
    Start,
    PlayersTurn,
    EnemysTurn,
    Win,
    Defeat
}


public class BattleSystem : MonoBehaviour
{
    [SerializeField] private ActionBar actionBar;
    [SerializeField] private BattlePlayer playerUnit;
    [SerializeField] private BattleEyesEnemy enemyUnit;
    [SerializeField] private Animator deadPlayerScreenAnimator;
    [SerializeField] private SpriteRenderer deadPlayerSpriteRenderer;

    private BattleState battleState = BattleState.PlayersTurn;
    private int round = 1;
    private ChosenTarget target;
    private ChousenAction action;

    private void Start()
    {
        playerUnit.SetTargetUnit(enemyUnit);
        enemyUnit.SetTargetUnit(playerUnit);
        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        if (battleState == BattleState.PlayersTurn)
        {
            target = actionBar.GetChosenTarget();
            action = actionBar.GetChousenAction();
            CheckPlayersChoice();
        }
    }

    IEnumerator SetupBattle()
    {
        yield return null;
        PlayersTurn();
        actionBar.EnableActionButtons();
    }

    private void PlayersTurn()
    {
        battleState = BattleState.PlayersTurn;
        Debug.Log("------------------------- ROUND " + round + " ----------------------------");
        Debug.Log("Players turn!");
    }

    private void EndRound()
    {
        Debug.Log("------------------------- END OF ROUND " + round + " ----------------------------");
        Debug.Log("Players health: " + playerUnit.GetHealth());
        Debug.Log("Enemy health: " + enemyUnit.GetHealth());
        Debug.Log("-------------------------------------------------------------------");

        round++;
        actionBar.Reset();
        StopAllCoroutines();
        PlayersTurn();
    }

    private void ResetBattle()
    {
        round = 1;
        playerUnit.Ressurect();
        enemyUnit.Ressurect();
        actionBar.Reset();
        playerUnit.SetDefaultPosition();
        StopAllCoroutines();
        PlayersTurn();
    }

    private void CheckPlayersChoice()
    {
        switch (action)
        {
            case ChousenAction.Attack:
                if (target == ChosenTarget.None && !actionBar.IsTargetsEnabled())
                {
                    actionBar.ShowTargets();
                    actionBar.EnableTargetButtons();
                    actionBar.DisableActionButtons();
                }

                if (target != ChosenTarget.None)
                {
                    actionBar.HideAndDisableAll();
                    StartCoroutine(HandleAttackRound());
                }
                break;

            case ChousenAction.Block:
                playerUnit.SetBlocking();
                actionBar.HideAndDisableAll();

                Debug.Log("Player is blocking!");

                StartCoroutine(HandleBlockRound());

                break;

            case ChousenAction.RunAway:
                actionBar.HideAndDisableAll();
                StartCoroutine(HandleRunAwayRound());
                break;

            default:
                break;
        }
    }

    private IEnumerator HandleAttackRound()
    {
        battleState = BattleState.EnemysTurn;
        enemyUnit.SetTargetPart(target);

        yield return StartCoroutine(playerUnit.MoveCardToCenter());

        yield return StartCoroutine(PlayerAttack());

        if (enemyUnit.IsDead())
        {
            Debug.Log("Enemy is dead");
            ResetBattle();
        }
        else
        {
            if (!enemyUnit.DidBlock())
            {
                yield return StartCoroutine(EnemyAttack());

                if (playerUnit.IsDead())
                {
                    Debug.Log("Player is dead!");

                    yield return StartCoroutine(ShowPlayerDeadScreen());

                    ResetBattle();
                }
                else
                {
                    yield return StartCoroutine(playerUnit.MoveCardToRight());
                    EndRound();
                }
            }
            else
            {
                yield return StartCoroutine(playerUnit.MoveCardToRight());
                EndRound();
            }
        }
    }

    private IEnumerator HandleBlockRound()
    {
        battleState = BattleState.EnemysTurn;
        enemyUnit.SetTargetPart(target);

        yield return StartCoroutine(playerUnit.MoveCardToCenter());

        yield return StartCoroutine(EnemyAttack());

        yield return StartCoroutine(playerUnit.MoveCardToRight());

        playerUnit.ResetBlocking();

        EndRound();
    }

    private IEnumerator HandleRunAwayRound()
    {
        if (playerUnit.CanRunAway())
        {
            Debug.Log("Player ran away! Resetting the battle.");
            ResetBattle();
        }
        else
        {
            Debug.Log("Player failed to ran away! Now Enemy attacks!");

            battleState = BattleState.EnemysTurn;
            enemyUnit.SetTargetPart(target);

            yield return StartCoroutine(playerUnit.MoveCardToCenter());

            yield return StartCoroutine(EnemyAttack());

            if (playerUnit.IsDead())
            {
                Debug.Log("Player is dead!");
                ResetBattle();
            }
            else
            {
                yield return StartCoroutine(playerUnit.MoveCardToRight());
                EndRound();
            }
        }
    }

    private IEnumerator PlayerAttack()
    {
        Debug.Log("Player started attack");
        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(playerUnit.Attack());

        Debug.Log("Player finished attack");
    }

    private IEnumerator EnemyAttack()
    {
        Debug.Log("Enemy started attack");
        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(enemyUnit.Attack());
        Debug.Log("Enemy finished attack");
    }

    private IEnumerator BackgroundFadeIn()
    {
        float fadeDuration = 2f;
        float elapsed = 0f;
        Color startColor = Color.black;
        Color endColor = deadPlayerSpriteRenderer.color; // original sprite color


        // Temporarily set the sprite color to black
        deadPlayerSpriteRenderer.color = startColor;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            deadPlayerSpriteRenderer.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        // Ensure exact final color
        deadPlayerSpriteRenderer.color = endColor;
    }

    private IEnumerator ShowPlayerDeadScreen()
    {
        deadPlayerSpriteRenderer.sortingOrder = 4;

        yield return StartCoroutine(BackgroundFadeIn());

        deadPlayerScreenAnimator.Play("YouDead");

        // Wait until the animation starts
        yield return null;

        // Get info about the current animation
        AnimatorStateInfo info = deadPlayerScreenAnimator.GetCurrentAnimatorStateInfo(0);

        Debug.Log("Playing " + deadPlayerScreenAnimator + " animation for " + info.length + "s");

        // Wait for the duration of the animation
        yield return new WaitForSeconds(info.length);

        yield return new WaitForSeconds(5);
        deadPlayerScreenAnimator.Play("YouDeadIdle");
        deadPlayerSpriteRenderer.sortingOrder = -1;
    }
}
