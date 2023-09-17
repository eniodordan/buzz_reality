using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace BuzzReality
{
    public class SelectMenu : MonoBehaviour
    {
        [SerializeField] private GameObject prefabLevelButton;
        [SerializeField] private Transform transformLevelButtons;
        [Space(10)]
        [SerializeField] private Button buttonBack;
        [SerializeField] private Button buttonRandom;

        private List<LevelButton> _levelButtons = new();

        public static event Action<int> OnLevelSelected;

        private void OnEnable()
        {
            buttonBack.onClick.AddListener(OnClickBack);
            buttonRandom.onClick.AddListener(OnClickRandom);

            DataManager.OnDataUpdated += OnDataUpdated;
            DataManager.OnProgressReset += OnDataUpdated;
        }

        private void OnDisable()
        {
            buttonBack.onClick.RemoveListener(OnClickBack);
            buttonRandom.onClick.RemoveListener(OnClickRandom);
            
            DataManager.OnDataUpdated -= OnDataUpdated;
            DataManager.OnProgressReset -= OnDataUpdated;
            
            foreach (var btn in _levelButtons) btn.OnLevelClicked -= OnLevelClicked;
        }

        private void Start()
        {
            for (int i = 0; i < GameManager.Instance.Levels.Count; i++)
            {
                var buttonObject = Instantiate(prefabLevelButton, transformLevelButtons);
                var levelButton = buttonObject.GetComponent<LevelButton>();
                
                levelButton.UpdateLevel(i);
                if (i > 0)
                {
                    var isPrevCompleted = DataManager.Instance.IsLevelCompleted(i - 1);
                    levelButton.Lock(!isPrevCompleted);
                    levelButton.HideRating(!isPrevCompleted);
                }
                levelButton.UpdateRating(DataManager.Instance.GetLevelRating(i));

                levelButton.OnLevelClicked += OnLevelClicked;
                
                _levelButtons.Add(levelButton);
            }
        }

        private void OnClickBack()
        {
            MenuManager.Instance.ChangeMenuState(MenuState.MAIN_MENU);
        }

        private void OnClickRandom()
        {
            var random = Random.Range(0, _levelButtons.Count(lvl => !lvl.IsLocked) - 1);
            OnLevelSelected?.Invoke(random);
        }

        private void OnDataUpdated()
        {
            for (int i = 0; i < _levelButtons.Count; i++)
            {
                _levelButtons[i].UpdateRating(DataManager.Instance.GetLevelRating(i));
                
                if (i < 1) continue;
                var isPrevCompleted = DataManager.Instance.IsLevelCompleted(i - 1);
                _levelButtons[i].Lock(!isPrevCompleted);
                _levelButtons[i].HideRating(!isPrevCompleted);
            }
        }
        
        private void OnLevelClicked(int levelIndex) => OnLevelSelected?.Invoke(levelIndex);
    }
}