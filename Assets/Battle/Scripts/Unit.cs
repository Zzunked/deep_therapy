using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private int healthPoints = 100;
    public bool isDead = false;

    public void TakeDamage(int damage)
    {
        healthPoints -= Math.Abs(damage);

        if (healthPoints <= 0)
        {
            Debug.Log(gameObject.name + " has died!");
            healthPoints = 0;
            isDead = true;
        }
        else
        {
            Debug.Log(gameObject.name + " HP: " + healthPoints);
        }
    }

    public int GetHeatlthPoints()
    {
        return healthPoints;
    }

    public void ResetHealth()
    {
        healthPoints = 100;
        isDead = false;
    }
}
