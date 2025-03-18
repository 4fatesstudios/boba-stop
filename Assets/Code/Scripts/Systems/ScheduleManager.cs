using System.Collections.Generic;
using BobaStop.Data.Saved;
using BobaStop.Systems.World;
using UnityEngine.Events;

namespace BobaStop.Systems
{
    public struct ScheduledTime
    {
        private float startTime;
        private float endTime;

        public ScheduledTime(ScheduledTime scheduledTime) {
            startTime = scheduledTime.GetStartTime();
            endTime = scheduledTime.GetEndTime();
            
            ValidateScheduledTime();
        }
        
        public ScheduledTime(float startTime, float endTime) {
            this.startTime = startTime;
            this.endTime = endTime;

            ValidateScheduledTime();
        }

        public float GetStartTime() {
            return startTime;
        }

        public float GetEndTime() {
            return endTime;
        }

        public void ChangeStartTime(float newStartTime) {
            startTime = newStartTime;
            if (startTime > endTime) {
                endTime = startTime;
            }
        }

        public void ChangeEndTime(float newEndTime) {
            endTime = newEndTime;
            if (endTime < startTime) {
                startTime = endTime;
            }
        }

        private void ValidateScheduledTime() {
            if (startTime > endTime) {
                endTime = startTime;
            } else if (endTime < startTime) {
                startTime = endTime;
            }
        }

        public bool IsValid() {
            return startTime == 0 && endTime == 0;
        }
    }
    
    // struct that takes timeofdays list and set times
    public class Schedule {
        private Dictionary<DayOfWeek, ScheduledTime> operations = new() {
            { DayOfWeek.Sunday, new ScheduledTime() },
            { DayOfWeek.Monday, new ScheduledTime() },
            { DayOfWeek.Tuesday, new ScheduledTime() },
            { DayOfWeek.Wednesday, new ScheduledTime() },
            { DayOfWeek.Thursday, new ScheduledTime() },
            { DayOfWeek.Friday, new ScheduledTime() },
            { DayOfWeek.Saturday, new ScheduledTime() }
        };

        public void AddOperationTime(DayOfWeek dayOfWeek, ScheduledTime operation) {
            operations[dayOfWeek] = operation;
        }

        public ScheduledTime GetOperationTime(DayOfWeek dayOfWeek) {
            return operations[dayOfWeek];
        }
    }

    public interface IScheduledObject {
        public ScheduledTime GetScheduledTime();
        public void SetIsScheduledTime(bool isScheduledTime);
        public bool GetIsScheduledTime();
    }

    public struct ScheduleData {
        public IScheduledObject scheduledObject;
        public ScheduledTime scheduledTime;

        public ScheduleData(IScheduledObject scheduledObject, ScheduledTime scheduledTime) {
            this.scheduledObject = scheduledObject;
            this.scheduledTime = scheduledTime;
        }
    }
    
    public class ScheduleManager : RealtimeSystemManager {
        List<ScheduleData> scheduledObjects = new();
        private float currentTime;
        
        public override void Start() {
            
        }
        
        public override void Update() {
            currentTime = GameManager.Instance.dayCycleManager.GetAdjustedTime();
            CheckScheduledObjects();
        }

        public void AddToSchedule(IScheduledObject scheduledObject, ScheduledTime scheduledTime) {
            scheduledObjects.Add(new ScheduleData(scheduledObject, scheduledTime));
        }

        private void CheckScheduledObjects() {
            foreach (var scheduledObject in scheduledObjects) {
                HandleScheduledData(scheduledObject);
            }
        }

        private void HandleScheduledData(ScheduleData scheduleData) {
            var scheduledTime = scheduleData.scheduledTime;
            var scheduledObject = scheduleData.scheduledObject;

            // return early if not scheduled
            if (scheduledObject.GetIsScheduledTime() == false &&
                !IsFloatInbetween(scheduledTime.GetStartTime(), scheduledTime.GetEndTime(), currentTime)) return;

            // return early if the scheduled time is not valid for setting it to true
            if (scheduledTime.GetStartTime() > currentTime || scheduledTime.GetEndTime() < currentTime) {
                scheduledObject.SetIsScheduledTime(false);
                return;
            }

            // if between the times, set it to true
            scheduledObject.SetIsScheduledTime(true);
        }

        private bool IsFloatInbetween(float a, float b, float t) {
            return t <= a && t >= b;
        }
    }
}
