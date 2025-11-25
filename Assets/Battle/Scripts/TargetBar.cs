using UnityEngine;
using System;
using System.Collections.Generic;

public class TargetBar : MonoBehaviour
{
    [SerializeField] private TargetButton _button1;
    [SerializeField] private TargetButton _button2;
    [SerializeField] private TargetButton _button3;
    private List<TargetButton> _buttons;
    private PlayersTarget _clickedTarget = PlayersTarget.None;

    public PlayersTarget ClickedTarget
    {
        get => _clickedTarget;
        private set => _clickedTarget = value;
    }

    public void Init(List<PlayersTarget> targets)
    {
        var buttons = new List<TargetButton> {_button1, _button2, _button3};
        _buttons = buttons;

        if (targets.Count != _buttons.Count)
            throw new ArgumentException("Number of targets does not match number of buttons");

        for(int idx = 0; idx < targets.Count; idx++)
        {
            _buttons[idx].Target = targets[idx];
            _buttons[idx].OnClick += SetClickedTarget;
        }
    }

    private void SetClickedTarget(Button button)
    {
        TargetButton targetButton = button as TargetButton;

        if(targetButton != null)
            ClickedTarget = targetButton.Target;
    }
}
