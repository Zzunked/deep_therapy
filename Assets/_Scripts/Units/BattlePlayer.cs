using UnityEngine;
using System;


public class BattlePlayer : BattleUnit
{
    [SerializeField] private float _runAwayProbability = 0.2f;
    [SerializeField] private Heart _heart1;
    [SerializeField] private Heart _heart2;
    [SerializeField] private Heart _heart3;

    public bool CanRunAway()
    {
        float runAwyRate = UnityEngine.Random.Range(0f, 1f);

        Debug.Log("Players runAwayProbability: " + _runAwayProbability + ", runAwyRate: " + runAwyRate);

        return (runAwyRate <= _runAwayProbability) ? true : false;
    }

    private float GetHeartPercent(float band, float nextEdge, float healthPercent)
    {
        return (healthPercent - nextEdge) / band;
    }

    public void ResetHearts()
    {
        float fullHeart = 1;

        _heart1.UpdateHeart(fullHeart);
        _heart2.UpdateHeart(fullHeart);
        _heart3.UpdateHeart(fullHeart);
    }

    public void UpdateHearts()
    {
        float band;
        float heartPercent;
        float edge1 = 0.66f;
        float edge2 = 0.33f;
        float edge3 = 0;
        float healthPercent = _health / _initialHealth;
        float noHeart = 0;

        if(healthPercent > edge1)
        {
            band = 1 - edge1;
            heartPercent = GetHeartPercent(band, edge1, healthPercent);
            Debug.Log($"heartPercent: {heartPercent} = ({healthPercent} - {edge1}) / (1 - {edge1})");
            _heart1.UpdateHeart(heartPercent);
        }
        else if(healthPercent > edge2)
        {
            band = edge1 - edge2;
            heartPercent = GetHeartPercent(band, edge2, healthPercent);
            _heart1.UpdateHeart(noHeart);
            _heart2.UpdateHeart(heartPercent);
        }
        else if(healthPercent > edge3)
        {
            band = edge2 - edge3;
            heartPercent = GetHeartPercent(band, edge3, healthPercent);
            _heart1.UpdateHeart(noHeart);
            _heart2.UpdateHeart(noHeart);
            _heart3.UpdateHeart(heartPercent);
        }
        else
        {
            _heart1.UpdateHeart(noHeart);
            _heart2.UpdateHeart(noHeart);
            _heart3.UpdateHeart(noHeart);
        }
    }
}