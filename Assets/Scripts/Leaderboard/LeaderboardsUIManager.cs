using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class LeaderboardsUIManager : MonoBehaviour
{
    public InputField scoreInputField;
    public InputField playerNameField;
    public Text leaderboardText;
    public Button addScoreButton;
    public LeaderboardsManager leaderboardsManager;

    void Start()
    {
        addScoreButton.onClick.AddListener(OnAddScoreButtonClicked);
    }

    void OnAddScoreButtonClicked()
    {
        int score;
        if (int.TryParse(scoreInputField.text, out score))
        {
            leaderboardsManager.AddScore(score);
        }
        else
        {
            Debug.LogError("Invalid score input.");
        }
    }

    public void AddScore()
    {
        string playerName;
        if (string.IsNullOrEmpty(playerNameField.text))
        {
            playerName = "Anonymous";
        }
        else
        {
            playerName = playerNameField.text;
        }

        int score;
        if (int.TryParse(scoreInputField.text, out score))
        {
            leaderboardsManager.AddScore(playerName, score);
        }
        else
        {
            Debug.LogError("Invalid score input.");
        }
    }

    public void GetTopScores(int limit)
    {
        leaderboardsManager.GetTopScores(limit);
    }
}
