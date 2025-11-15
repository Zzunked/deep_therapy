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
