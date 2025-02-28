using System;
using UnityEngine;

namespace BobaStop.Systems {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        #region Manager Instances
        private static SaveSystem saveSystem;
        private static PlayerDataManager playerDataManager;
        private static WorldDataManager worldDataManager;
        private static DayCycleManager dayCycleManager;
        private static int saveSlot;
        #endregion
        
        #region Unity Functions
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Make persistent across scenes
            }
            else {
                Destroy(gameObject); // Delete duplicates
            }
            
            saveSystem = new SaveSystem();
            playerDataManager = new PlayerDataManager();
            worldDataManager = new WorldDataManager();
            dayCycleManager = new DayCycleManager();
        }

        private void Start() {
            playerDataManager.Start();
            worldDataManager.Start();
            dayCycleManager.Start();
        }

        private void Update() {
            dayCycleManager.Update();
        }
        #endregion
        
        #region SaveSystem Functions
        public void SetSaveSlot(int slot) {
            saveSlot = slot;
        }

        public void LoadData() {
            saveSystem.SetSaveLoadSlot(saveSlot);
            saveSystem.LoadData(playerDataManager.playerData, worldDataManager.worldData);
        }

        public void SaveData() {
            saveSystem.SetSaveLoadSlot(saveSlot);
            saveSystem.SaveData(playerDataManager.playerData, worldDataManager.worldData);
        }
        #endregion
        
        #region Player Data Functions
        public bool SetPlayerName(string playerName) {
            return playerDataManager.SetPlayerName(playerName);
        }

        public string GetPlayerName() {
            return playerDataManager.GetPlayerName();
        }

        public int GetPlayerPearls() {
            return playerDataManager.GetPlayerPearls();
        }

        public void AddPlayerPearls(int pearls) {
            playerDataManager.AddPlayerPearls(pearls);
        }
        #endregion
        
        #region Day Cycle Functions
        public string GetStandardTime() {
            return dayCycleManager.GetStandardTime();
        }

        public string GetMilitaryTime() {
            return dayCycleManager.GetMilitaryTime();
        }
        
        public void SubscribeToDayPhaseChanges(EventHandler<DayCycleManager.OnDayPhaseChangeEventArgs> handler)
        {
            dayCycleManager.OnDayPhaseChanged += handler;
        }

        public void UnsubscribeFromDayPhaseChanges(EventHandler<DayCycleManager.OnDayPhaseChangeEventArgs> handler)
        {
            dayCycleManager.OnDayPhaseChanged -= handler;
        }
        #endregion
        
        
    }
}