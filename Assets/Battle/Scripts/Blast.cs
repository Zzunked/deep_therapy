using System;
using UnityEngine;


public class Blast : MonoBehaviour
{
    public event Action BlastDamagePhase;
    public event Action BlastSignPhase;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

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