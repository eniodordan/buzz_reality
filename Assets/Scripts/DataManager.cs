using System;
using UnityEngine;

namespace BuzzReality
{
    public class DataManager : Singleton<DataManager>
    {
        public static event Action OnDataUpdated;
        public static event Action OnProgressReset;
        
        private void OnEnable()
        {
            GameManager.OnLevelCompleted += OnLevelCompleted;
            SettingsMenu.OnResetProgressClicked += OnResetProgressClicked;
        }

        private void OnDisable()
        {
            GameManager.OnLevelCompleted -= OnLevelCompleted;
            SettingsMenu.OnResetProgressClicked -= OnResetProgressClicked;
        }

        private void OnLevelCompleted(int index, float time, int attempts, int rating)
        {
            PlayerPrefs.SetInt("Level " + index + " Status", 1);
            PlayerPrefs.SetInt("Level " + index + " Rating", rating);
            
            OnDataUpdated?.Invoke();
        }

        private void OnResetProgressClicked()
        {
            PlayerPrefs.DeleteAll();
            OnProgressReset?.Invoke();
        }

        public bool IsLevelCompleted(int index) => PlayerPrefs.GetInt("Level " + index + " Status") == 1;

        public int GetLevelRating(int index) => PlayerPrefs.GetInt("Level " + index + " Rating");
    }
}
