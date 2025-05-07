using System;
using UnityEngine;

public class VfxHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _boosters;
    [SerializeField] private GameObject[] _boosterTrail;

    private void OnDisable()
    {
        SetActiveBooster(false);
    }

    public void SetActiveBooster(bool val)
    {
        for (int i = 0; i < _boosters.Length; i++)
        {
            if (val)
            {
                _boosters[i].Play();
            }
            else
            {
                _boosters[i].Stop();
            }
        }
        
        for (int i = 0; i < _boosterTrail.Length; i++)
        {
            if (val)
            {
                _boosterTrail[i].SetActive(true);
            }
            else
            {
                _boosterTrail[i].SetActive(false);
            }
        }

    }
}
