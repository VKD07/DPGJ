using System;
using TMPro;
using UnityEngine;

namespace Code
{
    public class PlayerDroneKillHandler : MonoBehaviour
    {
        public int numOfDroneKilled;
        [SerializeField] private TextMeshProUGUI _droneKilledText;


        public void Update()
        {
            _droneKilledText.text = numOfDroneKilled.ToString();
        }
    }
}