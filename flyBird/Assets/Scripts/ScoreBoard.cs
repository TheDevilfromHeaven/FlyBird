using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    public Text scoreText;
    public Text bestScoreText;
    public Text medalText;

    private int scoreThisTime = 0;
    private int bestScore = 0;

    public void SetScore(int score)
    {
        scoreThisTime = score;
        scoreText.text = score.ToString();
    }

    public void SetBestScore(int score)
    {
        bestScore = score;
        bestScoreText.text = score.ToString();
    }

    public void CreateRating()
    {
        if(scoreThisTime < 3)
        {
            medalText.text = "D";
        }else if(scoreThisTime >= 3 && scoreThisTime < 5)
        {
            medalText.text = "C";
        }else if(scoreThisTime >= 5 && scoreThisTime < 10)
        {
            medalText.text = "B";
        }else if(scoreThisTime >= 10 && scoreThisTime <= bestScore)
        {
            medalText.text = "A";
        }else if(scoreThisTime >= 10 && scoreThisTime > bestScore)
        {
            medalText.text = "S";
        }
    }
}
