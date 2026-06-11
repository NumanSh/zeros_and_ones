using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI timerText; 
    [SerializeField] private float timeRemaining = 60f; 
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
               
            }
        }
    }

   
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (timeToDisplay < 60f)
        {
            timerText.color = Color.red; 
        }
        else
        {
            timerText.color = Color.black; 
        }
    }

    public bool PassChallenge()
    {
        if (isTimerRunning)
        {
            Debug.Log("Challenge Passed!");
            return true;
        }
        Debug.Log("Challenge Failed!");
        return false;
    }
}
