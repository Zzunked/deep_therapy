using System;
using UnityEngine;


public class Blast : MonoBehaviour
{
    public event Action<int> BlastDamagePhase;
    private int _damage;

    public int Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    public void OnBlastDamagePhase()
    {
        BlastDamagePhase?.Invoke(_damage);
    }
}