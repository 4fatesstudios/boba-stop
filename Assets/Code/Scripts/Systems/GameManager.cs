using System;
using System.Collections.Generic;
using BobaStop.Data.Saved;
using BobaStop.Systems.World;
using BobaStop.Characters;
using BobaStop.Data.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BobaStop.Systems {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private Player player;
        
        #region Manager Instances & Related Vars
        public SaveSystem saveSystem;
        private GameData gameData;
        
        public CompanionManager companionManager;
        
        public DayCycleManager dayCycleManager;
        public ShopManager shopManager;
        public WorldManager worldManager;
        public ScheduleManager scheduleManager;
        public DialogueManager dialogueManager;
        public InventoryManager inventoryManager;

        public LevelManagerHelper levelManagerHelper;
        
        public List<(SaveData, IPersistenceData)> saveDataAssociations;
        
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
            
            player.Instantiate();

            saveSystem = new SaveSystem();
            gameData = new GameData();
            
            companionManager = new CompanionManager();
            
            dayCycleManager = new DayCycleManager();
            shopManager = new ShopManager();
            worldManager = new WorldManager();
            scheduleManager = new ScheduleManager();
            
            dialogueManager = new DialogueManager();
            inventoryManager = new InventoryManager();
            
            levelManagerHelper = new LevelManagerHelper();
            
            UpdateSaveAssociations();
            
            // Call EndDay when DayCycleManager hits 12:00 AM
            dayCycleManager.OnDayEnd += EndDay;
        }

        private void Start() {
            dayCycleManager.Start();
            shopManager.Start();
            scheduleManager.Start();
            dialogueManager.Start();
        }

        private void Update() {
            dayCycleManager.Update();
            shopManager.Update();
            scheduleManager.Update();

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                Application.Quit();
            }
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
            dayCycleManager.Reset();
            worldManager.Reset();
            
            UnpauseDay();

            Player.Instance.gameObject.SetActive(true);
            GameInput.Instance.EnableInputMapOnly(ActionMap.Default);
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
            GameInput.Instance.DisableAllInputs();
            
            PauseDay();
            
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

        public void PauseDay() {
            dayCycleManager.Pause();
            shopManager.Pause();
            scheduleManager.Pause();
        }

        public void UnpauseDay() {
            dayCycleManager.Unpause();
            shopManager.Unpause();
            scheduleManager.Unpause();
        }
        
        /// <summary>
        /// Replace save slot with a new save
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="playerName"></param>
        /// <param name="character"></param>
        public void CreateNewSaveGame(int slot, string playerName, PlayerCharacter character) {
            saveSystem.SetSaveLoadSlot(slot);
            saveSystem.DeleteData();
            LoadIntoGame(slot, playerName, character);
        }

        public void LoadIntoScenario(string scenario, string playerName, PlayerCharacter character) {
            var scene = scenario switch {
                "Aster" => "HomeOutside2",
                "Jade" => "HomeOutside",
                "Karen" => "HomeInside",
                "Kaden" => "TownSquare",
                _ => "DevScene1"
            };

            SceneManager.sceneLoaded += OnSceneLoaded;
            levelManagerHelper.SwitchScene(Resources.Load<LevelProperties>($"Data/Level/{scene}"));
            
            UpdateSaveAssociations();
            saveSystem.LoadAllDataToGame();
            
            Player.Instance.GetPlayerDataManager().SetPlayerCharacter(character);
            Player.Instance.GetPlayerDataManager().SetPlayerName(playerName);
            
            StartNewDay();
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
            Debug.Log(saveSystem.SaveFileExists());
            if (saveSystem.SaveFileExists()) {
                saveSystem.LoadDataFromDisk(gameData);
            }
            if (gameData.firstLoad) {
                Debug.Log("first load");
                // do other first load things (ie cutscenes and stuff like that)
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
            levelManagerHelper.SwitchScene(Resources.Load<LevelProperties>("Data/Level/DevScene1"));
            
            UpdateSaveAssociations();
            saveSystem.LoadAllDataToGame();
            
            StartNewDay();
        }

        public void LoadIntoGame(int slot, string playerName, PlayerCharacter character) {
            saveSystem.SetSaveLoadSlot(slot);
            // if no save data found and not first load, do not continue
            Debug.Log(saveSystem.SaveFileExists());
            if (saveSystem.SaveFileExists()) {
                saveSystem.LoadDataFromDisk(gameData);
            }
            if (gameData.firstLoad) {
                Debug.Log("first load");
                // do other first load things (ie cutscenes and stuff like that)
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
            levelManagerHelper.SwitchScene(Resources.Load<LevelProperties>("Data/Level/DevScene1"));
            
            UpdateSaveAssociations();
            saveSystem.LoadAllDataToGame();
            
            if (gameData.firstLoad) {
                Player.Instance.GetPlayerDataManager().SetPlayerCharacter(character);
                Player.Instance.GetPlayerDataManager().SetPlayerName(playerName);
            }
            
            StartNewDay();
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            PositionPlayerAfterSceneLoad();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void PositionPlayerAfterSceneLoad() {
            GameObject spawnGO = FindObjectOfType<PlayerSpawnPoint>()?.gameObject;
            if (spawnGO != null) {
                CharacterController cc = Player.Instance.GetComponent<CharacterController>();
                if (cc != null)
                {
                    cc.enabled = false;
                }
                Player.Instance.transform.position = spawnGO.transform.position;
                if (cc != null) {
                    cc.enabled = true;
                }
            }
            else {
                Debug.LogWarning("No PlayerSpawnPoint found in the scene!");
            }
        }

        private void UpdateSaveAssociations() {
            saveDataAssociations?.Clear();
            saveDataAssociations = new List<(SaveData, IPersistenceData)> {
                (gameData.playerData, Player.Instance.GetPlayerDataManager()),
                (gameData.shopManagerData, shopManager),
                (gameData.worldData, worldManager.GetWorldDataManager()),
                (gameData.inventoryData, inventoryManager),
                (gameData.allCompanionData, companionManager)
            };
        }
    }
}