public class ActionButton : Button
{
    private PlayersAction _action;

    public PlayersAction Action
    {
        get => _action;
        set => _action = value;
    }
}