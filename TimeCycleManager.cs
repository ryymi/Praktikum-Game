    using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeCycleManager : MonoBehaviour
{
    public string YouWinMenu = "YouWin";

    [Header("Sun Rotation Settings")]
    public Light sunLight;
    public float rotationStartAngle = -90f; // Matahari terbit
    public float rotationEndAngle = 90f;    // Matahari terbenam

    [Header("Time Settings")]
    public float totalDayDuration = 60f; // 10 menit real-time
    private float currentTime = 0f;

    [Header("UI Settings")]
    public Slider timeSlider;
    public Text timeText;

    private bool gameEnded = false;

    void Start()
    {
        currentTime = 0f;
        if (timeSlider != null)
        {
            timeSlider.maxValue = totalDayDuration;
            timeSlider.value = 0;
        }
    }

    void Update()
    {
        if (gameEnded) return;

        currentTime += Time.deltaTime;

        // Update UI
        if (timeSlider != null)
            timeSlider.value = currentTime;

        if (timeText != null)
        {
            float inGameHours = Mathf.Lerp(0, 24, currentTime / totalDayDuration);
            int hour = Mathf.FloorToInt(inGameHours);
            int minutes = Mathf.FloorToInt((inGameHours - hour) * 60);
            timeText.text = string.Format("{0:00}:{1:00}", hour, minutes);
        }

        if (sunLight != null)
        {
            float t = currentTime / totalDayDuration;
            float sunAngle = Mathf.Lerp(rotationStartAngle, rotationEndAngle, t);
            sunLight.transform.rotation = Quaternion.Euler(sunAngle, 0f, 0f);
        }

        // Win condition
        if (currentTime >= totalDayDuration)
        {
            gameEnded = true;
            GameWin();
        }
    }

    void GameWin()
    {
        Debug.Log("You survived!");
        SceneManager.LoadScene(YouWinMenu);
    }
}
