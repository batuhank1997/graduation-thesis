using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        score = 0;
        UpdateScore();
    }

    public int IncreaseScore(int amount)
    {
        score += amount;
        UpdateScore();
        return score;
    }

    public int DecreaseScore(int amount)
    {
        score -= amount;
        UpdateScore();
        return score;
    }

    public int UpdateScore()
    {
        scoreText.text = "$" + score.ToString();
        return score;
    }

}
