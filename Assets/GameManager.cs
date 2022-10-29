using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TMP_Text scoreText;
    public TMP_Text totalScoreText;
    public GameObject gameEndPanel;
    public int score = 0;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateText()
    {
        scoreText.text = "Shoot Count : " + score.ToString();
        totalScoreText.text = "Finished Score Count : " + score.ToString();
    }

    public void GameOver()
    {
        gameEndPanel.SetActive(true);
    }
}
