using System.Collections;
using System.Collections.Generic;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop
{
    public class SaveLoadUI : MonoBehaviour {
        SaveSystem saveSystem;

        private void Start() {
            saveSystem = GameManager.Instance.saveSystem;
        }
        
        public void CreateNewSaveGame(int slot) {
            saveSystem.SetSaveLoadSlot(slot);
        }
        
        public void LoadGame(int slot) {
            saveSystem.SetSaveLoadSlot(slot);
        }
    }
}
