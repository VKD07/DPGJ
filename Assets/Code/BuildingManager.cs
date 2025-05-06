using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class BuildingManager : MonoBehaviour
    {
        public static BuildingManager Instance { get; private set; }

        public Building[] AllBuildings { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            AllBuildings = FindObjectsByType<Building>(FindObjectsSortMode.None);
        }
    }
}