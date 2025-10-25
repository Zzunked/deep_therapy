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
    [SerializeField] private float delayBeforePlayerAttack = 1.5f;
    [SerializeField] private float delayBeforePlayerBlock = 1.5f;
    [SerializeField] private float delayBeforeEnemyAttack = 4f;

    private BattleState battleState = BattleState.PlayersTurn;
    private bool isActionsEnabled = false;
    private bool isTargetsEnabled = false;
    private int round = 1;
    private ChosenTarget target;
    private ChousenAction action;



    // Update is called once per frame
    // void Update()
    // {
    //     if (playerUnit.IsDead())
    //     {
    //         Debug.Log("GAME OVER");
    //         ResetBattle();
    //     }

    //     if (enemyUnit.IsDead())
    //     {
    //         Debug.Log("VICTORY");
    //         ResetBattle();
    //     }

    //     switch (battleState)
    //     {
    //         case BattleState.PlayersTurn:
    //             HandlePlayerTurn();
    //             break;

    //         case BattleState.EnemysTurn:
    //             HandleEnemyTurn();
    //             EndRound();
    //             break;

    //         default:
    //             Debug.LogError("Unexpected battle state!");
    //             break;
    //     }
    // }

    void Start()
    {
        playerUnit.SetTargetUnit(enemyUnit);
        enemyUnit.SetTargetUnit(playerUnit);
        StartCoroutine(SetupBattle());
    }

    void Update()
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
        Debug.Log("Players turn!");
    }

    void EndRound()
    {
        actionBar.Reset();
        battleState = BattleState.PlayersTurn;
        Debug.Log("------------------------- END OF ROUND " + round + " ----------------------------");
        Debug.Log("Players health: " + playerUnit.GetHealth());
        Debug.Log("Enemy health: " + enemyUnit.GetHealth());
        Debug.Log("-------------------------------------------------------------------");
        round++;
    }

    void ResetBattle()
    {
        playerUnit.Ressurect();
        enemyUnit.Ressurect();
        actionBar.Reset();
        PlayersTurn();
    }

    void CheckPlayersChoice()
    {
        switch (action)
        {
            case ChousenAction.Attack:
                if (target == ChosenTarget.None && !isTargetsEnabled)
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

    IEnumerator HandleAttackRound()
    {
        battleState = BattleState.EnemysTurn;
        enemyUnit.SetTargetPart(target);

        yield return StartCoroutine(playerUnit.MoveCardToCenter());

        yield return StartCoroutine(PlayerAttack());

        if (!enemyUnit.DidBlock())
        {
            yield return StartCoroutine(EnemyAttack());
        }

        yield return StartCoroutine(playerUnit.MoveCardToRight());

        NextRound();
    }

    IEnumerator HandleBlockRound()
    {
        battleState = BattleState.EnemysTurn;
        enemyUnit.SetTargetPart(target);

        yield return StartCoroutine(playerUnit.MoveCardToCenter());

        yield return StartCoroutine(EnemyAttack());

        yield return StartCoroutine(playerUnit.MoveCardToRight());

        playerUnit.ResetBlocking();

        NextRound();
    }

    IEnumerator HandleRunAwayRound()
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

            yield return StartCoroutine(playerUnit.MoveCardToRight());

            NextRound();
        }
    }

    private void NextRound()
    {
        actionBar.Reset();
        PlayersTurn();
    }

    IEnumerator PlayerAttack()
    {
        Debug.Log("Player started attack");
        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(playerUnit.Attack());
        Debug.Log("Player finished attack");
    }

    IEnumerator EnemyAttack()
    {
        Debug.Log("Enemy started attack");
        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(enemyUnit.Attack());
        Debug.Log("Enemy finished attack");
    }
}
