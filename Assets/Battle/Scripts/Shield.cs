public class Shield : ActionAnimation
{
    protected override string _animationName => "Shield";

    public void OnShieldAnimationEnd()
    {
        Destroy(gameObject);
    }
}