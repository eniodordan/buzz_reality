using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

namespace BuzzReality
{
    public class GameManager : Singleton<GameManager>
    {
        private const int EasyLowerBound = 8;
        private const int EasyUpperBound = 12;
        private const int MediumUpperBound = 16;
        private const int HardUpperBound = 20;
        
        [SerializeField] private List<GameObject> levels;
        [SerializeField] private Transform levelArea;
        [Space(10)]
        [SerializeField] private XRSimpleInteractable controlInteractable;
        [SerializeField] private XRSimpleInteractable entryInteractable;
        [SerializeField] private XRSimpleInteractable exitInteractable;
        [Space(10)]
        [SerializeField] private Material redMaterial;
        [SerializeField] private Material metalMaterial;
        
        private Level _currentLevel;
        private int _currentLevelIndex = -1;

        private bool _isPaused = false;
        private bool _timerIsRunning = false;
        private float _elapsedTime = 0;
        private int _attempts = 0;
        private int _tempAttempts = 0;

        public List<GameObject> Levels => levels;
        public int Attempts => _attempts;
        public float ElapsedTime => _elapsedTime;
        public bool HasInvalidWires => _currentLevel.invalidWiresInteractable.Count != 0;
        public int CurrentLevelIndex => _currentLevelIndex;

        public static event Action OnLevelInstantiated;
        public static event Action OnAttemptsUpdated;
        public static event Action<int, float, int, int> OnLevelCompleted;

        private void OnEnable()
        {
            controlInteractable.hoverEntered.AddListener(OnEnterLevelControl);
            entryInteractable.hoverEntered.AddListener(OnEnterLevelEntry);
            exitInteractable.hoverEntered.AddListener(OnEnterLevelExit);

            SelectMenu.OnLevelSelected += OnLevelSelected;
            PauseMenu.OnHomeClicked += OnHomeClicked;
            PauseMenu.OnResumeClicked += OnResumeClicked;
            CompleteMenu.OnReplayClicked += OnReplayClicked;
            CompleteMenu.OnNextClicked += OnNextClicked;

            DataManager.OnProgressReset += OnProgressReset;
            InputManager.OnPauseButtonPressed += OnPauseButtonPressed;
        }

        private void OnDisable()
        {
            controlInteractable.hoverEntered.RemoveListener(OnEnterLevelControl);
            entryInteractable.hoverEntered.RemoveListener(OnEnterLevelEntry);
            exitInteractable.hoverEntered.RemoveListener(OnEnterLevelExit);

            SelectMenu.OnLevelSelected -= OnLevelSelected;
            PauseMenu.OnHomeClicked -= OnHomeClicked;
            PauseMenu.OnResumeClicked -= OnResumeClicked;
            CompleteMenu.OnReplayClicked -= OnReplayClicked;
            CompleteMenu.OnNextClicked -= OnNextClicked;
            
            DataManager.OnProgressReset -= OnProgressReset;
            InputManager.OnPauseButtonPressed -= OnPauseButtonPressed;
        }

        private void InstantiateLevel(int levelIndex)
        {
            if (_currentLevel != null) return;
            
            var levelObject = Instantiate(levels[levelIndex], levelArea);
            _currentLevel = levelObject.GetComponent<Level>();
            _currentLevelIndex = levelIndex;

            _currentLevel.OnInteractableEntered += OnLevelInteractableEntered;
            _currentLevel.OnInvalidWireEntered += OnLevelInvalidWireEntered;
            _currentLevel.OnStateChanged += OnLevelStateChanged;
            
            levelArea.gameObject.SetActive(true);

            _attempts = 0;
            _elapsedTime = 0;
            OnLevelInstantiated?.Invoke();
        }

        private void DestroyLevel()
        {
            if (_currentLevel == null) return;
            
            _currentLevel.OnInteractableEntered -= OnLevelInteractableEntered;
            _currentLevel.OnInvalidWireEntered -= OnLevelInvalidWireEntered;
            _currentLevel.OnStateChanged -= OnLevelStateChanged;
            
            Destroy(_currentLevel.gameObject);
            _currentLevel = null;
            
            levelArea.gameObject.SetActive(false);
        }

        private void OnEnterLevelControl(HoverEnterEventArgs _)
        {
            if (!_currentLevel.LevelState.Equals(LevelState.INPROGRESS)) return;
            
            _currentLevel.ChangeLevelState(LevelState.UNATTENDED);
        }

        private void OnEnterLevelEntry(HoverEnterEventArgs _)
        {
            if (_currentLevel.LevelState.Equals(LevelState.INPROGRESS)) return;
            
            _currentLevel.ChangeLevelState(LevelState.INPROGRESS);
        }

