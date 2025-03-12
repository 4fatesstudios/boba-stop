using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Data.Saved {
    public class SaveData : ScriptableObject {
        public virtual void CopyFrom(SaveData other) {}
    }
}
