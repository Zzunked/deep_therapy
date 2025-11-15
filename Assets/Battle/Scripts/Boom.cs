using UnityEngine;
using System;


public class Boom : ActionAnimation
{
    protected override string _animationName => "Boom";

    public void OnBoomAnimationEnd()
    {
        Destroy(gameObject);
    }
}
