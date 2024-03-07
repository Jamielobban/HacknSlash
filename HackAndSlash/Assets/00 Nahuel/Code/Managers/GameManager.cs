using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("Game Manager");
                go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }

    }

    public Enums.GameState state;

    private bool _isPaused;
    private PlayerControl _player;
    public PlayerControl Player => _player;
    public bool IsPaused => _isPaused;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        _player = FindObjectOfType<PlayerControl>();
    }

    void Update()
    {
        
    }

    public void Init()
    {
        UpdateState(Enums.GameState.Menu);
    }

    public void LoadLevel(string _name, GameObject _loader, Image _progress)
    {
        StartCoroutine(LoadSceneAsync(_name, _loader, _progress));
    }

    private IEnumerator LoadSceneAsync(string levelName, GameObject _loaderCanvas, Image _progresionBar)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(levelName);
        async.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);
        float progress = 0f;
        _progresionBar.fillAmount = progress;
        while(!async.isDone)
        {
            _progresionBar.fillAmount = progress;
            if(progress >= 0.9f)
            {
                _progresionBar.fillAmount = 1;
                async.allowSceneActivation = true;
            }
            progress = async.progress;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        UpdateState(Enums.GameState.Playing);
        if(_player == null)
        {
            _player = FindObjectOfType<PlayerControl>();
        }
        //MainMenu.Hide();
    }

    public void UpdateState(Enums.GameState newState)
    {
        if(state == newState)
        {
            return;
        }

        state = newState;

        switch (state)
        {
            case Enums.GameState.Menu:
                MainMenu.Show();
                break;
            case Enums.GameState.StartPlaying:
                break;
            case Enums.GameState.Playing:
                break;
            case Enums.GameState.Pause:
                PauseMenu.Show();
                break;
            case Enums.GameState.ReturningMenu:
                MainMenu.Show();
                break;
            case Enums.GameState.Exit:
                Application.Quit();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
    private bool GuardPlayer() => _player != null ? true : false;

    public void PauseGame()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0 : 1;
    }

    public void SetPauseVariable(bool pause)
    {
        _isPaused = pause;
    }

}
