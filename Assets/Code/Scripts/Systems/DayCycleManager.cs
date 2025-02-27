using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Systems
{
    public class DayCycleManager {
        private const float SECONDS_IN_MINUTE = 1.0f;
        private const float SECONDS_IN_HOUR = 60.0f;
        private const float SECONDS_IN_DAY = 1440.0f;

        private float elapsedSeconds;

        private bool timeIsMoving;

        public void Start() {
            timeIsMoving = false;
        }

        public void Update() {
            if (timeIsMoving) {
                elapsedSeconds += Time.deltaTime;
                if (elapsedSeconds >= SECONDS_IN_DAY) {
                    // ResetTime();
                }
            }
        }

        public bool GetIsTimeMoving() {
            return timeIsMoving;
        }

        public void StartDay() {
            timeIsMoving = true;
        }

        private void ResetTime() {
            elapsedSeconds = 0;
        }

        public void PauseDay() {
            timeIsMoving = false;
        }

        public string GetMilitaryTime() {
            int totalMinutes = Mathf.FloorToInt(elapsedSeconds / SECONDS_IN_MINUTE);
            int hours = (totalMinutes / (int)(SECONDS_IN_HOUR / SECONDS_IN_MINUTE)) % 24;
            int minutes = totalMinutes % (int)(SECONDS_IN_HOUR / SECONDS_IN_MINUTE);
            return $"{hours:D2}:{minutes:D2}";
        }

        public string GetStandardTime() {
            int totalMinutes = Mathf.FloorToInt(elapsedSeconds / SECONDS_IN_MINUTE);
            int hours = (totalMinutes / (int)(SECONDS_IN_HOUR / SECONDS_IN_MINUTE)) % 24;
            int minutes = totalMinutes % (int)(SECONDS_IN_HOUR / SECONDS_IN_MINUTE);
            string period = hours >= 12 ? "PM" : "AM";
            int standardHours = hours % 12;
            if (standardHours == 0) standardHours = 12;
            return $"{standardHours}:{minutes:D2} {period}";
        }
    }

}
