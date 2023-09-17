using TMPro;
using I2.Loc;
using UnityEngine;

namespace BuzzReality
{
    public class IngameMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textLevel;
        [SerializeField] private TextMeshProUGUI textAttempts;
        [SerializeField] private TextMeshProUGUI textElapsedTime;
        [SerializeField] private GameObject gameObjectInvalidWires;
        
        private void OnEnable()
        {
            GameManager.OnLevelInstantiated += OnLevelInstantiated;
            GameManager.OnAttemptsUpdated += OnAttemptsUpdated;
        }

        private void OnDisable()
        {
            GameManager.OnLevelInstantiated -= OnLevelInstantiated;
            GameManager.OnAttemptsUpdated -= OnAttemptsUpdated;
        }

        private void OnLevelInstantiated()
        {
            textLevel.text = LocalizationManager.GetTranslation(ScriptTerms.Level) + " " + (GameManager.Instance.CurrentLevelIndex + 1);
            textAttempts.text = GameManager.Instance.Attempts.ToString();
            textElapsedTime.text = GameManager.Instance.ElapsedTime.ToString("F0");
            gameObjectInvalidWires.SetActive(GameManager.Instance.HasInvalidWires);
        }

        private void OnAttemptsUpdated() => textAttempts.text = GameManager.Instance.Attempts.ToString();

        private void Update() => textElapsedTime.text = GameManager.Instance.ElapsedTime.ToString("F0");
    }
}
