using System.Collections;
using System.Collections.Generic;
using BobaStop.Data.Saved;
using UnityEngine;

namespace BobaStop.Systems
{
    public struct ScheduledTime
    {
        private float startTime;
        private float endTime;

        public ScheduledTime(float startTime, float endTime) {
            this.startTime = startTime;
            this.endTime = endTime;

            ValidateScheduledTime();
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
    public class Schedule
    {
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
    
    public class ScheduleManager : MonoBehaviour {
        // subscribe to 
        
    }
}
