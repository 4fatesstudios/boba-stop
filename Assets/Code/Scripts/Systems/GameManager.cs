using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Data.Saved;
using BobaStop.Systems.World;
using BobaStop.Systems.DataManagement;
using BobaStop.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace BobaStop.Systems {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private Player player;
        
        #region Manager Instances & Related Vars
        public SaveSystem saveSystem;
        private GameData gameData;
        
        public PlayerDataManager playerDataManager;
        public WorldDataManager worldDataManager;
        
        public CompanionManager companionManager;
        
        public DayCycleManager dayCycleManager;
        public LevelManagerHelper levelManagerHelper;
        public ShopManager shopManager;

        public List<(SaveData, ISaveableData)> saveDataAssociations;
        
        // monobehavior classes
        [SerializeField] public SunlightManager sunlightManager;
        
        // public InventoryManager inventoryManager; to be uncommented later when inventory is worked on
        #endregion
        
        #region Unity Functions
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // make persistent across scenes
            }
            else {
                Destroy(gameObject); // delete duplicates
            }
            
            player.InstantiateSingleton();

            saveSystem = new SaveSystem();
            gameData = new GameData();
            
            // playerDataManager = new PlayerDataManager(playerData);
            // worldDataManager = new WorldDataManager(worldData);
            
            companionManager = new CompanionManager();
            
            dayCycleManager = new DayCycleManager();
            levelManagerHelper = new LevelManagerHelper();
            shopManager = new ShopManager();
            
            UpdateSaveAssociations();
            
            // Call EndDay when DayCycleManager hits 12:00 AM
            dayCycleManager.OnDayEnd += EndDay;
        }

        private void Start() {
            dayCycleManager.Start();
            dayCycleManager.Unpause();
            shopManager.Start();
        }

        private void Update() {
            dayCycleManager.Update();
        }
        #endregion

        public GameData GetGameData() {
            return gameData;
        }
        
        /// <summary>
        /// Starts new day and unpauses necessary World systems
        /// Teleports player to be next to their bed
        /// TODO: if player was not at home, punishment ie loss of currency
        /// </summary>
        private void StartNewDay() {
            Player.Instance.transform.position = new Vector3(-2.68300009f,0.157000005f,0f); // DEFINE A DEFAULT START POSITION AT BED
            dayCycleManager.Reset();
            dayCycleManager.Unpause();
            shopManager.Unpause();

            Player.Instance.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Call to start end of day operations
        /// Currently called whenever DayCycleManager hits end of day
        /// Runs and end-of-day report
        /// TODO: implement end-of-day report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void EndDay(object sender, EventArgs eventArgs) {
            Player.Instance.gameObject.SetActive(false);
            
            // pause all World Systems
            dayCycleManager.Pause();
            shopManager.Pause();
            
            // ensure that after first save (after first day) it is no longer considered a first load
            if (gameData.firstLoad) gameData.firstLoad = false;
            
            // get data from managers and save to disk
            saveSystem.SaveAllDataFromGame();
            saveSystem.SaveDataToDisk(gameData);
            
            // run end report
            
            // while end report is running, change scene back to starting shop
            StartCoroutine(levelManagerHelper.LoadLevelAsync(levelManagerHelper.startingLevel, false));
            
            // temporary
            StartNewDay();
        }
        
        /// <summary>
        /// Replace save slot with a new save
        /// </summary>
        /// <param name="slot"></param>
        public void CreateNewSaveGame(int slot) {
            saveSystem.SetSaveLoadSlot(slot);
            saveSystem.DeleteData();
            LoadIntoGame(slot);
        }
        
        /// <summary>
        /// Load data from indicated save slot
        /// Calls saveSystem to load data to their necessary systems
        /// Changes scene to starting shop area
        /// </summary>
        /// <param name="slot">Relevant save slot</param>
        /// <returns></returns>
        public void LoadIntoGame(int slot) {
            saveSystem.SetSaveLoadSlot(slot);
            // if no save data found and not first load, do not continue
            if (saveSystem.SaveFileExists()) {
                saveSystem.LoadDataFromDisk(gameData);
            }
            if (gameData.firstLoad) {
                Debug.Log("first load");
                // do other first load things (ie cutscenes and stuff like that)
            }
            StartCoroutine(levelManagerHelper.LoadLevelAsync(levelManagerHelper.startingLevel, false));
            
            UpdateSaveAssociations();
            saveSystem.LoadAllDataToGame();
            
            StartNewDay();
        }

        private void UpdateSaveAssociations() {
            saveDataAssociations?.Clear();
            saveDataAssociations = new List<(SaveData, ISaveableData)> {
                (gameData.playerData, Player.Instance),
                (gameData.shopManagerData, shopManager)
            };
        }
    }
}