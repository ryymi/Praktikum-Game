using System;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    [SerializeField] private float timeMultiplier = 2000f;
    [SerializeField] private float startHour = 12f;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private Light sunLight;
    [SerializeField] private Light moonLight;
    [SerializeField] private float maxSunIntensity = 1f;
    [SerializeField] private float maxMoonIntensity = 0.5f;

    [SerializeField] private Color daytimeAmbientLight;
    [SerializeField] private Color nighttimeAmbientLight;
    [SerializeField] private AnimationCurve lightChangeCurve;

    [SerializeField] private float sunriseHour = 7f;
    [SerializeField] private float sunsetHour = 20.5f;

    private DateTime currentTime;

    private void Start()
    {
        currentTime = DateTime.Now.Date.Add(TimeSpan.FromHours(startHour));
        UpdateLightSettings();
    }

    private void Update()
    {
        UpdateTimeOfDay();
        UpdateLightSettings();
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        if (currentTime.Hour >= 24)
        {
            currentTime = currentTime.AddDays(1).AddHours(-24);
        }

        if (timeText != null)
        {
            string label = GetTimeLabel(currentTime.Hour);
            timeText.text = $"{currentTime:HH:mm} - {label}";
        }

        RotateSun();
    }

    private string GetTimeLabel(int hour)
    {
        float currentHour = (float)hour;

        if (currentHour >= sunriseHour && currentHour < 11f)
        {
            return "Pagi";
        }
        else if (currentHour >= 11f && currentHour < 15f)
        {
            return "Siang";
        }
        else if (currentHour >= 15f && currentHour < sunsetHour)
        {
            return "Sore";
        }
        else
        {
            return "Malam";
        }
    }

    private void RotateSun()
    {
        float sunRotation = 0f;

        TimeSpan sunriseTime = TimeSpan.FromHours(sunriseHour);
        TimeSpan sunsetTime = TimeSpan.FromHours(sunsetHour);
        TimeSpan currentTimeSpan = currentTime.TimeOfDay;

        if (currentTimeSpan >= sunriseTime && currentTimeSpan <= sunsetTime)
        {
            TimeSpan totalDayTime = sunsetTime - sunriseTime;
            TimeSpan timeSinceSunrise = currentTimeSpan - sunriseTime;
            float percentageOfDay = (float)timeSinceSunrise.TotalMinutes / (float)totalDayTime.TotalMinutes;
            sunRotation = Mathf.Lerp(0, 180, percentageOfDay);
        }
        else
        {
            TimeSpan totalNightTime = (TimeSpan.FromHours(24) - sunsetTime) + sunriseTime;
            TimeSpan timeSinceSunset = currentTimeSpan >= sunsetTime
                ? currentTimeSpan - sunsetTime
                : currentTimeSpan + (TimeSpan.FromHours(24) - sunsetTime);
            float percentageOfNight = (float)timeSinceSunset.TotalMinutes / (float)totalNightTime.TotalMinutes;
            sunRotation = Mathf.Lerp(180, 360, percentageOfNight);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunRotation, Vector3.right);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        float curveValue = lightChangeCurve.Evaluate(dotProduct);

        float sunIntensity = Mathf.Lerp(0, maxSunIntensity, curveValue);
        float moonIntensity = Mathf.Lerp(maxMoonIntensity, 0, curveValue);

        sunLight.intensity = sunIntensity;
        moonLight.intensity = moonIntensity;

        RenderSettings.ambientLight = Color.Lerp(nighttimeAmbientLight, daytimeAmbientLight, curveValue);

        // Warna moonlight menjadi lebih kebiruan saat malam
        moonLight.color = Color.Lerp(new Color(0.5f, 0.5f, 1f), Color.white, curveValue);
    }
}
