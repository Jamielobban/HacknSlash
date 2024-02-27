using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManagerEnemies : MonoBehaviour
{
    private static ManagerEnemies _instance;

    public static ManagerEnemies Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("Enemies Manager");
                go.AddComponent<ManagerEnemies>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    //-//-//-//-//-//
    // Timer -> every X time object with chance by enemiesKilled and enemiesPoints acumulated in the time period
    // Spawning enemies all the time each X seg, with enemies cap
    // If enemies are to far from player deactive and respawn
    //-//-//-//-//-//
    private float _enemiesKilled = 0;
    private float _enemiesScore = 0;
    private float _timer = 0f;

    public int timeToGetItem = 2;

    public TextMeshProUGUI textTime;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        _timer += Time.deltaTime;

        textTime.text = _timer.ToString();

        if(_timer >= timeToGetItem)
        {
            _enemiesScore = 0;
            _enemiesKilled = 0;
            timeToGetItem += 2;
        }
    }

    public void AddEnemyScore(float val) => _enemiesScore += val;
    public void AddEnemyKilled() => _enemiesKilled++;
}
