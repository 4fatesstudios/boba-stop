using System.Collections;
using System.Collections.Generic;
using SimpleCombat.Components;
using UnityEngine;

namespace SimpleCombat {
    public interface IDamageable {
        void OnDamageTaken(Attack attackComponent, CombatController source);
        void OnDeath(Attack attackComponent, CombatController source);
    }
}