using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDSystem : MonoBehaviour
{
    public Image progressBar;
    public Image progressScoreBar;
    private int _propId = Shader.PropertyToID("_ProgressBar");

    private void Awake()
    {
        progressBar.material.SetFloat(_propId, 1);
        ResetProgressScoreBar();
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        progressBar.material.SetFloat(_propId, Mathf.Clamp(currentHealth / maxHealth, 0, 1));
    }

    public void UpdateProgressScoreBar(float currentScore, float maxScore)
    {
        progressScoreBar.material.SetFloat(_propId, Mathf.Clamp(currentScore / maxScore, 0, 1));
    }
    public void ResetProgressScoreBar() => progressScoreBar.material.SetFloat(_propId, 0);
}
