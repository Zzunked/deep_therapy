using UnityEngine;
using System.Collections;
using System.Threading.Tasks;


public class BattlePlayer : BattleUnit
{
    [SerializeField] private float _runAwayProbability = 0.2f;

    public bool CanRunAway()
    {
        float runAwyRate = Random.Range(0f, 1f);

        Debug.Log("Players runAwayProbability: " + _runAwayProbability + ", runAwyRate: " + runAwyRate);

        return (runAwyRate <= _runAwayProbability) ? true : false;
    }
}