using UnityEngine;
using System.Collections;


enum BattleState
{
    PlayersTurn,
    EnemysTurn
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
        float delay = playerUnit.IsBlocking() ? delayBeforePlayerBlock : delayBeforeEnemyAttack;

        StartCoroutine(EnemyAttack(delay));
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
                    StartCoroutine(PlayerAttack(delayBeforePlayerAttack));
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

    IEnumerator PlayerAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerUnit.Attack();
    }

    IEnumerator EnemyAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        enemyUnit.Attack();
    }

}
