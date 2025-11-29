using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour
{
    [SerializeField] private ActionButton _button1;
    [SerializeField] private ActionButton _button2;
    [SerializeField] private ActionButton _button3;
    private List<ActionButton> _buttons;
    private PlayersAction _clickedAction = PlayersAction.None;
    public PlayersAction ClickedAction
    {
        get => _clickedAction;
        private set => _clickedAction = value;
    }

    public void Init(List<PlayersAction> actions)
    {
        _buttons = new List<ActionButton> {_button1, _button2, _button3};

        if (actions.Count != _buttons.Count)
            throw new ArgumentException("Number of actions does not match number of buttons");

        for(int idx = 0; idx < actions.Count; idx++)
        {
            _buttons[idx].Action = actions[idx];
            _buttons[idx].OnClick += SetClickedAction;
        }
    }

    private void SetClickedAction(Button button)
    {
        ActionButton actionButton = button as ActionButton;

        if(actionButton != null)
            ClickedAction = actionButton.Action;
    }

    public void FreezeActions()
    {
        foreach(ActionButton btn in _buttons)
        {
            btn.OnClick -= SetClickedAction;

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
}
