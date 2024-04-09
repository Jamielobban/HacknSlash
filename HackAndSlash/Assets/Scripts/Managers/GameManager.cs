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
                if (_instance == null)
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

        public bool isTutorialCompleted = false;

        public bool isInMenu = false;


        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            _player = FindObjectOfType<PlayerControl>();
        }

        void Update()
        {

        }

        public void Init()
        {
            UpdateState(Enums.GameState.Menu);
        }

        public void LoadLevel(string _name, Image _progress)
        {
            StartCoroutine(LoadSceneAsync(_name, _progress));
        }

        private IEnumerator LoadSceneAsync(string levelName, Image _progresionBar)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(levelName);
            async.allowSceneActivation = false;
            while (!async.isDone)
            {
                float progress = Mathf.Clamp01(async.progress / .9f);
                _progresionBar.fillAmount = progress;
                if (progress >= 0.9f)
                {
                    _progresionBar.fillAmount = 1;
                    async.allowSceneActivation = true;
                
                }
                yield return null;
            }
            yield return new WaitForSeconds(0.15f);

            _player = FindObjectOfType<PlayerControl>();
            //MainMenu.Hide();
        }

        public void UpdateState(Enums.GameState newState)
        {
            if (state == newState)
            {
                return;
            }

            state = newState;

        switch (state)
        {
            case Enums.GameState.Menu:
                // MainMenu.Show();
                break;
            case Enums.GameState.Tutorial:
                break;
            case Enums.GameState.StartPlaying:
                break;
            case Enums.GameState.Playing:
                break;
            case Enums.GameState.Pause:
                // PauseMenu.Show();
                break;
            case Enums.GameState.ReturningMenu:
                // MainMenu.Show();
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
            if (!isInMenu)
            {
                foreach (var feedback in _player.feedbacksPlayer)
                {
                    feedback.StopFeedbacks();
                }
                _isPaused = !_isPaused;
                Time.timeScale = _isPaused ? 0 : 1;
            }
        }

        public void PauseMenuGame()
        {
            foreach (var feedback in _player.feedbacksPlayer)
            {
                feedback.StopFeedbacks();
            }
            isInMenu = true;
            _isPaused = true;
            Time.timeScale = 0;
        }

        public void UnPauseMenuGame()
        {
            isInMenu = false;
            _isPaused = false;
            Time.timeScale = 1;
        }

        public void SetPauseVariable(bool pause)
        {
            _isPaused = pause;
        }

    }

