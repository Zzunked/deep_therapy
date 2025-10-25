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
    [SerializeField] private Button attackActionBtn;
    [SerializeField] private Button blockActionBtn;
    [SerializeField] private Button runAwayActionBtn;

    [SerializeField] private Button headChoiceBtn;
    [SerializeField] private Button bodyChoiceBtn;
    [SerializeField] private Button eyesChoiceBtn;

    [SerializeField] private SpriteRenderer targetsSpriteRenderer;
    [SerializeField] private SpriteRenderer actionsSpriteRenderer;

    private ChousenAction chosenAction = ChousenAction.None;
    private ChosenTarget chosenTarget = ChosenTarget.None;

    void Start()
    {
        Reset();
    }

    void Update()
    {
        HandleActions();

        if (chosenAction == ChousenAction.Attack)
        {
            HandleTargets();
        }
    }

    public ChousenAction GetChousenAction()
    {
        return chosenAction;
    }

    public ChosenTarget GetChosenTarget()
    {
        return chosenTarget;
    }

    void HandleActions()
    {
        if (attackActionBtn.isClicked)
        {
            chosenAction = ChousenAction.Attack;
            DisableActionButtons();
            ShowTargets();
            EnableTargetButtons();
            attackActionBtn.isClicked = false;
        }
        else if (blockActionBtn.isClicked)
        {
            chosenAction = ChousenAction.Block;
            DisableActionButtons();
            blockActionBtn.isClicked = false;
        }
        else if (runAwayActionBtn.isClicked)
        {
            chosenAction = ChousenAction.RunAway;
            DisableActionButtons();
            runAwayActionBtn.isClicked = false;
        }
    }

    void HandleTargets()
    {
        if (headChoiceBtn.isClicked)
        {
            chosenTarget = ChosenTarget.Head;
            headChoiceBtn.isClicked = false;
        }
        else if (bodyChoiceBtn.isClicked)
        {
            chosenTarget = ChosenTarget.Body;
            bodyChoiceBtn.isClicked = false;
        }
        else if (eyesChoiceBtn.isClicked)
        {
            chosenTarget = ChosenTarget.Eyes;
            eyesChoiceBtn.isClicked = false;
        }
    }

    public void EnableActionButtons()
    {
        attackActionBtn.Enable();
        blockActionBtn.Enable();
        runAwayActionBtn.Enable();
    }

    public void DisableActionButtons()
    {
        attackActionBtn.Disable();
        blockActionBtn.Disable();
        runAwayActionBtn.Disable();
    }

    public void ShowActions()
    {
        actionsSpriteRenderer.sortingOrder = 1;
    }
    
    public void HideActions()
    {
        actionsSpriteRenderer.sortingOrder = -1;
    }

    public void HideActionButtons()
    {
        attackActionBtn.SetFrameInvisible();
        blockActionBtn.SetFrameInvisible();
        runAwayActionBtn.SetFrameInvisible();
    }

    public void EnableTargetButtons()
    {
        headChoiceBtn.Enable();
        bodyChoiceBtn.Enable();
        eyesChoiceBtn.Enable();
    }

    public void DisableTargetButtons()
    {
        headChoiceBtn.Disable();
        bodyChoiceBtn.Disable();
        eyesChoiceBtn.Disable();
    }

    public void HideTargetButtons()
    {
        headChoiceBtn.SetFrameInvisible();
        bodyChoiceBtn.SetFrameInvisible();
        eyesChoiceBtn.SetFrameInvisible();
    }

    public void ShowTargets()
    {
        targetsSpriteRenderer.sortingOrder = 1;
    }

    public void HideTargets()
    {
        targetsSpriteRenderer.sortingOrder = -1;
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
        chosenAction = ChousenAction.None;
    }

    public void ResetChosenTarget()
    {
        chosenTarget = ChosenTarget.None;
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