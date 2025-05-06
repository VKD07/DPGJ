using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class LootPowerUpsManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _powerUps;
        [SerializeField] private int _numofPoolPerPowerUp = 6;

        private List<GameObject> _poolOfPowerUps = new List<GameObject>();
    
        public GameObject [] PowerUps => _poolOfPowerUps.ToArray();
        private void Awake()
        {
            InitPool();
        }

        private void InitPool()
        {
            for (int i = 0; i < _powerUps.Length; i++)
            {
                for (int j = 0; j < _numofPoolPerPowerUp; j++)
                {
                    GameObject powerUp = Instantiate(_powerUps[i], transform.position, Quaternion.identity);
                    _poolOfPowerUps.Add(powerUp);
                    powerUp.SetActive(false);
                }
            }
        }
    }
}
