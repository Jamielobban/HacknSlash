using System;
using System.Collections;
using UnityEditor;
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

        public bool _isPaused;
        private PlayerControl _player;
        public PlayerControl Player => GetPlayer();
        public bool IsPaused => _isPaused;

        public bool isTutorialCompleted = false;

        public bool isInMenu = false;

        public float volumeSFX = 27f;
        public float volumeMusic = 27f;

        public PlayerData _data;
        public InventorySO _inventory;
        public bool canLoadItem = false;

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            _player = FindObjectOfType<PlayerControl>();
            _data = Resources.Load("Data/Player Data") as PlayerData;
            _inventory = Resources.Load("Data/Player Inventory") as InventorySO;
            Init();
        }

        void Update()
        {

        }

        public void Init()
        {
            UpdateState(Enums.GameState.Menu);
            canLoadItem = false;
            _data.ResetData();
            _inventory.ClearInventory();
    }

        public void LoadLevel(string _name, Image _progress)
        {
            StartCoroutine(LoadSceneAsync(_name, _progress));
        }

        private IEnumerator LoadSceneAsync(string levelName, Image _progresionBar)
        {
            AudioManager.Instance.audioMusicEffects.Stop();            
            AudioManager.Instance.PlayMusic(Enums.Music.LoadingFX);
            AsyncOperation async = SceneManager.LoadSceneAsync(levelName);
            async.allowSceneActivation = false;
            while (!async.isDone)
            {
                float progress = Mathf.Clamp01(async.progress / .9f);
                _progresionBar.fillAmount = progress;
                if (progress >= 0.9f)
                {
                    AudioManager.Instance.audioMusic.Stop();
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

            switch (newState)
            {
                case Enums.GameState.Menu:
                    canLoadItem = false;
                    _data.ResetData();
                    _inventory.ClearInventory();
                    break;
                case Enums.GameState.Tutorial:
                    canLoadItem = false;
                    break;
                case Enums.GameState.StartPlaying:
                    break;
                case Enums.GameState.Playing:
                    canLoadItem = true;
                    break;
                case Enums.GameState.Pause:

                    break;
                case Enums.GameState.ReturningMenu:
                    break;
                case Enums.GameState.Exit:
                    break;
                default:
                    break;
            }
            state = newState;

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

        public PlayerControl GetPlayer()
        {
            if(_player == null)
            {
                _player = FindObjectOfType<PlayerControl>();
            }

            return _player;
        }
}

