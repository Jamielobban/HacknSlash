using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI currentScoreText;
    public float timeToStartResetScore;
    public float timeToRemoveScore;
    public float removeScore = 10;
    public int maxScore;

    private float _enemiesScore = 0;
    private float lastTimeScored;
    private float _curretTime;

    void Update()
    {
        if ((Time.time - lastTimeScored) >= timeToStartResetScore)
        {
            _curretTime += Time.deltaTime;
            if(_curretTime >= timeToRemoveScore)
            {
                AddEnemyScore(-10);
                _curretTime = 0;
            }
        }
    }

    public bool IsMaxScore() => _enemiesScore >= maxScore;
    public void UpdateScore()
    {
        currentScoreText.text = "Score: " + _enemiesScore;
        GameManager.Instance.Player.hud.UpdateProgressScoreBar(_enemiesScore, maxScore);
    }
    public void ResetScore()
    {
        _enemiesScore = 0;
        GameManager.Instance.Player.hud.ResetProgressScoreBar();
        UpdateScore();
    }

    public void AddEnemyScore(float val)
    {
        lastTimeScored = Time.time;
        _enemiesScore += val;
        if (_enemiesScore <= 0)
            _enemiesScore = 0;
        UpdateScore();
    }

}
