using System;
using System.Threading.Tasks;


public class Tentacle : ActionAnimation
{
    protected override string _animationName => "Tentacle";
    private TaskCompletionSource<bool> _crackTcs;
    private TaskCompletionSource<bool> _damageTcs;

    public TaskCompletionSource<bool> CrackTcs
    {
        get => _crackTcs;
        set => _crackTcs = value;
        
    }
    public TaskCompletionSource<bool> DamageTcs
    {
        get => _damageTcs;
        set => _damageTcs = value;
        
    }

    public void OnTentacleAnimationEnd()
    {
        Destroy(gameObject);
    }

    public void OnTentacleDamagePhase()
    {
        _damageTcs?.TrySetResult(true);
    }

    public void OnTentacleCrackPhase()
    {
        _crackTcs?.TrySetResult(true);
    }
}
