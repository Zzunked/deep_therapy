using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public enum ChousenAction
{
    None,
    Attack,
    Block,
    RunAway

}

public enum ChosenBodyPart
{
    None,
    Head,
    Body,
    Eyes
}


public class ActionBar : MonoBehaviour
{
    [SerializeField] private GameObject attackBtnObj;
    [SerializeField] private GameObject blockBtnObj;
    [SerializeField] private GameObject runAwayBtnObj;

    [SerializeField] private GameObject headBtnObj;
    [SerializeField] private GameObject bodyBtnObj;
    [SerializeField] private GameObject eyesBtnObj;

    public ChousenAction chosenAction = ChousenAction.None;
    public ChosenBodyPart chosenBodyPart = ChosenBodyPart.None;

    private Button attackActionBtn;
    private Button blockActionBtn;
    private Button runAwayActionBtn;

    private Button headChoiceBtn;
    private Button bodyChoiceBtn;
    private Button eyesChoiceBtn;

    void Start()
    {
        attackActionBtn = attackBtnObj.GetComponent<Button>();
        blockActionBtn = blockBtnObj.GetComponent<Button>();
        runAwayActionBtn = runAwayBtnObj.GetComponent<Button>();

        headChoiceBtn = headBtnObj.GetComponent<Button>();
        bodyChoiceBtn = bodyBtnObj.GetComponent<Button>();
        eyesChoiceBtn = eyesBtnObj.GetComponent<Button>();

        EnableActionButtons();
    }

    void Update()
    {
        HandleActionButton();

        if (chosenAction == ChousenAction.Attack)
        {
            EnableTargetButtons();
            HandleBodyPartsButton();
        }
    }

    void HandleBodyPartsButton()
    {
        if (headChoiceBtn.isClicked)
        {
            chosenBodyPart = ChosenBodyPart.Head;
            DisableTargetButtons();
            headChoiceBtn.isClicked = false;
        }
        else if (bodyChoiceBtn.isClicked)
        {
            chosenBodyPart = ChosenBodyPart.Body;
            DisableTargetButtons();
            bodyChoiceBtn.isClicked = false;
        }
        else if (eyesChoiceBtn.isClicked)
        {
            chosenBodyPart = ChosenBodyPart.Eyes;
            DisableTargetButtons();
            eyesChoiceBtn.isClicked = false;
        }
    }

    void HandleActionButton()
    {
        if (attackActionBtn.isClicked)
        {
            chosenAction = ChousenAction.Attack;
            DisableActionButtons();
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

    public void EnableActionButtons()
    {
        attackActionBtn.Enable();
        blockActionBtn.Enable();
        runAwayActionBtn.Enable();
    }

    void DisableActionButtons()
    {
        attackActionBtn.Disable();
        blockActionBtn.Disable();
        runAwayActionBtn.Disable();
    }

    public void HideActionButtons()
    {
        attackActionBtn.SetFrameInvisible();
        blockActionBtn.SetFrameInvisible();
        runAwayActionBtn.SetFrameInvisible();
    }

    void EnableTargetButtons()
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


}