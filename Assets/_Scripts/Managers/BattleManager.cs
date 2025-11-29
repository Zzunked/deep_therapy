using UnityEngine;
using System.Threading.Tasks;


enum BattleState
{
    Start,
    PlayersTurn,
    EnemysTurn,
    Win,
    Defeat
}


public class BattleManager : MonoBehaviour
{
    [SerializeField] private ActionMenuManager _actionMenu;
    [SerializeField] private BattlePlayer _playerUnit;
    [SerializeField] private BattleEyesEnemy _enemyUnit;
    [SerializeField] private ActionDisplayer _actionDisplayer;

    private BattleState _battleState;
    private int _round = 1;

    private void Awake()
    {
        PlayersTurn();
    }

    private void Update()
    {
        if (_battleState == BattleState.PlayersTurn)
        {
            CheckPlayersChoice();
        }
    }

    private void PlayersTurn()
    {
        _battleState = BattleState.PlayersTurn;
        Debug.Log("------------------------- ROUND " + _round + " ----------------------------");
        Debug.Log("Players turn!");
    }

    private void EndRound()
    {
        Debug.Log("------------------------- END OF ROUND " + _round + " ----------------------------");
        Debug.Log("Players health: " + _playerUnit.Health);
        Debug.Log("Enemy health: " + _enemyUnit.Health);
        Debug.Log("-------------------------------------------------------------------");

        _round++;
        _actionMenu.Reset();
        PlayersTurn();
    }

    private void ResetBattle()
    {
        _round = 1;
        _playerUnit.Ressurect();
        _playerUnit.ResetHearts();
        _enemyUnit.Ressurect();
        _actionMenu.Reset();
        _actionDisplayer.Reset();
        PlayersTurn();
    }

    private async void CheckPlayersChoice()
    {
        PlayersAction action = _actionMenu.ChosenAction;

        switch (action)
        {
            case PlayersAction.Attack:
                PlayersTarget target = _actionMenu.ChosenTarget;

                if (target != PlayersTarget.None)
                {
                    await HandleAttackRound(target);
                }
                break;

            case PlayersAction.Block:
                await HandleBlockRound();
                break;

            case PlayersAction.RunAway:
                await HandleRunAwayRound();
                break;

            default:
                break;
        }
    }

    private async Task HandleAttackRound(PlayersTarget target)
    {
        float playersDamage;

        _battleState = BattleState.EnemysTurn;

        await _actionDisplayer.MoveCardToCenter();

        if (!_enemyUnit.IsBlocking())
        {
            playersDamage = _playerUnit.AttackDamage();
            _enemyUnit.TakeDamage(playersDamage, target);
            _actionDisplayer.Damage = (int)playersDamage;

            if (!_enemyUnit.IsDead)
            {
                await _actionDisplayer.ShowDamageOnEnemy(target, false);
                await HandleDamageToPlayer();
            }
            else
            {
                await _actionDisplayer.ShowDamageOnEnemy(target, true);
                await _actionDisplayer.MoveCardToRight();
                await _actionDisplayer.ShowVictory();
                _battleState = BattleState.Win;
                ResetBattle();
            }
        }
        else
        {
            await _actionDisplayer.ShowShieldOnEnemy();
            await _actionDisplayer.MoveCardToRight();
            EndRound();
        }
    }

    private async Task HandleBlockRound()
    {
        _battleState = BattleState.EnemysTurn;

        await _actionDisplayer.MoveCardToCenter();

        if(_playerUnit.IsBlocking())
        {
            await _actionDisplayer.ShowShieldOnPlayer();
            await _actionDisplayer.MoveCardToRight();
            EndRound();
        }
        else
        {
            await HandleDamageToPlayer();
        }
    }
    private async Task HandleRunAwayRound()
    {
        if (_playerUnit.CanRunAway())
        {
            Debug.Log("Player ran away! Resetting the battle.");
            ResetBattle();
        }
        else
        {
            Debug.Log("Player failed to ran away! Now Enemy attacks!");
            _battleState = BattleState.EnemysTurn;
            await _actionDisplayer.MoveCardToCenter();
            await HandleDamageToPlayer();
        }
    }

    private async Task HandleDamageToPlayer()
    {
        float enemyDamage = _enemyUnit.AttackDamage();
        _playerUnit.TakeDamage(enemyDamage);
        _actionDisplayer.Damage = (int)enemyDamage;
        _actionDisplayer.OnCrackFinished += _playerUnit.UpdateHearts;

        if(!_playerUnit.IsDead)
        {
            await _actionDisplayer.ShowDamageOnPlayer();
            await _actionDisplayer.MoveCardToRight();
            EndRound();
        }
        else
        {
            await _actionDisplayer.ShowDamageOnPlayer();
            await _actionDisplayer.ShowPlayerDeadScreen();
            _battleState = BattleState.Defeat;
            ResetBattle();
        }

        _actionDisplayer.OnCrackFinished -= _playerUnit.UpdateHearts;
    }
}
