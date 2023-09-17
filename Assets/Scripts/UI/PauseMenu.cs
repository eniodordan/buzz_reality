using TMPro;
using I2.Loc;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BuzzReality
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textLevel;
        [Space(10)]
        [SerializeField] private Button buttonHome;
        [SerializeField] private Button buttonSettings;
        [SerializeField] private Button buttonResume;
        
        public static event Action OnHomeClicked;
        public static event Action OnResumeClicked;

        private void OnEnable()
        {
            buttonHome.onClick.AddListener(OnClickHome);
            buttonSettings.onClick.AddListener(OnClickSettings);
            buttonResume.onClick.AddListener(OnClickResume);

            GameManager.OnLevelInstantiated += OnLevelInstantiated;
        }

        private void OnDisable()
        {
            buttonHome.onClick.RemoveListener(OnClickHome);
            buttonSettings.onClick.RemoveListener(OnClickSettings);
            buttonResume.onClick.RemoveListener(OnClickResume);
            
            GameManager.OnLevelInstantiated += OnLevelInstantiated;
        }

        private void OnClickSettings()
        {
            MenuManager.Instance.ChangeMenuState(MenuState.SETTINGS_MENU);
        }
        
        private void OnLevelInstantiated()
        {
            textLevel.text = LocalizationManager.GetTranslation(ScriptTerms.Level) + " " + (GameManager.Instance.CurrentLevelIndex + 1);
        }
        
        private void OnClickHome() => OnHomeClicked?.Invoke();
        private void OnClickResume() => OnResumeClicked?.Invoke();
    }
}
