using System;
using UnityEngine;

namespace Code
{
    public class Barrier : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out PlayerDeathHandler playerDeathHandler))
            {
                playerDeathHandler.KillPlayer();
            }
        }
    }
}
