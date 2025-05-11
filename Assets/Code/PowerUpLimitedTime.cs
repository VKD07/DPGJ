using UnityEngine;

namespace Code
{
    public abstract class PowerUpLimitedTime : MonoBehaviour
    {
        [SerializeField] private float _disableTime = 20f;
        private float _currentTime;

        private void Update()
        {
            RunTimer();
        }

        private void RunTimer()
        {
            if (_currentTime < _disableTime)
            {
                _disableTime += Time.deltaTime;
                return;
            }

            _currentTime = 0;
            gameObject.SetActive(false);
        }
    }
}