using UnityEngine;


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


public class ActionBar : MonoBehaviour
{
    [SerializeField] private Button _attackActionBtn;
    [SerializeField] private Button _blockActionBtn;
    [SerializeField] private Button _runAwayActionBtn;

    [SerializeField] private Button _headChoiceBtn;
    [SerializeField] private Button _bodyChoiceBtn;
    [SerializeField] private Button _eyesChoiceBtn;

    [SerializeField] private SpriteRenderer _targetsSpriteRenderer;
    [SerializeField] private SpriteRenderer _actionsSpriteRenderer;

    private ChousenAction _chosenAction = ChousenAction.None;
    private ChosenTarget _chosenTarget = ChosenTarget.None;
    private bool _isTargetsEnabled = false;

    public bool IsTargetsEnabled()
    {
        return _isTargetsEnabled;
    }

    void Start()
    {
        Reset();
    }

    void Update()
    {
        HandleActions();

        if (_chosenAction == ChousenAction.Attack)
        {
            HandleTargets();
        }
    }

    public ChousenAction GetChousenAction()
    {
        return _chosenAction;
    }

    public ChosenTarget GetChosenTarget()
    {
        return _chosenTarget;
    }

    void HandleActions()
    {
        if (_attackActionBtn.IsClicked)
        {
            _chosenAction = ChousenAction.Attack;
            DisableActionButtons();
            ShowTargets();
            EnableTargetButtons();
            _attackActionBtn.IsClicked = false;
        }
        else if (_blockActionBtn.IsClicked)
        {
            _chosenAction = ChousenAction.Block;
            DisableActionButtons();
            _blockActionBtn.IsClicked = false;
        }
        else if (_runAwayActionBtn.IsClicked)
        {
            _chosenAction = ChousenAction.RunAway;
            DisableActionButtons();
            _runAwayActionBtn.IsClicked = false;
        }
    }

    void HandleTargets()
    {
        if (_headChoiceBtn.IsClicked)
        {
            _chosenTarget = ChosenTarget.Head;
            _headChoiceBtn.IsClicked = false;
        }
        else if (_bodyChoiceBtn.IsClicked)
        {
            _chosenTarget = ChosenTarget.Body;
            _bodyChoiceBtn.IsClicked = false;
        }
        else if (_eyesChoiceBtn.IsClicked)
        {
            _chosenTarget = ChosenTarget.Eyes;
            _eyesChoiceBtn.IsClicked = false;
        }
    }

    public void EnableActionButtons()
    {
        _attackActionBtn.Enable();
        _blockActionBtn.Enable();
        _runAwayActionBtn.Enable();
    }

    public void DisableActionButtons()
    {
        _attackActionBtn.Disable();
        _blockActionBtn.Disable();
        _runAwayActionBtn.Disable();
    }

    public void ShowActions()
    {
        _actionsSpriteRenderer.sortingOrder = 1;
    }
    
    public void HideActions()
    {
        _actionsSpriteRenderer.sortingOrder = -1;
    }

    public void HideActionButtons()
    {
        _attackActionBtn.SetFrameInvisible();
        _blockActionBtn.SetFrameInvisible();
        _runAwayActionBtn.SetFrameInvisible();
    }

    public void EnableTargetButtons()
    {
        _headChoiceBtn.Enable();
        _bodyChoiceBtn.Enable();
        _eyesChoiceBtn.Enable();
        _isTargetsEnabled = true;
    }

    public void DisableTargetButtons()
    {
        _headChoiceBtn.Disable();
        _bodyChoiceBtn.Disable();
        _eyesChoiceBtn.Disable();
        _isTargetsEnabled = false;
    }

    public void HideTargetButtons()
    {
        _headChoiceBtn.SetFrameInvisible();
        _bodyChoiceBtn.SetFrameInvisible();
        _eyesChoiceBtn.SetFrameInvisible();
    }

    public void ShowTargets()
    {
        _targetsSpriteRenderer.sortingOrder = 1;
    }

    public void HideTargets()
    {
        _targetsSpriteRenderer.sortingOrder = -1;
    }

    public void HideAndDisableAll()
    {
        DisableActionButtons();
        HideActionButtons();
        HideActions();

        DisableTargetButtons();
        HideTargetButtons();
        HideTargets();
    }

    public void ResetChosenAction()
    {
        _chosenAction = ChousenAction.None;
    }

    public void ResetChosenTarget()
    {
        _chosenTarget = ChosenTarget.None;
    }

    public void Reset()
    {
        ResetChosenAction();
        ResetChosenTarget();
        HideTargets();
        DisableTargetButtons();
        ShowActions();
        HideActionButtons();
        EnableActionButtons();
    }
}