using System;
using UnityEngine;
using BobaStop.Systems;
using BobaStop.Systems.World;

namespace BobaStop
{
    public class SunlightManager : MonoBehaviour
    {
        [SerializeField] Light sunLight;
        [SerializeField] Light moonLight;

        private float sunriseTime;
        private float afternoonTime;
        private float sunsetTime;
        private float moonriseTime;
        private float midnightTime;

        private Quaternion morningRotation = Quaternion.Euler(0, 60f, 180);
        private Quaternion afternoonRotation = Quaternion.Euler(50f, 0, 180);
        private Quaternion sunsetRotation = Quaternion.Euler(380, 300, 0);
        private Quaternion nightRotation = Quaternion.Euler(300, 360, 0); // Sun under horizon at night
        private Quaternion moonriseRotation = Quaternion.Euler(0, 60f, 180);
        private Quaternion moonPeakRotation = Quaternion.Euler(50f, 0, 180);

        private float morningTemp = 3000f;
        private float afternoonTemp = 6000f;
        private float sunsetTemp = 3000f;
        private float nightTemp = 2000f; // Cooler color for nighttime sun
        private float moonriseTemp = 6000f;
        private float moonPeakTemp = 15000f;

        private void Start() {
            sunriseTime = DayCycleManager.MORNING_THRESHOLD;
            afternoonTime = DayCycleManager.AFTERNOON_THRESHOLD;
            sunsetTime = DayCycleManager.EVENING_THRESHOLD;
            midnightTime = GameManager.Instance.dayCycleManager.SecondsInDay();

            // Start moonrise slightly before sunset for smoother transition
            moonriseTime = sunsetTime - GameManager.Instance.dayCycleManager.SecondsInHour();

            moonLight.enabled = false;

            GameManager.Instance.dayCycleManager.OnDayPhaseChanged += DayPhaseChange;
        }

        private float GetCurrentTime() {
            return GameManager.Instance.dayCycleManager.GetAdjustedTime();
        }

        private void Update() {
            RotateSun();
            RotateMoon();

            if (Input.GetKeyDown(KeyCode.R)) {
                ResetLighting();
            }
        }

        private void DayPhaseChange(object sender, DayCycleManager.OnDayPhaseChangeEventArgs e) {
            // Additional transition logic if needed
        }

        private void RotateSun() {
            float currentTime = GetCurrentTime();

            if (currentTime >= sunriseTime && currentTime < afternoonTime) {
                float t = Mathf.InverseLerp(sunriseTime, afternoonTime, currentTime);
                sunLight.transform.rotation = Quaternion.Slerp(morningRotation, afternoonRotation, t);
                sunLight.colorTemperature = Mathf.Lerp(morningTemp, afternoonTemp, t);
                sunLight.enabled = true;
                moonLight.enabled = false;
            }
            else if (currentTime >= afternoonTime && currentTime < sunsetTime) {
                float t = Mathf.InverseLerp(afternoonTime, sunsetTime, currentTime);
                sunLight.transform.rotation = Quaternion.Slerp(afternoonRotation, sunsetRotation, t);
                sunLight.colorTemperature = Mathf.Lerp(afternoonTemp, sunsetTemp, t);
                sunLight.enabled = true;
            }
            else if (currentTime >= sunsetTime && currentTime < midnightTime) {
                // New logic for sunset to midnight transition
                float t = Mathf.InverseLerp(sunsetTime, midnightTime, currentTime);
                sunLight.transform.rotation = Quaternion.Slerp(sunsetRotation, nightRotation, t);
                sunLight.colorTemperature = Mathf.Lerp(sunsetTemp, nightTemp, t);
                sunLight.enabled = true;
            }
            else {
                sunLight.enabled = false;
            }
        }

        private void RotateMoon() {
            float currentTime = GetCurrentTime();

            if (currentTime >= moonriseTime && currentTime < midnightTime) {
                float t = Mathf.InverseLerp(moonriseTime, midnightTime, currentTime);
                moonLight.transform.rotation = Quaternion.Slerp(moonriseRotation, moonPeakRotation, t);
                moonLight.colorTemperature = Mathf.Lerp(moonriseTemp, moonPeakTemp, t);

                // Blend with sunlight during transition
                moonLight.enabled = true;
                if (currentTime < sunsetTime || currentTime > sunriseTime) {
                    sunLight.enabled = true;
                }
            }
            else if (currentTime >= 0 && currentTime < sunriseTime) {
                float t = Mathf.InverseLerp(0, sunriseTime, currentTime);
                moonLight.transform.rotation = Quaternion.Slerp(moonPeakRotation, moonriseRotation, t);
                moonLight.colorTemperature = Mathf.Lerp(moonPeakTemp, moonriseTemp, t);
                moonLight.enabled = true;
            }
            else {
                moonLight.enabled = false;
            }
        }

        private void ResetLighting() {
            sunLight.transform.rotation = morningRotation;
            sunLight.colorTemperature = morningTemp;
            sunLight.enabled = true;

            moonLight.transform.rotation = moonriseRotation;
            moonLight.colorTemperature = moonriseTemp;
            moonLight.enabled = false;

            Debug.Log("Lighting reset.");
        }
    }
}
