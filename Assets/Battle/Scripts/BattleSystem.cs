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
    [SerializeField] private Unit playerUnit;
    [SerializeField] private Unit enemyUnit;

    private BattleState battleState = BattleState.PlayersTurn;
    private bool isActionsEnabled = false;
    private bool isTargetsEnabled = false;

    // Update is called once per frame
    void Update()
    {
        if (playerUnit.isDead)
        {
            Debug.Log("GAME OVER");
            ResetBattle();
        }
        else if (enemyUnit.isDead)
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
                Debug.Log("Enemy attacks!");
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
        int enemyDamage = 10;

        if (actionBar.GetChousenAction() == ChousenAction.Block)
        {
            enemyDamage -= 5;
        }

        playerUnit.TakeDamage(enemyDamage);
    }

    void EndRound()
    {
        actionBar.Reset();
        battleState = BattleState.PlayersTurn;
    }

    void ResetBattle()
    {
        playerUnit.ResetHealth();
        enemyUnit.ResetHealth();
        actionBar.Reset();
        battleState = BattleState.PlayersTurn;
    }

    void HandlePlayerTurn()
    {
        switch (actionBar.GetChousenAction())
        {
            case ChousenAction.None:
                if (!isActionsEnabled)
                {
                    actionBar.EnableActionButtons();
                }
                break;

            case ChousenAction.Attack:
                HandlePlayerAttack();
                break;

            case ChousenAction.Block:
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
