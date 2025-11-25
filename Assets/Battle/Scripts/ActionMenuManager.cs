using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public enum PlayersAction
{
    None,
    Attack,
    Block,
    RunAway

}


public enum PlayersTarget
{
    None,
    Head,
    Body,
    Eyes
}


public enum ActionBarState
{
    ChooseAction,
    ChooseTarget,
    ActionChosen
}


public class ActionMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _actionBarPrefab;
    [SerializeField] private GameObject _targetBarPrefab;

    private GameObject _actionBarGO;
    private ActionBar _actionBar;
    private List<PlayersAction> _actions = new List<PlayersAction> {PlayersAction.Attack, PlayersAction.Block, PlayersAction.RunAway};
    private Vector3 _actionBarPos = new Vector3(-4.22394f, -3.51485f, 0);
    private GameObject _targetBarGO;
    private TargetBar _targetBar;
    private List<PlayersTarget> _targets = new List<PlayersTarget> {PlayersTarget.Head, PlayersTarget.Body, PlayersTarget.Eyes};
    private Vector3 _targetBarPos = new Vector3(-0.2239399f, -3.51485f, 0);

    private PlayersAction _chosenAction = PlayersAction.None;
    private PlayersTarget _chosenTarget = PlayersTarget.None;
    private ActionBarState _barState = ActionBarState.ChooseAction;
    
    public PlayersAction ChosenAction
    {
        get => _chosenAction;
        private set => _chosenAction = value;
    }

    public PlayersTarget ChosenTarget
    {
        get => _chosenTarget;
        private set => _chosenTarget = value;
    }

    private void Awake()
    {
        InstantiateActionsBar();
        _barState = ActionBarState.ChooseAction;
    }

    private void Update()
    {
        switch(_barState)
        {
            case ActionBarState.ChooseAction:
                HandleActionChoosing();
                break;

            case ActionBarState.ChooseTarget:
                HandleTargetChosing();
                break;
            
            case ActionBarState.ActionChosen:
                break;

            default:
                throw new Exception("Unexpected bar state");
        }
    }

    public void Reset()
    {
        _barState = ActionBarState.ChooseAction;
        _chosenAction = PlayersAction.None;
        _chosenTarget = PlayersTarget.None;
        InstantiateActionsBar();
    }

    private void HandleTargetChosing()
    {
        if(_targetBarGO == null)
            InstantiateTargetsBar();

        if(_targetBar.ClickedTarget != PlayersTarget.None)
        {
            _chosenTarget = _targetBar.ClickedTarget;
            _barState = ActionBarState.ActionChosen;
            DestroyTargetBar();
            DestroyActionBar();
        }
    }

    private void HandleActionChoosing()
    {
        if(_actionBarGO == null)
            InstantiateActionsBar();

        if(_actionBar.ClickedAction == PlayersAction.Attack)
        {
            _chosenAction = _actionBar.ClickedAction;
            _barState = ActionBarState.ChooseTarget;
            _actionBar.FreezeActions();
        }
        else if (_actionBar.ClickedAction == PlayersAction.Block || _actionBar.ClickedAction == PlayersAction.RunAway)
        {
            _chosenAction = _actionBar.ClickedAction;
            _barState = ActionBarState.ActionChosen;
            DestroyActionBar();
        }
    }

    private void InstantiateActionsBar()
    {
        _actionBarGO = Instantiate(_actionBarPrefab);
        _actionBarGO.transform.position = _actionBarPos;
        _actionBar = _actionBarGO.GetComponent<ActionBar>();
        _actionBar.Init(_actions);
    }

    private void DestroyActionBar()
    {
        Destroy(_actionBarGO);
    }

    private void InstantiateTargetsBar()
    {
        _targetBarGO = Instantiate(_targetBarPrefab);
        _targetBarGO.transform.position = _targetBarPos;
        _targetBar = _targetBarGO.GetComponent<TargetBar>();
        _targetBar.Init(_targets);
    }

    private void DestroyTargetBar()
    {
        Destroy(_targetBarGO);
    }
}