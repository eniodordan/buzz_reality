using System;
using UnityEngine;
using System.Collections.Generic;

namespace BuzzReality
{
    [Serializable]
    public class MenuStateToCanvas
    {
        public CanvasGroup CanvasGroup;
        public MenuState MenuState;
    }
    
    public class CanvasChangeListener : MonoBehaviour
    {
        [SerializeField] private List<MenuStateToCanvas> menuStates;
        
        private void OnEnable()
        {
            MenuManager.OnMenuStateChanged += OnMenuStateChanged;
        }

        private void OnDisable()
        {
            MenuManager.OnMenuStateChanged -= OnMenuStateChanged;
        }

        private void OnMenuStateChanged(MenuState oldState, MenuState newState)
        {
            foreach (var menuState in menuStates)
            {
                if (menuState.MenuState.Equals(oldState))
                {
                    menuState.CanvasGroup.alpha = 0f;
                    menuState.CanvasGroup.interactable = false;
                    menuState.CanvasGroup.blocksRaycasts = false;
                    
                }
                else if (menuState.MenuState.Equals(newState))
                {
                    menuState.CanvasGroup.alpha = 1f;
                    menuState.CanvasGroup.interactable = true;
                    menuState.CanvasGroup.blocksRaycasts = true;
                }
            }
        }
    }
}
