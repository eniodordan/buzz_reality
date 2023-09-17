using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

namespace BuzzReality
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Slider sliderMusicVolume;
        [SerializeField] private Slider sliderSfxVolume;
        [SerializeField] private Button buttonResetProgress;
        [Space(10)]
        [SerializeField] private Button buttonBack;

        public static event Action OnResetProgressClicked;

        private void OnEnable()
        {
            buttonBack.onClick.AddListener(OnClickBack);
            buttonResetProgress.onClick.AddListener(OnClickResetProgress);
        }

        private void OnDisable()
        {
            buttonBack.onClick.RemoveListener(OnClickBack);
            buttonResetProgress.onClick.RemoveListener(OnClickResetProgress);
        }

        private void OnClickBack()
        {
            if (MenuManager.Instance.PreviousMenuState.Equals(MenuState.MAIN_MENU))
            {
                MenuManager.Instance.ChangeMenuState(MenuState.MAIN_MENU);
            }
            else if (MenuManager.Instance.PreviousMenuState.Equals(MenuState.PAUSE_MENU))
            {
                MenuManager.Instance.ChangeMenuState(MenuState.PAUSE_MENU);
            }
        }

        private void OnClickResetProgress() => OnResetProgressClicked?.Invoke();
        
        public void SetMusicVolume()
        {
            AudioManager.Instance.SetMusicVolume(sliderMusicVolume.value);
        }

        public void SetSFXVolume()
        {
            AudioManager.Instance.SetSFXVolume(sliderSfxVolume.value);
        }

        public void UpdateLanguage(int index)
        {
            LocalizationManager.CurrentLanguage = index == 0 ? "English" : "Croatian";
        }

        public void UpdateDominantHand(int index)
        {
            InputManager.Instance.UpdateDominantHand(index);
        }
    }
}
