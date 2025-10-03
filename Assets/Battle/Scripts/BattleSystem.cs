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

    private BattleState battleState = BattleState.PlayersTurn;

    private bool isActionsEnabled = false;
    private bool isTargetsEnabled = false;

    // Update is called once per frame
    void Update()
    {
        switch (battleState)
        {
            case BattleState.PlayersTurn:
                HandlePlayerTurn();
                break;

            case BattleState.EnemysTurn:
                Debug.Log("Enemy attacks!");
                battleState = BattleState.PlayersTurn;
                break;

            default:
                Debug.LogError("Unexpected battle state!");
                break;
        }
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
                actionBar.ResetChosenAction();
                break;

            case ChousenAction.RunAway:
                Debug.Log("Player runs away!");
                battleState = BattleState.EnemysTurn;
                actionBar.ResetChosenAction();
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
                battleState = BattleState.EnemysTurn;
                actionBar.Reset();
                break;

            case ChosenTarget.Body:
                Debug.Log("Player attacks enemy's body!");
                battleState = BattleState.EnemysTurn;
                actionBar.Reset();
                break;

            case ChosenTarget.Eyes:
                Debug.Log("Player attacks enemy's eyes!");
                battleState = BattleState.EnemysTurn;
                actionBar.Reset();
                break;

            default:
                Debug.LogError("Unexpected target!");
                break;
        }
    }
}
