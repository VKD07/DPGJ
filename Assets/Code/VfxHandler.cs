using System;
using UnityEngine;

public class VfxHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _boosters;
    [SerializeField] private GameObject[] _boosterTrail;
    [SerializeField] private ParticleSystem[] _thrustBoosters;
    [SerializeField] private ParticleSystem[] _weaponLazer;
    [SerializeField] private ParticleSystem _water;

    private void OnDisable()
    {
        SetActiveBooster(false);
        SetEnableThrustBoosters(false);
        SetEnableLazerWeapon(false);
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

    public void SetEnableThrustBoosters(bool val)
    {
        for (int i = 0; i < _thrustBoosters.Length; i++)
        {
            if (val)
            {
                _thrustBoosters[i].Play();
            }
            else
            {
                _thrustBoosters[i].Stop();
            }
        }
    }

    public void SetEnableLazerWeapon(bool val)
    {
        for (int i = 0; i < _weaponLazer.Length; i++)
        {
            if (val)
            {
                _weaponLazer[i].Play();
            }
            else
            {
                _weaponLazer[i].Stop();
            }
        }
    }

    public void SetEnableWaterGun(bool val)
    {
        if (val)
        {
            _water.Play();
            return;
        }
        _water.Stop();
    }
}
