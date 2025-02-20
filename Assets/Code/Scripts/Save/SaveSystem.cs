using UnityEngine;

public static class SaveSystem
{
    // Constants for PlayerPrefs keys
    private const string PearlsKey = "PlayerPearls";
    private const string EnergyKey = "PlayerEnergy";
    private const string ShopReputationKey = "ShopReputation";

    // Save player's data to PlayerPrefs
    public static void Save(int pearls, int energy, int shopReputation)
    {
        PlayerPrefs.SetInt(PearlsKey, pearls);
        PlayerPrefs.SetInt(EnergyKey, energy);
        PlayerPrefs.SetInt(ShopReputationKey, shopReputation);
        PlayerPrefs.Save();
    }

    // Load player's data from PlayerPrefs
    public static void Load(out int pearls, out int energy, out int shopReputation)
    {
        pearls = PlayerPrefs.GetInt(PearlsKey, 0); // Default to 0 if no saved data
        energy = PlayerPrefs.GetInt(EnergyKey, 100); // Default to 100 Energy
        shopReputation = PlayerPrefs.GetInt(ShopReputationKey, 0); // Default to 0 Reputation
    }

    // Reset all player's data (for debugging or new game)
    public static void ResetData()
    {
        PlayerPrefs.SetInt(PearlsKey, 0);
        PlayerPrefs.SetInt(EnergyKey, 100);
        PlayerPrefs.SetInt(ShopReputationKey, 0);
        PlayerPrefs.Save();
    }
}
