using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager _instance;

    // Player currency and stats
    private int _pearls;
    private int _energy;
    private int _shopReputation;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Load saved data
        Load();
    }

    // Add or subtract Pearls
    public void AddPearls(int amount)
    {
        _pearls += amount;
        Save();
    }

    public void SubtractPearls(int amount)
    {
        _pearls = Mathf.Max(0, _pearls - amount); // pearls don't go below 0
        Save();
    }

    // Add or subtract Energy
    public void AddEnergy(int amount)
    {
        _energy += amount;
        Save();
    }

    public void SubtractEnergy(int amount)
    {
        _energy = Mathf.Max(0, _energy - amount); // energy doesn't go below 0
        Save();
    }

    // Add or subtract Shop Reputation
    public void AddShopReputation(int amount)
    {
        _shopReputation += amount;
        Save();
    }

    public void SubtractShopReputation(int amount)
    {
        _shopReputation = Mathf.Max(0, _shopReputation - amount); // rep doesn't go below 0
        Save();
    }

    // Save data using SaveSystem
    private void Save()
    {
        SaveSystem.Save(_pearls, _energy, _shopReputation);
    }

    // Load data using SaveSystem
    private void Load()
    {
        SaveSystem.Load(out _pearls, out _energy, out _shopReputation);
    }

    // Reset all data (for debugging or new game)
    public void ResetData()
    {
        SaveSystem.ResetData();
        Load(); // Make sure to reload the default values
    }
}