using UnityEngine;

namespace Code
{
    public class EnemyArrowIndicator : MonoBehaviour
    {
        [SerializeField] private Transform _enemy;
        [SerializeField] private Transform _player;
        [SerializeField] private RectTransform _damageImagePivot;

        private void Update()
        {
            if (!_enemy || !_player) return;

            Vector3 toEnemy = _enemy.position - _player.position;
            Vector3 forward = _player.forward;
            Vector3 right = _player.right;

            float x = Vector3.Dot(toEnemy, right);
            float y = Vector3.Dot(toEnemy, forward);
            float angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;

            _damageImagePivot.localEulerAngles = new Vector3(0, 0, -angle);
        }
    }
}