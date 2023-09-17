using UnityEngine;

namespace BuzzReality
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
            }
        }

        private void OnApplicationQuit()
        {
            Instance = null;
            Destroy(gameObject);
        }
    }
}
