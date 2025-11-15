using UnityEngine;


public class Crack : ActionAnimation
{
    protected override string _animationName => "Crack";

    public void OnCrackAnimationEnd()
    {
        Destroy(gameObject);
    }
}