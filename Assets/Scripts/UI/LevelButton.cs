using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace BuzzReality
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Button buttonLevel;
        [SerializeField] private TextMeshProUGUI textLevel;
        [SerializeField] private Image imageRadialIndicator;
        [Space(10)]
        [SerializeField] private GameObject gameObjectRating;
        [SerializeField] private Sprite emptyStar;
        [SerializeField] private Sprite filledStar;
        [SerializeField] private List<Image> ratingStars;
        
        [field: SerializeField] public int LevelIndex { get; private set; }
        [field: SerializeField] public bool IsLocked { get; private set; }

        private float _indicatorTimer = 1.0f;
        private float _maxIndicatorTimer = 1.0f;
        private bool _isButtonPressed = false;

        public event Action<int> OnLevelClicked;
        
        public void UpdateLevel(int levelIndex)
        {
            textLevel.text = (levelIndex + 1).ToString();
            LevelIndex = levelIndex;
        }
        
        public void Lock(bool value)
        {
            textLevel.gameObject.SetActive(!value);
            buttonLevel.interactable = !value;
            IsLocked = value;
        }

        public void UpdateRating(int rating)
        {
            for (int i = 0; i < ratingStars.Count; i++)
            {
                ratingStars[i].sprite = i < rating ? filledStar : emptyStar;
            }
        }

        public void HideRating(bool value)
        {
            gameObjectRating.SetActive(!value);
        }

        public void OnPointerDown()
        {
            if (!buttonLevel.interactable) return;
            
            _isButtonPressed = true;
            imageRadialIndicator.enabled = true;
        }

        public void OnPointerUp()
        {
            if (!buttonLevel.interactable) return;

            _isButtonPressed = false;
            imageRadialIndicator.enabled = false;
            
            _indicatorTimer = _maxIndicatorTimer;
            imageRadialIndicator.fillAmount = _maxIndicatorTimer;
        }

        private void Update()
        {
            if (_isButtonPressed)
            {
                _indicatorTimer -= Time.deltaTime;
                imageRadialIndicator.fillAmount = _indicatorTimer;

                if (_indicatorTimer <= 0)
                {
                    OnPointerUp();
                    OnLevelClicked?.Invoke(LevelIndex);
                }
            }
        }
    }
}