        private void OnLevelInteractableEntered()
        {
            if (!_currentLevel.LevelState.Equals(LevelState.INPROGRESS)) return;

            _currentLevel.ChangeLevelState(LevelState.FAILED);
            InputManager.Instance.SendHapticImpulse();
        }

        private void OnLevelInvalidWireEntered()
        {
            _attempts++;
            OnAttemptsUpdated?.Invoke();
            
            AudioManager.Instance.PlayLevelFailed();
            InputManager.Instance.SendHapticImpulse();
        }

        private void OnEnterLevelExit(HoverEnterEventArgs _)
        {
            if (!_currentLevel.LevelState.Equals(LevelState.INPROGRESS)) return;
            
            _currentLevel.ChangeLevelState(LevelState.COMPLETED);
        }

        private void OnLevelSelected(int levelIndex)
        {
            InstantiateLevel(levelIndex);
            MenuManager.Instance.ChangeMenuState(MenuState.INGAME_MENU);
        }
        
        private void OnReplayClicked()
        {
            InstantiateLevel(_currentLevelIndex);
            MenuManager.Instance.ChangeMenuState(MenuState.INGAME_MENU);
        }

        private void OnNextClicked()
        {
            if (++_currentLevelIndex < levels.Count)
            {
                InstantiateLevel(_currentLevelIndex);
                MenuManager.Instance.ChangeMenuState(MenuState.INGAME_MENU);
            }
            else
            {
                DestroyLevel();
                MenuManager.Instance.ChangeMenuState(MenuState.SELECT_MENU);
            }
        }

        private void OnHomeClicked()
        {
            _isPaused = false;
            MenuManager.Instance.ChangeMenuState(MenuState.SELECT_MENU);
        }

        private void OnResumeClicked()
        {
            _isPaused = false;
            InstantiateLevel(_currentLevelIndex);
            _attempts = _tempAttempts;
            OnAttemptsUpdated?.Invoke();
            MenuManager.Instance.ChangeMenuState(MenuState.INGAME_MENU);
        }

        private void OnProgressReset()
        {
            _isPaused = false;
            MenuManager.Instance.ChangeMenuState(MenuState.SELECT_MENU);
        }

        private void OnPauseButtonPressed()
        {
            _isPaused = !_isPaused;
            
            if (_isPaused)
            {
                _timerIsRunning = false;
                _tempAttempts = _attempts;
                DestroyLevel();
                MenuManager.Instance.ChangeMenuState(MenuState.PAUSE_MENU);
            }
            else
            {
                InstantiateLevel(_currentLevelIndex);
                _attempts = _tempAttempts;
                OnAttemptsUpdated?.Invoke();
                MenuManager.Instance.ChangeMenuState(MenuState.INGAME_MENU);
            }
        }

        private void OnLevelStateChanged(LevelState levelState)
        {
            switch (levelState)
            {
                case LevelState.INPROGRESS:
                    _elapsedTime = 0;
                    _timerIsRunning = true;
                    _attempts++;
                    OnAttemptsUpdated?.Invoke();
                    _currentLevel.UpdateWireMaterial(metalMaterial);
                    break;
                case LevelState.COMPLETED:
                    AudioManager.Instance.PlayLevelCompleted();
                    _timerIsRunning = false;

                    int rating = 3;
                    switch (_currentLevel.LevelDifficulty)
                    {
                        case LevelDifficulty.EASY:
                            if (_elapsedTime is > EasyLowerBound and <= EasyUpperBound) rating = 2;
                            else if (_elapsedTime > EasyUpperBound) rating = 1;
                            break;
                        case LevelDifficulty.MEDIUM:
                            if (_elapsedTime is > EasyUpperBound and <= MediumUpperBound) rating = 2;
                            else if (_elapsedTime > MediumUpperBound) rating = 1;
                            break;
                        case LevelDifficulty.HARD:
                            if (_elapsedTime is > MediumUpperBound and <= HardUpperBound) rating = 2;
                            else if (_elapsedTime > HardUpperBound) rating = 1;
                            break;
                    }
                    
                    OnLevelCompleted?.Invoke(_currentLevelIndex, _elapsedTime, _attempts, rating);
                    DestroyLevel();
                    MenuManager.Instance.ChangeMenuState(MenuState.COMPLETE_MENU);
                    break;
                case LevelState.FAILED:
                    AudioManager.Instance.PlayLevelFailed();
                    _timerIsRunning = false;
                    _currentLevel.UpdateWireMaterial(redMaterial);
                    break;
                default:
                    _currentLevel.UpdateWireMaterial(metalMaterial);
                    break;
            }
        }

        private void Update()
        {
            if (!_isPaused && _timerIsRunning) _elapsedTime += Time.deltaTime;
        }
    }
}
