using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.Data.Saved;

namespace BobaStop.Systems
{
    public interface ISaveableData {
        // public void LoadSaveData(SaveData saveData) {
        //     if (data.GetType() == saveData.GetType()) {
        //         data.CopyFrom(saveData);
        //     }
        //     else {
        //         Debug.LogErrorFormat("SaveData is not of type {0}", saveData.GetType());
        //     }
        // }

        public abstract void LoadData(SaveData saveData);
        
        public abstract void WriteSaveData(SaveData saveData);
    }
}
