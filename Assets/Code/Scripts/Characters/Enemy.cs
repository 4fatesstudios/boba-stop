using System.Collections;
using System.Collections.Generic;
using BobaStop.LootTables;
using SimpleCombat;
using SimpleCombat.Components;
using UnityEngine;

namespace BobaStop.Characters {
    public class Enemy : Character {
        [Header("Loot Settings")]
        [SerializeField] protected LootTable lootTable;
        [SerializeField] protected GameObject pickupPrefab;
        private GeneratePickupableLoot lootGenerator;

        protected override void Start() {
            base.Start();
            lootGenerator = gameObject.AddComponent<GeneratePickupableLoot>();
            lootGenerator.Initialize(lootTable, gameObject.transform, pickupPrefab);
        }
        
        public override void OnDeath(Attack attackComponent, CombatController source) {
            if (lootGenerator != null)
                lootGenerator.GenerateLoot();
            
            base.OnDeath(attackComponent, source);
        }
    }
}