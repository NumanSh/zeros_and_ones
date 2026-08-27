using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI timerText; 
    [SerializeField] private float timeRemaining = 60f; 
    private bool isComplete = true;


    public bool GetIsComplete()
    {
        return isComplete;
    }

    void Update()
    {
        if (isComplete)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                isComplete = false;
               
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

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    /// <summary>
    /// Cheat menu only: puts time back on the clock so a puzzle can be demonstrated without
    /// racing it. Compiled out of release builds along with the rest of the cheat system.
    /// </summary>
    public void CheatRefill(float seconds)
    {
        timeRemaining = seconds;
        isComplete = true;

        if (timerText != null)
        {
            DisplayTime(timeRemaining);
        }
    }
#endif

    public bool PassChallenge()
    {
        if (isComplete)
        {
            Debug.Log("Challenge Passed!");
            return true;
        }
        Debug.Log("Challenge Failed!");
        return false;
    }
}
