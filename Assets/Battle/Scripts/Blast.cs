using System;

public class Blast : ActionAnimation
{
    public event Action BlastDamagePhase;
    public event Action BlastSignPhase;
    protected override string _animationName => "Blast";


    public void OnBlastAnimationEnd()
    {
        Destroy(gameObject);
    }

    public void OnBlastDamagePhase()
    {
        BlastDamagePhase?.Invoke();
    }

    public void OnBlastSignPhase()
    {
        BlastSignPhase?.Invoke();
    }
}