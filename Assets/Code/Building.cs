using System;
using UnityEngine;

namespace Code
{
    public class Building : MonoBehaviour, IBurnable, IBuilding
    {
        [SerializeField] private float secondsBeforeDestroyed = 60;
        [SerializeField] private ParticleSystem _fireParticle;
        [SerializeField] private float _burnPercent = 100f;
        private float _currentTime;
        public bool IsBurning { get; set; }

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
                _fireParticle.Play();
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
                return;
            }

            _currentTime = 0;
            _fireParticle.Stop();
            IsBurning = false;
        }

        private void DestroyBuilding()
        {
            //TODO: Spawn Cubes
            Destroy(gameObject);
        }
    }
}