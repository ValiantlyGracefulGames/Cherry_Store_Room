using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // If using TextMeshPro for UI
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [Header("Score & Timer")]
    public int currentScore = 0;
    public TMP_Text scoreText;      // Assign in inspector
    public TMP_Text timerText;      // Assign in inspector
    public TMP_Text highScoreText;  // Assign in inspector (optional)

    [Header("Streak UI")]
    public TMP_Text streakText;
    public float streakMessageDuration = 1f; // seconds

    private Coroutine streakCoroutine;

    [Header("Streak UI Flash")]
    public Color streakFlashColor = Color.cyan;  // Light cyan for pop
    public float flashDuration = 0.8f;           // Total flash time
    public float maxScale = 1.5f;

    [Header("End Game UI")]
    public GameObject endGamePanel;
    public TMP_Text finalScoreText;
    public TMP_Text punText;
    public TMP_Text restartText;

    [Header("End Game Messages")]
    public string[] iceCreamPuns = {
        "What's your favorite ice cream flavor?",
        "You are the true cherry on top!",
        "Sundae Funday is over!",
        "Cone-gratulations, you delivered like a pro!",
        "Life is better with sprinkles!"  
    };

    public float gameTime = 300f;   // 5 minutes
    private float remainingTime;

    private Flavor? lastFlavor = null;
    private int flavorStreak = 0;
    private bool isGameOver = false;

    void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        remainingTime = gameTime;
    }

    void Start()
    {
        UpdateScoreUI();
        UpdateTimerUI();
        UpdateHighScoreUI();

        if (endGamePanel != null) endGamePanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver)
        {
            // Restart game on R
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1f;
            return;
        }

        // Countdown timer
        remainingTime -= Time.deltaTime;
        UpdateTimerUI();

        if (remainingTime <= 0)
            TriggerGameOver();
    }

    public float GetDifficultyMultiplier()
    {
        float t = 1f - (remainingTime / gameTime);
        return Mathf.Lerp(1f, 2f, t);
    }

    // Add points and update UI
    public void AddScore(int points)
    {
        if (isGameOver) return;

        currentScore += points;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {currentScore}";
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
    public void OnTubDelivery(Flavor flavor)
    {
        if (lastFlavor == flavor)
            flavorStreak++;
        else
            flavorStreak = 1;

        lastFlavor = flavor;

        int points = 10;

        if (flavorStreak == 3)
        {
            points += 25;
            ShowStreakMessage("FLAVOR STREAK BONUS! +25");

            flavorStreak = 0;
            lastFlavor = null;
        }
    }
    public void ShowStreakMessage(string message)
    {
        if (streakText == null) return;

        // Stop any existing animation
        StopAllCoroutines();

        streakText.text = message;
        StartCoroutine(FlashStreakText());
    }
    private IEnumerator FlashStreakText()
    {
        float halfDuration = flashDuration / 2f;

        Vector3 originalScale = streakText.transform.localScale;
        Color originalColor = streakText.color;

        // Phase 1: grow and change color
        float timer = 0f;
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float t = timer / halfDuration;

            streakText.transform.localScale = Vector3.Lerp(originalScale, originalScale * maxScale, t);
            streakText.color = Color.Lerp(originalColor, streakFlashColor, t);

            yield return null;
        }

        // Phase 2: shrink back and fade color
        timer = 0f;
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float t = timer / halfDuration;

            streakText.transform.localScale = Vector3.Lerp(originalScale * maxScale, originalScale, t);
            streakText.color = Color.Lerp(streakFlashColor, originalColor, t);

            yield return null;
        }

        streakText.text = "";
        streakText.transform.localScale = originalScale;
        streakText.color = originalColor;
    }

    public void ResetStreak()
    {
        flavorStreak = 0;
        lastFlavor = null;
    }

    void UpdateHighScoreUI()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreText != null)
            highScoreText.text = $"High Score: {highScore}";
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        // Save high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();
            highScore = currentScore;
        }

        // Update UI
        UpdateHighScoreUI();

        if (endGamePanel != null) endGamePanel.SetActive(true);
        if (finalScoreText != null) finalScoreText.text = $"Score: {currentScore}";
        if (punText != null && iceCreamPuns.Length > 0)
            punText.text = iceCreamPuns[Random.Range(0, iceCreamPuns.Length)];
        if (restartText != null)
            restartText.text = "Press R to Restart!";

        Time.timeScale = 0f;

    }
}
