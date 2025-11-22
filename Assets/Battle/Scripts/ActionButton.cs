public class ActionButton : Button
{
    private ChousenAction _action;

    public ChousenAction Action
    {
        get => _action;
        set => _action = value;
    }
}