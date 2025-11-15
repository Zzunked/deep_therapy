using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


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

    public IEnumerator PlayAnimationEnum()
    {
        _animator.Play(_animationName);

        yield return null;

        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);

        Debug.Log("Playing " + _animator + " animation for " + info.length + "s");

        yield return new WaitForSeconds(info.length);
    }
}