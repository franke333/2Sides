using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputField : MonoBehaviour
{
    [SerializeField] private Leaderboard leaderboard;
    public TextMeshProUGUI Input;

    public void Add()
    {
        float score = SettingsManager.Instance.EndTime;

        string name = Input.text;

        if (name == null || name.Length == 0) return;

        if (name.Length > 3)
        {
            name = name.Substring(0, 3);
        }

        leaderboard.AddLeaderboardEntry((int)score, name);
    }
}
