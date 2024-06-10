using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Leaderboard : SingletonClass<Leaderboard>
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> leaderboardEntryTransformList;

    new public void Awake()
    {
        entryContainer = transform.Find("LineContainer");
        entryTemplate = entryContainer.Find("OneLine");
        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("leaderboard");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            highscores = new Highscores();
        }
        else
        {
            for (int i = 1; i < entryContainer.childCount; i++)
            {
                Destroy(entryContainer.GetChild(i).gameObject);
            }
        }

        if (highscores.leaderboardEntryList == null)
        {
            highscores.leaderboardEntryList = new List<LeaderboardEntry>();
        }

        // Sort entry list by Score
        for (int i = 0; i < highscores.leaderboardEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.leaderboardEntryList.Count; j++)
            {
                if (highscores.leaderboardEntryList[j].score > highscores.leaderboardEntryList[i].score)
                {
                    // Swap
                    LeaderboardEntry tmp = highscores.leaderboardEntryList[i];
                    highscores.leaderboardEntryList[i] = highscores.leaderboardEntryList[j];
                    highscores.leaderboardEntryList[j] = tmp;
                }
            }
        }

        if (highscores.leaderboardEntryList.Count > 10)
        {
            for (int h = highscores.leaderboardEntryList.Count; h > 10; h--)
            {
                highscores.leaderboardEntryList.RemoveAt(10);
            }
        }

        leaderboardEntryTransformList = new List<Transform>();
        foreach (LeaderboardEntry leaderboardEntry in highscores.leaderboardEntryList)
        {
            CreateLeaderboardEntryTransform(leaderboardEntry, entryContainer, leaderboardEntryTransformList);
        }
    }

    private void CreateLeaderboardEntryTransform(LeaderboardEntry LeaderboardEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 50f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            case 1:
                rankString = "1ST"; break;
            case 2:
                rankString = "2ND"; break;
            case 3:
                rankString = "3RD"; break;
            default:
                rankString = rank + "TH"; break;
        }
        entryTransform.Find("Position").GetComponent<TextMeshProUGUI>().text = rankString;

        int score = LeaderboardEntry.score;
        int min = Mathf.FloorToInt(score / 60);
        int sec = Mathf.FloorToInt(score % 60);
        entryTransform.Find("Score").GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", min, sec);

        string name = LeaderboardEntry.name;
        entryTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = name;

        // Set background visible odds and evens, easier to read
        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);

        if (rank == 1)
        {
            // highlight first
            entryTransform.Find("Background").GetComponent<Image>().color = new Color32(23, 230, 91, 255);
            entryTransform.Find("Position").GetComponent<TextMeshProUGUI>().color = Color.white;
            entryTransform.Find("Score").GetComponent<TextMeshProUGUI>().color = Color.white;
            entryTransform.Find("Name").GetComponent<TextMeshProUGUI>().color = Color.white;
        }

        // Set trophy
        switch (rank)
        {
            case 1:
                entryTransform.Find("trophy").GetComponent<Image>().color = new Color32(255, 210, 0, 255);
                break;
            case 2:
                entryTransform.Find("trophy").GetComponent<Image>().color = new Color32(198, 198, 198, 255);
                break;
            case 3:
                entryTransform.Find("trophy").GetComponent<Image>().color = new Color32(183, 111, 86, 255);
                break;
            default:
                entryTransform.Find("trophy").gameObject.SetActive(false);
                break;
        }

        transformList.Add(entryTransform);
    }

    public void AddLeaderboardEntry(int score, string name)
    {
        // Create LeaderboardEntry
        LeaderboardEntry leaderboardEntry = new LeaderboardEntry { score = score, name = name };

        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("leaderboard");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            highscores = new Highscores();
        }
        if (highscores.leaderboardEntryList == null)
        {
            highscores.leaderboardEntryList = new List<LeaderboardEntry>();
        }

        // Add new entry to Highscores
        highscores.leaderboardEntryList.Add(leaderboardEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("leaderboard", json);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<LeaderboardEntry> leaderboardEntryList;
    }

    /// <summary>
    /// Represents a single Leaderboard entry
    /// </summary>
    [System.Serializable]
    private class LeaderboardEntry
    {
        public int score;
        public string name;
    }
}

