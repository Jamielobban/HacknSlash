using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    public int level;
    private float _currentXp;
    public Image fillXpBar;
    public TextMeshProUGUI xpText;

    private void UpdateHud() => fillXpBar.fillAmount = Mathf.Clamp01(_currentXp / GetNextLevelXp());

    public void GainXp(float val)
    {
        _currentXp += val * GameManager.Instance.Player.xpPercentageExtra;
        UpdateHud();
        if (_currentXp >= GetNextLevelXp())
        {            
            NextLevel();
        }
    }

    private void NextLevel()
    {
        _currentXp -= GetNextLevelXp();
        level++;
        ItemsLootBoxManager.Instance.ShowNewOptions();
        if (_currentXp >= GetNextLevelXp())
        {
            NextLevel();
        }
        xpText.text = "" + level;
        fillXpBar.fillAmount = 0;
    }

    public float GetNextLevelXp()
    {
        int nextLevel = level + 1;

        if (nextLevel <= 20) 
          return (nextLevel * 10) - 5;

        if (nextLevel > 20 && nextLevel <= 40) 
           return (nextLevel * 13) - 6;

        if (nextLevel > 40) 
          return  (nextLevel * 16) - 8;

        return 0;
    }
}
