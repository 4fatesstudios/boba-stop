using System.Collections;
using System.Collections.Generic;
using BobaStop.Data.Level;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop.Context
{
    public static class WorldContextManager {
        public static string GetWeather() {
            return "sunny";
        }

        public static string GetTime() {
            return GameManager.Instance.dayCycleManager.GetStandardTime();
        }

        public static string GetCurrentLevelAreaName() {
            return GameManager.Instance.levelManagerHelper.GetCurrentLevelProperties().localAreaName;
        }

        public static string[] GetCurrentLevelContext() {
            return GameManager.Instance.levelManagerHelper.GetCurrentLevelProperties().levelContext;
        }
    }
}
