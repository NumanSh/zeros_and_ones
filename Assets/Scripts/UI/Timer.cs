using UnityEngine;
using TMPro; // خطوة إجبارية لتشغيل الـ TextMeshPro

public class Timer : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI timerText; // سحب عنصر النص هنا
    [SerializeField] private float timeRemaining = 60f; // وقت العداد بالثواني
    private bool isTimerRunning = true;

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;
                // يمكنك هنا إضافة أي حدث عند انتهاء الوقت (مثل Game Over)
            }
        }
    }

    // دالة لتحويل الثواني إلى صيغة دقائق وثواني وعرضها
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // تحديث النص في الواجهة ليظهر بالشكل 00:00
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
