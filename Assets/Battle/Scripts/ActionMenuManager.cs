using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public enum ChousenAction
{
    None,
    Attack,
    Block,
    RunAway

}


public enum ChosenTarget
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
    [SerializeField] private GameObject _actionButtonPrefab;
    [SerializeField] private GameObject _targetButtonPrefab;
    [SerializeField] private GameObject _actionsPrefab;
    [SerializeField] private GameObject _targetsPrefab;

    private Vector3 _actionsBarPos = new Vector3(-4.22394f, -3.51485f, 0);
    private Vector3 _attackBtnPos = new Vector3(-4.22394f, -2.85485f, 0);
    private Vector3 _blockBtnPos = new Vector3(-4.22394f, -3.51485f, 0);
    private Vector3 _runAwayBtnPos = new Vector3(-4.22394f, -4.21485f, 0);
    private Vector3 _targetsBarPos = new Vector3(-0.2239399f, -3.51485f, 0);
    private Vector3 _headBtnPos = new Vector3(-0.2239399f, -2.76485f, 0);
    private Vector3 _bodyBtnPos = new Vector3(-0.2239399f, -3.41485f, 0);
    private Vector3 _eyesBtnPos = new Vector3(-0.2239399f, -4.11485f, 0);

    // Action bar
    private GameObject _actionsBarGO;
    private List<ActionButton> _actionButtons = new List<ActionButton> {};
    private List<GameObject> _actionButtonGOs = new List<GameObject> {};
    private ActionButton _attackBtn;
    private ActionButton _blockBtn;
    private ActionButton _runAwayBtn;

    // Targets
    private GameObject _targetsBarGO;
    private List<TargetButton> _targetButtons = new List<TargetButton> {};
    private List<GameObject> _targetButtonGOs = new List<GameObject> {};
    private TargetButton _headBtn;
    private TargetButton _bodyBtn;
    private TargetButton _eyesBtn;

    private ChousenAction _chosenAction = ChousenAction.None;
    private ChosenTarget _chosenTarget = ChosenTarget.None;
    private ActionBarState _barState = ActionBarState.ChooseAction;
    
    public ChousenAction ChosenAction
    {
        get => _chosenAction;
        private set => _chosenAction = value;
    }

    public ChosenTarget ChosenTarget
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
        _chosenAction = ChousenAction.None;
        _chosenTarget = ChosenTarget.None;
        InstantiateActionsBar();
    }

    private void HandleTargetChosing()
    {
        if(_targetsBarGO == null)
            InstantiateTargetsBar();
        
        foreach(TargetButton btn in _targetButtons)
        {
            if(btn.IsClicked)
            {
                _chosenTarget = btn.Target;
            }
        }

        if(_chosenTarget != ChosenTarget.None)
        {
            _barState = ActionBarState.ActionChosen;
            DestroyTargets();
            DestroyActions();
        }
    }

    private void HandleActionChoosing()
    {
        if(_actionsBarGO == null)
            InstantiateActionsBar();

        foreach(ActionButton btn in _actionButtons)
        {
            if(btn.IsClicked)
            {
                Debug.Log("Clicked btn " + nameof(btn.Action));
                _chosenAction = btn.Action;
                Debug.Log($"Chosen action: {_chosenAction}");
                break;
            }
        }

        if(_chosenAction == ChousenAction.Attack)
        {
            _barState = ActionBarState.ChooseTarget;
            FreezeActions();
        }
        else if (_chosenAction == ChousenAction.Block || _chosenAction == ChousenAction.RunAway)
        {
            _barState = ActionBarState.ActionChosen;
            DestroyActions();
        }
    }

    private void DestroyActions()
    {
        Destroy(_actionsBarGO);

        foreach(GameObject btnGO in _actionButtonGOs)
            Destroy(btnGO);

        _actionButtonGOs.Clear();
        _actionButtons.Clear();
    }

    private void DestroyTargets()
    {
        Destroy(_targetsBarGO);

        foreach(GameObject btnGO in _targetButtonGOs)
            Destroy(btnGO);
        
        _targetButtonGOs.Clear();
        _targetButtons.Clear();
    }

    private void FreezeActions()
    {
        foreach(ActionButton btn in _actionButtons)
        {
            if(btn.IsClicked)
            {
                btn.SetFrameVisible();
                btn.Disable();
            }
            else
            {
                btn.Disable();
            }
        }
    }

    private void InstantiateActionsBar()
    {
        _actionsBarGO = Instantiate(_actionsPrefab);
        _actionsBarGO.transform.position = _actionsBarPos;
        
        GameObject attackBtnGO = Instantiate(_actionButtonPrefab);
        attackBtnGO.transform.position = _attackBtnPos;
        _attackBtn = attackBtnGO.GetComponent<ActionButton>();
        _attackBtn.Action = ChousenAction.Attack;

        _actionButtonGOs.Add(attackBtnGO);
        _actionButtons.Add(_attackBtn);

        GameObject blockBtnGO = Instantiate(_actionButtonPrefab);
        blockBtnGO.transform.position = _blockBtnPos;
        _blockBtn = blockBtnGO.GetComponent<ActionButton>();
        _blockBtn.Action = ChousenAction.Block;

        _actionButtons.Add(_blockBtn);
        _actionButtonGOs.Add(blockBtnGO);

        GameObject runAwayBtnGO = Instantiate(_actionButtonPrefab);
        runAwayBtnGO.transform.position = _runAwayBtnPos;
        _runAwayBtn = runAwayBtnGO.GetComponent<ActionButton>();
        _runAwayBtn.Action = ChousenAction.RunAway;

        _actionButtonGOs.Add(runAwayBtnGO);
        _actionButtons.Add(_runAwayBtn);
    }

    private void InstantiateTargetsBar()
    {
        _targetsBarGO = Instantiate(_targetsPrefab);
        _targetsBarGO.transform.position = _targetsBarPos;
        
        GameObject headBtnGO = Instantiate(_targetButtonPrefab);
        headBtnGO.transform.position = _headBtnPos;
        _headBtn = headBtnGO.GetComponent<TargetButton>();
        _headBtn.Target = ChosenTarget.Head;

        _targetButtonGOs.Add(headBtnGO);
        _targetButtons.Add(_headBtn);

        GameObject bodyBtnGO = Instantiate(_targetButtonPrefab);
        bodyBtnGO.transform.position = _bodyBtnPos;
        _bodyBtn = bodyBtnGO.GetComponent<TargetButton>();
        _bodyBtn.Target = ChosenTarget.Body;

        _targetButtonGOs.Add(bodyBtnGO);
        _targetButtons.Add(_bodyBtn);

        GameObject eyesBtnGO = Instantiate(_targetButtonPrefab);
        eyesBtnGO.transform.position = _eyesBtnPos;
        _eyesBtn = eyesBtnGO.GetComponent<TargetButton>();
        _eyesBtn.Target = ChosenTarget.Eyes;

        _targetButtonGOs.Add(eyesBtnGO);
        _targetButtons.Add(_eyesBtn);
    }
}