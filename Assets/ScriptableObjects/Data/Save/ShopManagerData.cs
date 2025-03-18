using System.Collections.Generic;
using UnityEngine;
using BobaStop.Items;
using BobaStop.Systems;
using BobaStop.Systems.World;

namespace BobaStop.Data.Saved
{
    [System.Serializable]
    public class ShopManagerData : SaveData {
        [SerializeField] public List<Resource> shopSelection = new() {null, null, null, null};
        [SerializeField] public List<Resource> shopInventory = new() {null, null, null, null};
        [SerializeField] public int shopReputationLevel = 1;
        [SerializeField] public int currentShopExp = 0;
        public Schedule schedule = new();
        
        private ScheduledTime defaultScheduledTime = new(DayCycleManager.AFTERNOON_THRESHOLD, DayCycleManager.EVENING_THRESHOLD);

        public ShopManagerData() {
            schedule.AddOperationTime(DayOfWeek.Monday, new ScheduledTime(defaultScheduledTime));
            schedule.AddOperationTime(DayOfWeek.Tuesday, new ScheduledTime(defaultScheduledTime));
            schedule.AddOperationTime(DayOfWeek.Wednesday, new ScheduledTime(defaultScheduledTime));
            schedule.AddOperationTime(DayOfWeek.Thursday, new ScheduledTime(defaultScheduledTime));
            schedule.AddOperationTime(DayOfWeek.Friday, new ScheduledTime(defaultScheduledTime));
        }
    }
}
