using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI finalScoreText;

    private float distance;
    private int highscore;
    private int coinCount;
    private bool isGameActive = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        highscore = PlayerPrefs.GetInt("Highscore", 0);
        UpdateHighscoreUI();

        distance = 0f;
        coinCount = 0;
        UpdateCoinUI();
        isGameActive = true;
    }

    void Update()
    {
        if (!isGameActive) return;

        distance += GroundSpawner.moveSpeed * Time.deltaTime;
        UpdateDistanceUI();

        int currentDistanceInt = Mathf.FloorToInt(distance);
        if (currentDistanceInt > highscore)
        {
            highscore = currentDistanceInt;
            PlayerPrefs.SetInt("Highscore", highscore);
            UpdateHighscoreUI();
        }
    }

    void UpdateDistanceUI()
    {
        if (distanceText != null)
            distanceText.text = "Distance: " + Mathf.FloorToInt(distance) + " m";
    }

    void UpdateHighscoreUI()
    {
        if (highscoreText != null)
            highscoreText.text = "Best: " + highscore + " m";
    }

    public void AddCoin()
    {
        coinCount++;
        UpdateCoinUI();
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coin: " + coinCount;
    }

    public void GameOver()
    {
        isGameActive = false;

        if (finalScoreText != null)
            finalScoreText.text = "Score: " + Mathf.FloorToInt(distance) + " m\nCoin: " + coinCount;
    }

    public void Restart()
    {
        isGameActive = true;
        distance = 0f;
        coinCount = 0;
        UpdateDistanceUI();
        UpdateCoinUI();
    }
}