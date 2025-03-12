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
        
        [SerializeField] GameObject playerPrefab;
        
        #region Manager Instances & Related Vars
        public SaveSystem saveSystem;
        private AllSavedData allSavedData;
        
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
            
            saveSystem = new SaveSystem();
            allSavedData = ScriptableObject.CreateInstance<AllSavedData>();
            
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
        
        private void EndDay(object sender, EventArgs eventArgs) {
            // pause all World Systems
            dayCycleManager.Pause();
            shopManager.Pause();
            saveSystem.SaveAllDataFromGame();
        }

        public AllSavedData GetAllSavedData() {
            return allSavedData;
        }

        private void StartNewDay() {
            saveSystem.SaveDataToDisk(allSavedData);
            Player.Instance.enabled = true;
            Player.Instance.transform.position = new Vector3(-2.68300009f,0.157000005f,0f); // DEFINE A DEFAULT START POSITION AT BED
        }

        public void CreateNewSaveGame(int slot) {
            saveSystem.SetSaveLoadSlot(slot);
            saveSystem.DeleteData();
            StartCoroutine(LoadIntoGame(slot));
        }

        public IEnumerator LoadIntoGame(int slot) {
            saveSystem.SetSaveLoadSlot(slot);
            
            yield return StartCoroutine(levelManagerHelper.LoadLevelAsync(levelManagerHelper.startingLevel, false));
            
            if (allSavedData.firstLoad) {
                allSavedData.firstLoad = false;
                UpdateSaveAssociations();
                saveSystem.SaveDataToDisk(allSavedData);
                Debug.Log("first load");
                if (allSavedData.playerData == null) {
                    Debug.Log("playerData is null");
                }
            }
            saveSystem.LoadDataFromDisk(allSavedData);
            // UpdateSaveAssociations();
            saveSystem.LoadAllDataToGame();
            StartNewDay();
        }

        private void UpdateSaveAssociations() {
            saveDataAssociations?.Clear();
            saveDataAssociations = new List<(SaveData, ISaveableData)> {
                (allSavedData.playerData, Player.Instance),
                (allSavedData.shopManagerData, shopManager)
            };
        }
    }
}