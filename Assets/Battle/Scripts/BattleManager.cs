using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System;


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
    // [SerializeField] private Animator _deadPlayerScreenAnimator;
    // [SerializeField] private SpriteRenderer _deadPlayerSpriteRenderer;
    [SerializeField] private ActionDisplayer _actionDisplayer;

    private BattleState _battleState = BattleState.PlayersTurn;
    private int _round = 1;
    private ChosenTarget _target;
    private ChousenAction _action;

    private void Start()
    {
        _playerUnit.SetTargetUnit(_enemyUnit);
        _enemyUnit.SetTargetUnit(_playerUnit);
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
        Debug.Log("Players health: " + _playerUnit.GetHealth());
        Debug.Log("Enemy health: " + _enemyUnit.GetHealth());
        Debug.Log("-------------------------------------------------------------------");

        _round++;
        _actionBar.Reset();
        // StopAllCoroutines();
        PlayersTurn();
    }

    private void ResetBattle()
    {
        _round = 1;
        _playerUnit.Ressurect();
        _enemyUnit.Ressurect();
        _actionBar.Reset();
        _playerUnit.SetDefaultPosition();
        StopAllCoroutines();
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
                    print("Handled");
                }
                break;

            case ChousenAction.Block:
                _playerUnit.SetBlocking();
                _actionBar.HideAndDisableAll();

                Debug.Log("Player is blocking!");

                StartCoroutine(HandleBlockRound());

                break;

            case ChousenAction.RunAway:
                _actionBar.HideAndDisableAll();
                StartCoroutine(HandleRunAwayRound());
                break;

            default:
                break;
        }
    }

    private async Task HandleAttackRound()
    {
        int playersDamage;
        int enemyDamage;

        _battleState = BattleState.EnemysTurn;
        _enemyUnit.SetTargetPart(_target);

        await _playerUnit.MoveCardToCenter();

        if (_enemyUnit.IsBlocking())
        {
            await _actionDisplayer.ShowShieldOnEnemy();
            await _playerUnit.MoveCardToRight();
            EndRound();
        }
        else
        {
            playersDamage = _playerUnit.AttackDamage();
            _enemyUnit.TakeDamage(playersDamage, _target);

            if (!_enemyUnit.IsDead())
            {
                _actionDisplayer.Damage = playersDamage;
                await _actionDisplayer.ShowDamageOnEnemy();

                enemyDamage = _enemyUnit.AttackDamage();
                _playerUnit.TakeDamage(enemyDamage);

                if(!_playerUnit.IsDead())
                {
                    _actionDisplayer.Damage = enemyDamage;
                    await _actionDisplayer.ShowDamageOnPlayer();
                    await _playerUnit.MoveCardToRight();
                    EndRound();
                }
                else
                {
                    await _actionDisplayer.ShowDamageOnPlayer();
                    await _actionDisplayer.ShowPlayerDeadScreen();
                    ResetBattle();
                }
            }
            else
            {
                await _actionDisplayer.ShowDamageOnEnemy();
                await _actionDisplayer.ShowWinScreen();
                _battleState = BattleState.Win;
                ResetBattle();
            }
        }
        
    }

    private IEnumerator _HandleAttackRound()
    {
        _battleState = BattleState.EnemysTurn;
        _enemyUnit.SetTargetPart(_target);

        // yield return StartCoroutine(_playerUnit.MoveCardToCenter());

        yield return StartCoroutine(PlayerAttack());

        if (_enemyUnit.IsDead())
        {
            Debug.Log("Enemy is dead");
            ResetBattle();
        }
        else
        {
            if (!_enemyUnit.DidBlock())
            {
                yield return StartCoroutine(EnemyAttack());

                if (_playerUnit.IsDead())
                {
                    Debug.Log("Player is dead!");

                    // yield return StartCoroutine(ShowPlayerDeadScreen());

                    ResetBattle();
                }
                else
                {
                    // yield return StartCoroutine(_playerUnit.MoveCardToRight());
                    EndRound();
                }
            }
            else
            {
                // yield return StartCoroutine(_playerUnit.MoveCardToRight());
                EndRound();
            }
        }
    }

    private IEnumerator HandleBlockRound()
    {
        _battleState = BattleState.EnemysTurn;
        _enemyUnit.SetTargetPart(_target);

        // yield return StartCoroutine(_playerUnit.MoveCardToCenter());

        yield return StartCoroutine(EnemyAttack());

        // yield return StartCoroutine(_playerUnit.MoveCardToRight());

        _playerUnit.ResetBlocking();

        EndRound();
    }

    private IEnumerator HandleRunAwayRound()
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
            _enemyUnit.SetTargetPart(_target);

            // yield return StartCoroutine(_playerUnit.MoveCardToCenter());

            yield return StartCoroutine(EnemyAttack());

            if (_playerUnit.IsDead())
            {
                Debug.Log("Player is dead!");
                ResetBattle();
            }
            else
            {
                // yield return StartCoroutine(_playerUnit.MoveCardToRight());
                EndRound();
            }
        }
    }

    private IEnumerator PlayerAttack()
    {
        Debug.Log("Player started attack");

        yield return StartCoroutine(_playerUnit.Attack());

        Debug.Log("Player finished attack");
    }

    private IEnumerator EnemyAttack()
    {
        Debug.Log("Enemy started attack");
        yield return new WaitForSeconds(0.8f);

        yield return StartCoroutine(_enemyUnit.Attack());
        Debug.Log("Enemy finished attack");
    }

    // private IEnumerator BackgroundFadeIn()
    // {
    //     float fadeDuration = 2f;
    //     float elapsed = 0f;
    //     Color startColor = Color.black;
    //     Color endColor = _deadPlayerSpriteRenderer.color; // original sprite color


    //     // Temporarily set the sprite color to black
    //     _deadPlayerSpriteRenderer.color = startColor;

    //     while (elapsed < fadeDuration)
    //     {
    //         elapsed += Time.deltaTime;
    //         float t = Mathf.Clamp01(elapsed / fadeDuration);
    //         _deadPlayerSpriteRenderer.color = Color.Lerp(startColor, endColor, t);
    //         yield return null;
    //     }

    //     // Ensure exact final color
    //     _deadPlayerSpriteRenderer.color = endColor;
    // }

    // private IEnumerator ShowPlayerDeadScreen()
    // {
    //     _deadPlayerSpriteRenderer.sortingOrder = 4;

    //     yield return StartCoroutine(BackgroundFadeIn());

    //     _deadPlayerScreenAnimator.Play("YouDead");

    //     // Wait until the animation starts
    //     yield return null;

    //     // Get info about the current animation
    //     AnimatorStateInfo info = _deadPlayerScreenAnimator.GetCurrentAnimatorStateInfo(0);

    //     Debug.Log("Playing " + _deadPlayerScreenAnimator + " animation for " + info.length + "s");

    //     // Wait for the duration of the animation
    //     yield return new WaitForSeconds(info.length);

    //     yield return new WaitForSeconds(5);
    //     _deadPlayerScreenAnimator.Play("YouDeadIdle");
    //     _deadPlayerSpriteRenderer.sortingOrder = -1;
    // }
}
