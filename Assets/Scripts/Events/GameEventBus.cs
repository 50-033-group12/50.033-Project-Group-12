using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public class GameEventBus : MonoBehaviour
    {
        public UnityEvent GamePauseEvent;
        public UnityEvent GameResumeEvent;
        public UnityEvent GameOverEvent;

        private static GameEventBus _instance;

        void Start()
        {
            _instance = this;
        }

        public static GameEventBus GetInstance()
        {
            return _instance;
        }
        
        public void CheckForPlayerDeath(int x, int a, int b)
        {
            
        }
    }
}
