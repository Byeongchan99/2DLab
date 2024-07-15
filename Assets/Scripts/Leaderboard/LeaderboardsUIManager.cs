using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Unity.VisualScripting;
using System.Collections.Generic;
using System;
using Unity.Services.Leaderboards.Models;

public class LeaderboardsUIManager : MonoBehaviour
{
    public InputField playerNameField;
    public InputField scoreInputField;
    public Button addScoreButton;
    public LeaderboardsManager leaderboardsManager;

    // UI 요소를 참조할 필드 추가
    public GameObject leaderboardEntryPrefab; // 리더보드 엔트리 프리팹
    public Transform leaderboardContent; // 리더보드 항목을 배치할 부모 오브젝트

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

    public void DisplayScores(LeaderboardScoresPage scoresResponse)
    {
        foreach (var scoreEntry in scoresResponse.Results)
        {
            string playerName = scoreEntry.PlayerName;
            if (!string.IsNullOrEmpty(scoreEntry.Metadata))
            {
                try
                {
                    var metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(scoreEntry.Metadata);
                    if (metadata != null && metadata.ContainsKey("PlayerName"))
                    {
                        playerName = metadata["PlayerName"];
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to parse metadata: {ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning("Metadata is empty.");
            }

            // 새로운 리더보드 엔트리 생성 및 설정
            GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContent);
            entry.transform.Find("Text PlayerId").GetComponent<Text>().text = scoreEntry.PlayerId;
            entry.transform.Find("Text PlayerName").GetComponent<Text>().text = playerName;
            entry.transform.Find("Text Rank").GetComponent<Text>().text = scoreEntry.Rank.ToString();
            entry.transform.Find("Text Score").GetComponent<Text>().text = scoreEntry.Score.ToString();
            entry.transform.Find("Text Tier").GetComponent<Text>().text = scoreEntry.Tier.ToString();
        }
    }
}
