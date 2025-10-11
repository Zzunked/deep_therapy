using System.Data;
using UnityEngine;


public enum BattleState
{
    PlayersTurn,
    EnemysTurn
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private ActionBar actionBar;
    [SerializeField] private BattlePlayer playerUnit;
    [SerializeField] private BattleEyesEnemy enemyUnit;

    private BattleState battleState = BattleState.PlayersTurn;
    private bool isActionsEnabled = false;
    private bool isTargetsEnabled = false;

    void Start()
    {
        playerUnit.SetTargetUnit(enemyUnit);
        enemyUnit.SetTargetUnit(playerUnit);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerUnit.IsDead())
        {
            Debug.Log("GAME OVER");
            ResetBattle();
        }

        if (enemyUnit.IsDead())
        {
            Debug.Log("VICTORY");
            ResetBattle();
        }

        switch (battleState)
        {
            case BattleState.PlayersTurn:
                HandlePlayerTurn();
                break;

            case BattleState.EnemysTurn:
                HandleEnemyTurn();
                EndRound();
                break;

            default:
                Debug.LogError("Unexpected battle state!");
                break;
        }
    }

    void HandleEnemyTurn()
    {
        enemyUnit.Attack();
    }

    void EndRound()
    {
        actionBar.Reset();
        battleState = BattleState.PlayersTurn;
        Debug.Log("------------------------- END OF ROUND ----------------------------");
        Debug.Log("Players health: " + playerUnit.GetHealth());
        Debug.Log("Enemy health: " + enemyUnit.GetHealth());
        Debug.Log("-------------------------------------------------------------------");
    }

    void ResetBattle()
    {
        playerUnit.Ressurect();
        enemyUnit.Ressurect();
        actionBar.Reset();
        battleState = BattleState.PlayersTurn;
    }

    void HandlePlayerTurn()
    {
        ChosenTarget target = actionBar.GetChosenTarget();
        ChousenAction action = actionBar.GetChousenAction();

        switch (action)
        {
            case ChousenAction.None:
                if (!isActionsEnabled)
                {
                    actionBar.EnableActionButtons();
                }
                break;

            case ChousenAction.Attack:
                if (target == ChosenTarget.None && !isTargetsEnabled)
                {
                    actionBar.EnableTargetButtons();
                }

                if (target != ChosenTarget.None)
                {
                    enemyUnit.SetTargetPart(target);
                    playerUnit.Attack();
                    enemyUnit.ResetTargetPart();
                    battleState = BattleState.EnemysTurn;
                }
                break;

            case ChousenAction.Block:
                playerUnit.SetBlocking();
                Debug.Log("Player is blocking!");
                battleState = BattleState.EnemysTurn;
                break;

            case ChousenAction.RunAway:
                Debug.Log("Player runs away!");
                battleState = BattleState.EnemysTurn;
                break;

            default:
                Debug.LogError("Unexpected player action!");
                break;
        }
    }

    void HandlePlayerAttack()
    {
        switch (actionBar.GetChosenTarget())
        {
            case ChosenTarget.None:
                if (actionBar.GetChousenAction() == ChousenAction.Attack && !isTargetsEnabled)
                {
                    actionBar.EnableTargetButtons();
                }
                break;

            case ChosenTarget.Head:
                Debug.Log("Player attacks enemy's head!");
                enemyUnit.TakeDamage(10);
                battleState = BattleState.EnemysTurn;
                break;

            case ChosenTarget.Body:
                Debug.Log("Player attacks enemy's body!");
                enemyUnit.TakeDamage(10);
                battleState = BattleState.EnemysTurn;
                break;

            case ChosenTarget.Eyes:
                Debug.Log("Player attacks enemy's eyes!");
                enemyUnit.TakeDamage(10);
                battleState = BattleState.EnemysTurn;
                break;

            default:
                Debug.LogError("Unexpected target!");
                break;
        }
    }
}
