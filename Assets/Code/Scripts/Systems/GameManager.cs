using System;
using BobaStop.Data.Saved;
using BobaStop.Systems.World;
using BobaStop.Systems.DataManagement;
using UnityEngine;

namespace BobaStop.Systems {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        #region Manager Instances & Related Vars
        public SaveSystem saveSystem;
        
        public PlayerDataManager playerDataManager;
        private PlayerData playerData;
        
        public WorldDataManager worldDataManager;
        private WorldData worldData;
        
        public CompanionManager companionManager;
        
        public DayCycleManager dayCycleManager;
        public LevelManagerHelper levelManagerHelper;
        public ShopManager shopManager;
        
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
            
            playerDataManager = new PlayerDataManager(playerData);
            playerData = ScriptableObject.CreateInstance<PlayerData>();
            
            worldDataManager = new WorldDataManager(worldData);
            worldData = ScriptableObject.CreateInstance<WorldData>();
            
            companionManager = new CompanionManager();
            
            dayCycleManager = new DayCycleManager();
            levelManagerHelper = new LevelManagerHelper();
            shopManager = new ShopManager();
            
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
        }

        private void ResetDay() {
            
        }
    }
}