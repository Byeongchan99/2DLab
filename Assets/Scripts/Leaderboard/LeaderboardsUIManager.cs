using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class LeaderboardsUIManager : MonoBehaviour
{
    public InputField scoreInputField;
    public Text leaderboardText;
    public Button addScoreButton;
    public LeaderboardsSample leaderboardsSample;

    void Start()
    {
        addScoreButton.onClick.AddListener(OnAddScoreButtonClicked);
    }

    void OnAddScoreButtonClicked()
    {
        int score;
        if (int.TryParse(scoreInputField.text, out score))
        {
            leaderboardsSample.AddScore(score);
        }
        else
        {
            Debug.LogError("Invalid score input.");
        }
    }
}
