using UnityEngine;


public enum BattleState
{
    PlayersTurn,
    EnemysTurn
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] GameObject actionBarObj;

    private ActionBar actionBar;
    private BattleState battleState = BattleState.PlayersTurn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actionBar = actionBarObj.GetComponent<ActionBar>();
    }

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
        switch (actionBar.chosenAction)
        {
            case ChousenAction.None:
                actionBar.EnableActionButtons();
                break;

            case ChousenAction.Attack:
                HandlePlayerAttack();
                break;

            case ChousenAction.Block:
                Debug.Log("Player is blocking!");
                battleState = BattleState.EnemysTurn;
                actionBar.chosenAction = ChousenAction.None;
                break;

            case ChousenAction.RunAway:
                Debug.Log("Player runs away!");
                battleState = BattleState.EnemysTurn;
                actionBar.chosenAction = ChousenAction.None;
                break;

            default:
                break;
        }
    }

    void HandlePlayerAttack()
    {
        switch (actionBar.chosenBodyPart)
        {
            case ChosenBodyPart.Head:
                Debug.Log("Player attacks enemy's head!");
                battleState = BattleState.EnemysTurn;
                ResetActionBar();
                break;

            case ChosenBodyPart.Body:
                Debug.Log("Player attacks enemy's body!");
                battleState = BattleState.EnemysTurn;
                ResetActionBar();
                break;

            case ChosenBodyPart.Eyes:
                Debug.Log("Player attacks enemy's eyes!");
                battleState = BattleState.EnemysTurn;
                ResetActionBar();
                break;

            default:
                break;
        }

    }

    void ResetActionBar()
    {
        actionBar.chosenBodyPart = ChosenBodyPart.None;
        actionBar.chosenAction = ChousenAction.None;
        actionBar.DisableTargetButtons();
        actionBar.HideTargetButtons();
        actionBar.HideActionButtons();
    }

}
