using System;
using UnityEngine;


public class Tentacle : ActionAnimation
{
    protected override string _animationName => "Tentacle";
    public event Action TentacleDamagePhase;
    public event Action TentacleCrackPhase;

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
        TentacleCrackPhase?.Invoke();
    }
}

// Vector3(0.940999985,-3.45700002,0) - crack
// Vector3(2.93000007,-3.42000008,0) - tentackle