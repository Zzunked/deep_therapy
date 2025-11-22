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
    [SerializeField] private ActionBar _actionBar;
    [SerializeField] private BattlePlayer _playerUnit;
    [SerializeField] private BattleEyesEnemy _enemyUnit;
    [SerializeField] private ActionDisplayer _actionDisplayer;

    private BattleState _battleState = BattleState.PlayersTurn;
    private int _round = 1;
    private ChosenTarget _target;
    private ChousenAction _action;

    private void Start()
    {
        SetupBattle();
    }

    private void Update()
    {
        if (_battleState == BattleState.PlayersTurn)
        {
            _target = _actionBar.GetChosenTarget();
            _action = _actionBar.GetChousenAction();
            CheckPlayersChoice();
        }
    }

    void SetupBattle()
    {
        PlayersTurn();
        _actionBar.EnableActionButtons();
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
        _actionBar.Reset();
        PlayersTurn();
    }

    private void ResetBattle()
    {
        _round = 1;
        _playerUnit.Ressurect();
        _enemyUnit.Ressurect();
        _actionBar.Reset();
        _actionDisplayer.SetCardDefaultPosition();
        PlayersTurn();
    }

    private async void CheckPlayersChoice()
    {
        switch (_action)
        {
            case ChousenAction.Attack:
                if (_target == ChosenTarget.None && !_actionBar.IsTargetsEnabled())
                {
                    _actionBar.ShowTargets();
                    _actionBar.EnableTargetButtons();
                    _actionBar.DisableActionButtons();
                }

                if (_target != ChosenTarget.None)
                {
                    _actionBar.HideAndDisableAll();
                    await HandleAttackRound();
                }
                break;

            case ChousenAction.Block:
                _actionBar.HideAndDisableAll();
                await HandleBlockRound();

                break;

            case ChousenAction.RunAway:
                _actionBar.HideAndDisableAll();
                await HandleRunAwayRound();
                break;

            default:
                break;
        }
    }

    private async Task HandleAttackRound()
    {
        float playersDamage;

        _battleState = BattleState.EnemysTurn;

        await _actionDisplayer.MoveCardToCenter();

        if (!_enemyUnit.IsBlocking())
        {
            playersDamage = _playerUnit.AttackDamage();
            _enemyUnit.TakeDamage(playersDamage, _target);

            if (!_enemyUnit.IsDead)
            {
                _actionDisplayer.Damage = (int)playersDamage;
                await _actionDisplayer.ShowDamageOnEnemy(_target);
                await HandleDamageToPlayer();
            }
            else
            {
                await _actionDisplayer.ShowDamageOnEnemy(_target);
                await _actionDisplayer.ShowWinScreen();
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
    }
}
