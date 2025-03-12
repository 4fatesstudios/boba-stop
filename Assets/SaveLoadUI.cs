using System.Collections;
using System.Collections.Generic;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop
{
    public class SaveLoadUI : MonoBehaviour {
        public void CreateNewSaveGame(int slot) {
            GameManager.Instance.CreateNewSaveGame(slot);
        }
        
        public void LoadGame(int slot) {
            StartCoroutine(GameManager.Instance.LoadIntoGame(slot));
        }
    }
}
