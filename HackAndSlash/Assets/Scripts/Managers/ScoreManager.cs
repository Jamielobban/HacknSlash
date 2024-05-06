using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI multiplierText;
    public float timeToStartResetScore;
    public float removeScore = 10;

    private int maxScore = 100;
    private float _enemiesScore = 0;
    private float lastTimeScored;
    private PlayerControl _player;

    private void Awake()
    {
        _player = GameManager.Instance.Player;
    }

    void Update()
    {
        if ((Time.time - lastTimeScored) >= timeToStartResetScore)
        {
            _enemiesScore -= removeScore * Time.deltaTime;

            _enemiesScore = Mathf.Clamp(_enemiesScore, 0, maxScore);

            UpdateScore();
        }

        if(_enemiesScore < 25 && _player.multiplierCombos != 1)
        {
            multiplierText.text = "";
            _player.multiplierCombos = 1;

        }
        else if(_enemiesScore >= 25 && _enemiesScore < 50 && _player.multiplierCombos != 1.25f)
        {
            multiplierText.text = "x1.25";
            _player.multiplierCombos = 1.25f;

        }
        else if(_enemiesScore >= 50 && _enemiesScore < 75 && _player.multiplierCombos != 1.5f)
        {
            multiplierText.text = "x1.5";
            _player.multiplierCombos = 1.5f;
        }
        else if(_enemiesScore >= 75 && _enemiesScore < 100 && _player.multiplierCombos != 1.75f)
        {
            multiplierText.text = "x1.75";
            _player.multiplierCombos = 1.75f;
        }
        else if(_enemiesScore >= 100 && _player.multiplierCombos != 2f)
        {
            multiplierText.text = "x2";
            GameManager.Instance.Player.multiplierCombos = 2;
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

        if (_enemiesScore >= maxScore)
            _enemiesScore = maxScore;
        UpdateScore();
    }

}
