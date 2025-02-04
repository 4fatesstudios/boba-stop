using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem
{
    private static SaveData _saveData = new SaveData();
    
    [System.Serializable]
    public struct SaveData
    {
        // public PlayerSaveData PlayerData;
        // PlayerSaveData needs to be made in Player.cs, will add more to struct after
    }

    // creates save file
    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }

    public static void Save()
    {
        // HandleSaveData();
        
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }

    private static void HandleSaveData()
    {
        // GameManager.Instance.Player.Save(ref _saveData.PlayerData);
        // 
    }

    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());
        
        _saveData = JsonUtility.FromJson<SaveData>(saveContent);
        
    }

    private static void HandleLoadData()
    {
        // GameManager.Instance.Player.Load(_saveData.PlayerData);
    }
    
}
