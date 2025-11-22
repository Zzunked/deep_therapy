public class TargetButton : Button
{
    private ChosenTarget _target;

    public ChosenTarget Target
    {
        get => _target;
        set => _target = value;
    }
}