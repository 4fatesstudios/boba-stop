using System;
using UnityEngine;

namespace BobaStop.Systems
{
    public enum DayPhase {
        Morning,
        Afternoon,
        Evening,
        Late
    }
    
    public class DayCycleManager {
        private const float SECONDS_IN_MINUTE = 1.0f;
        private const float SECONDS_IN_HOUR = 60.0f;
        private const float SECONDS_IN_DAY = 1440.0f;

        private const float PLAYER_TIME_OFFSET = 480;

        private const float MORNING_THRESHOLD = 480; // 8 AM
        private const float AFTERNOON_THRESHOLD = 720; // 12 PM
        private const float EVENING_THRESHOLD = 1080; // 6 PM
        private const float LATE_THRESHOLD = 0; // 12 AM

        private float elapsedSeconds;
        private bool timeIsMoving;
        private DayPhase currentDayPhase;

        public event EventHandler<OnDayPhaseChangeEventArgs> OnDayPhaseChanged;

        public class OnDayPhaseChangeEventArgs : EventArgs {
            public DayPhase DayPhase { get; }

            public OnDayPhaseChangeEventArgs(DayPhase dayPhase) {
                DayPhase = dayPhase;
            }
        }

        public void Start() {
            timeIsMoving = false;
            elapsedSeconds = 0;
            UpdateDayPhase(); // initialize the correct day phase
        }

        public void Update() {
            if (timeIsMoving) {
                elapsedSeconds += Time.deltaTime;
                if (elapsedSeconds >= SECONDS_IN_DAY) {
                    ResetTime();
                }
                UpdateDayPhase();
            }
        }
        
        public float SecondsInMinute() {
            return SECONDS_IN_MINUTE;
        }

        public float SecondsInHour() {
            return SECONDS_IN_HOUR;
        }

        public bool GetIsTimeMoving() {
            return timeIsMoving;
        }

        public void StartDay() {
            timeIsMoving = true;
        }

        public void PauseDay() {
            timeIsMoving = false;
        }

        private void ResetTime() {
            elapsedSeconds = 0;
            UpdateDayPhase();
        }

        private void UpdateDayPhase() {
            float currentTime = (elapsedSeconds + PLAYER_TIME_OFFSET) % SECONDS_IN_DAY;

            DayPhase newPhase;
            if (currentTime >= MORNING_THRESHOLD && currentTime < AFTERNOON_THRESHOLD) {
                newPhase = DayPhase.Morning;
            } else if (currentTime >= AFTERNOON_THRESHOLD && currentTime < EVENING_THRESHOLD) {
                newPhase = DayPhase.Afternoon;
            } else if (currentTime >= EVENING_THRESHOLD || currentTime < LATE_THRESHOLD) {
                newPhase = DayPhase.Evening;
            } else {
                newPhase = DayPhase.Late;
            }

            if (newPhase != currentDayPhase) {
                currentDayPhase = newPhase;
                TriggerOnDayPhaseChange(newPhase);
            }
        }

        private void TriggerOnDayPhaseChange(DayPhase newPhase) {
            OnDayPhaseChanged?.Invoke(this, new OnDayPhaseChangeEventArgs(newPhase));
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
