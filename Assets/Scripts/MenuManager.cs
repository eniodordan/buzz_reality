using System;

namespace BuzzReality
{
    public class MenuManager : Singleton<MenuManager>
    {
        private MenuState _currentMenuState = MenuState.MAIN_MENU;
        private MenuState _previousMenuState = MenuState.MAIN_MENU;

        public MenuState CurrentMenuState => _currentMenuState;
        public MenuState PreviousMenuState => _previousMenuState;

        public static event Action<MenuState, MenuState> OnMenuStateChanged;

        public void ChangeMenuState(MenuState newState)
        {
            if (_currentMenuState.Equals(newState)) return;

            _previousMenuState = _currentMenuState;
            _currentMenuState = newState;
            
            OnMenuStateChanged?.Invoke(_previousMenuState, newState);
        }
    }
}
