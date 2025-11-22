using System.Threading.Tasks;

public class Blast : ActionAnimation
{
    protected override string _animationName => "Blast";
    private TaskCompletionSource<bool> _signTcs;
    private TaskCompletionSource<bool> _damageTcs;

    public TaskCompletionSource<bool> SignTcs
    {
        get => _signTcs;
        set => _signTcs = value;
        
    }
    public TaskCompletionSource<bool> DamageTcs
    {
        get => _damageTcs;
        set => _damageTcs = value;
        
    }

    public void OnBlastAnimationEnd()
    {
        Destroy(gameObject);
    }

    public void OnBlastDamagePhase()
    {
        _damageTcs?.TrySetResult(true);
    }

    public void OnBlastSignPhase()
    {
        _signTcs?.TrySetResult(true);
    }
}