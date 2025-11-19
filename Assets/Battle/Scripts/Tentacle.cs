using System;
using System.Threading.Tasks;


public class Tentacle : ActionAnimation
{
    protected override string _animationName => "Tentacle";
    public event Action TentacleDamagePhase;
    // public event Action TentacleCrackPhase;
    public TaskCompletionSource<bool> CrackTcs;


    public void OnTentacleAnimationEnd()
    {
        Destroy(gameObject);
    }

    public void OnTentacleDamagePhase()
    {
        TentacleDamagePhase?.Invoke();
    }

    public void OnTentacleCrackPhase()
    {
        CrackTcs?.TrySetResult(true);
    }
}
