public class TargetButton : Button
{
    private PlayersTarget _target;

    public PlayersTarget Target
    {
        get => _target;
        set => _target = value;
    }
}