using UnityEngine;
using UnityEngine.UI;

namespace BuzzReality
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button buttonStart;
        [SerializeField] private Button buttonSettings;
        [SerializeField] private Button buttonExit;
        
        private void OnEnable()
        {
            buttonStart.onClick.AddListener(OnClickStart);
            buttonSettings.onClick.AddListener(OnClickSettings);
            buttonExit.onClick.AddListener(OnClickExit);
        }

        private void OnDisable()
        {
            buttonStart.onClick.RemoveListener(OnClickStart);
            buttonSettings.onClick.RemoveListener(OnClickSettings);
            buttonExit.onClick.RemoveListener(OnClickExit);
        }

        private void OnClickStart() => MenuManager.Instance.ChangeMenuState(MenuState.SELECT_MENU);
        
        private void OnClickSettings() => MenuManager.Instance.ChangeMenuState(MenuState.SETTINGS_MENU);
        
        private void OnClickExit() => Application.Quit();
    }
}
