using System.Collections;
using System.Collections.Generic;
using SimpleCombat;
using SimpleCombat.Components;
using UnityEngine;

public class EnemyTest : MonoBehaviour, IDamageable
{
    public void OnDamageTaken(Attack attackComponent, CombatController source) {
        Debug.Log("enemy test has taken damage!");
    }
    public void OnDeath(Attack attackComponent, CombatController source) {
        Debug.Log("enemy test has died!");
    }
}
