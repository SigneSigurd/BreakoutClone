using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI score_txt;

    private int score = 0;

    /// <summary>
    /// Add score to current score
    /// </summary>
    /// <param name="points"> Amount of points to add to the current score </param>
    public void AddScore(int points)
    {
        score += points;

        score_txt.text = "Score: " + score.ToString();
    }
}
