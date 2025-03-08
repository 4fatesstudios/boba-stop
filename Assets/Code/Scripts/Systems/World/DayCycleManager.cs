using System;
using BobaStop.Characters;
using UnityEngine;

namespace BobaStop.Systems.World
{
    public enum DayPhase {
        Morning,
        Afternoon,
        Evening,
        Late
    }
    
    public class DayCycleManager : WorldSystemManager {
        private const float SECONDS_IN_MINUTE = 1.0f;
        private const float SECONDS_IN_HOUR = 60.0f;
        private const float SECONDS_IN_DAY = 1440.0f;

        private const float PLAYER_TIME_OFFSET = 480;

        public readonly float MORNING_THRESHOLD = 480; // 8 AM
        public readonly float AFTERNOON_THRESHOLD = 720; // 12 PM
        public readonly float EVENING_THRESHOLD = 1080; // 6 PM
        public readonly float LATE_THRESHOLD = 0; // 12 AM
        
        // debugging purposes only should always be 1.0
        private float timeMultiplier = 50.0f;

        private float elapsedSeconds;
        private DayPhase currentDayPhase;

        public event EventHandler<OnDayPhaseChangeEventArgs> OnDayPhaseChanged; // event that is triggered whenever day phase changes
        
        // possibly deprecated
        public event EventHandler OnDayEnd; // event that is triggered whenever day ends (ends on LATE_THRESHOLD)
        
        public class OnDayPhaseChangeEventArgs : EventArgs {
            public DayPhase DayPhase { get; }

            public OnDayPhaseChangeEventArgs(DayPhase dayPhase) {
                DayPhase = dayPhase;
            }
        }

        public override void Start() {
            isPaused = true;
            elapsedSeconds = 0;
            UpdateDayPhase(); // initialize the correct day phase
        }

        public override void Update() {
            if (!isPaused) {
                elapsedSeconds += Time.deltaTime*timeMultiplier;
                if (elapsedSeconds + PLAYER_TIME_OFFSET >= SECONDS_IN_DAY) {
                    ResetDay();
                }
                UpdateDayPhase();
            }
        }

        public float GetAdjustedTime() {
            return elapsedSeconds + PLAYER_TIME_OFFSET;
        }
        
        public float SecondsInMinute() {
            return SECONDS_IN_MINUTE;
        }

        public float SecondsInHour() {
            return SECONDS_IN_HOUR;
        }

        public float SecondsInDay() {
            return SECONDS_IN_DAY;
        }

        public bool GetIsTimeMoving() {
            return isPaused;
        }

        public DayPhase GetCurrentDayPhase() {
            return currentDayPhase;
        }

        private void ResetDay() {
            elapsedSeconds = 0;
        }

        private void UpdateDayPhase() {
            float currentTime = (elapsedSeconds + PLAYER_TIME_OFFSET) % SECONDS_IN_DAY;

            DayPhase newPhase;
            if (currentTime >= MORNING_THRESHOLD && currentTime < AFTERNOON_THRESHOLD) {
                newPhase = DayPhase.Morning;
            } else if (currentTime >= AFTERNOON_THRESHOLD && currentTime < EVENING_THRESHOLD) {
                newPhase = DayPhase.Afternoon;
            } else if (currentTime >= EVENING_THRESHOLD) {
                newPhase = DayPhase.Evening;
            } else {
                newPhase = DayPhase.Late;
            }

            if (newPhase != currentDayPhase) {
                currentDayPhase = newPhase;
                TriggerOnDayPhaseChange(newPhase);
                
                if (currentDayPhase == DayPhase.Evening)
                    TriggerOnDayEnd();
            }
        }

        private void TriggerOnDayPhaseChange(DayPhase newPhase) {
            OnDayPhaseChanged?.Invoke(this, new OnDayPhaseChangeEventArgs(newPhase));
        }

        private void TriggerOnDayEnd() {
            OnDayEnd?.Invoke(this, EventArgs.Empty);
        }

        public string GetMilitaryTime() {
            int totalMinutes = Mathf.FloorToInt((elapsedSeconds + PLAYER_TIME_OFFSET) / SECONDS_IN_MINUTE);
            int hours = (totalMinutes / (int)(SECONDS_IN_HOUR / SECONDS_IN_MINUTE)) % 24;
            int minutes = totalMinutes % (int)(SECONDS_IN_HOUR / SECONDS_IN_MINUTE);
            return $"{hours:D2}:{minutes:D2}";
        }

        public string GetStandardTime() {
            int totalMinutes = Mathf.FloorToInt((elapsedSeconds + PLAYER_TIME_OFFSET) / SECONDS_IN_MINUTE);
            int hours = (totalMinutes / (int)(SECONDS_IN_HOUR / SECONDS_IN_MINUTE)) % 24;
            int minutes = totalMinutes % (int)(SECONDS_IN_HOUR / SECONDS_IN_MINUTE);
            string period = hours >= 12 ? "PM" : "AM";
            int standardHours = hours % 12;
            if (standardHours == 0) standardHours = 12;
            return $"{standardHours}:{minutes:D2} {period}";
        }
    }
}
