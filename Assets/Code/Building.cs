using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    public class Building : MonoBehaviour, IBurnable, IBuilding, IObstacle
    {
        [SerializeField] private float secondsBeforeDestroyed = 60;

        [FormerlySerializedAs("_fireParticle")] [SerializeField]
        private ParticleSystem[] _fireParticles;

        [SerializeField] private float _burnPercent = 100f;
        private float _currentTime;
        public bool IsBurning { get; set; }

        private void Awake()
        {
            SetPlayFireParticle(false);
        }

        private void Update()
        {
            if (IsBurning)
            {
                if (_currentTime < secondsBeforeDestroyed)
                {
                    _currentTime += Time.deltaTime;
                }
                else
                {
                    DestroyBuilding();
                }
            }
        }

        public void Ignite()
        {
            if (!IsBurning)
            {
                _burnPercent = 100;
                IsBurning = true;
                SetPlayFireParticle(true);
            }
        }

        private void SetPlayFireParticle(bool val)
        {
            for (int i = 0; i < _fireParticles.Length; i++)
            {
                if (val)
                {
                    _fireParticles[i].Play();
                }
                else
                {
                    _fireParticles[i].Stop();
                }
            }
        }

        public void Extinguish(float depleteVal)
        {
            if (!IsBurning)
            {
                return;
            }

            if (_burnPercent > 0)
            {
                _burnPercent -= depleteVal * Time.deltaTime;
              Debug.Log("Extinguished");
                return;
            }

            _currentTime = 0;
            SetPlayFireParticle(false);
            IsBurning = false;
        }

        private void DestroyBuilding()
        {
            //TODO: Spawn Cubes
            BuildingManager.Instance.RemoveBuilding(this);
            Destroy(gameObject);
        }

        public void OnCollided()
        {
            Ignite();
        }
    }
}