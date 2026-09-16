using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.Data.Saved;

namespace BobaStop.Systems
{
    public interface IPersistenceData {
        public abstract void LoadData(SaveData saveData);
        
        public abstract void SaveData(SaveData saveData);
    }
}
