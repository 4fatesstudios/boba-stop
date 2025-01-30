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
    public static string SaveFilePath()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }
    
}
