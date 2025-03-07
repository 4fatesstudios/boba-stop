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
        
        public DayCycleManager dayCycleManager;
        public LevelManagerHelper levelManagerHelper;
        public ShopManager shopManager;
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
            
            levelManagerHelper = new LevelManagerHelper();
            
            dayCycleManager = new DayCycleManager();
            shopManager = new ShopManager();
        }

        private void Start() {
            dayCycleManager.Start();
            shopManager.Start();
        }

        private void Update() {
            dayCycleManager.Update();
        }
        #endregion
    }
}