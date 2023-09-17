using TMPro;
using I2.Loc;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BuzzReality
{
    public class CompleteMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textLevel;
        [SerializeField] private TextMeshProUGUI textAttempts;
        [SerializeField] private TextMeshProUGUI textElapsedTime;
        [Space(10)]
        [SerializeField] private Sprite emptyStar;
        [SerializeField] private Sprite filledStar;
        [SerializeField] private List<Image> ratingStars;
        [Space(10)]
        [SerializeField] private Button buttonHome;
        [SerializeField] private Button buttonReplay;
        [SerializeField] private Button buttonNext;
        
        public static event Action OnReplayClicked;
        public static event Action OnNextClicked;

        private void OnEnable()
        {
            buttonHome.onClick.AddListener(OnClickHome);
            buttonReplay.onClick.AddListener(OnClickReplay);
            buttonNext.onClick.AddListener(OnClickNext);

            GameManager.OnLevelCompleted += OnLevelCompleted;
        }

        private void OnDisable()
        {
            buttonHome.onClick.RemoveListener(OnClickHome);
            buttonReplay.onClick.RemoveListener(OnClickReplay);
            buttonNext.onClick.RemoveListener(OnClickNext);
            
            GameManager.OnLevelCompleted -= OnLevelCompleted;
        }

        private void OnClickHome() => MenuManager.Instance.ChangeMenuState(MenuState.SELECT_MENU);
        private void OnClickReplay() => OnReplayClicked?.Invoke();
        private void OnClickNext() => OnNextClicked?.Invoke();

        private void OnLevelCompleted(int levelIndex, float elapsedTime, int attempts, int rating)
        {
            textLevel.text = LocalizationManager.GetTranslation(ScriptTerms.Level_Completed) + " " + (levelIndex + 1);
            textAttempts.text = LocalizationManager.GetTranslation(ScriptTerms.Attempts) + ": " + attempts;
            textElapsedTime.text = LocalizationManager.GetTranslation(ScriptTerms.Elapsed_Time) + ": " + elapsedTime.ToString("F0") + " s";
            
            for (int i = 0; i < ratingStars.Count; i++)
            {
                ratingStars[i].sprite = i < rating ? filledStar : emptyStar;
            }
        }
    }
}
