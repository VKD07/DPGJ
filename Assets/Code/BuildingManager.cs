using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Code
{
    public class BuildingManager : MonoBehaviour
    {
        public static BuildingManager Instance { get; private set; }

        public List<Building> AllBuildings { get; private set; }
        public Action ALlBuildingsDestroyed;
        private int numOfBuildingsDestroyed;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            AllBuildings = new List<Building>(FindObjectsByType<Building>(FindObjectsSortMode.None));
        }

        public void RemoveBuilding(Building building)
        {
            numOfBuildingsDestroyed++;

            if (numOfBuildingsDestroyed >= AllBuildings.Count)
            {
                ALlBuildingsDestroyed?.Invoke();
            }
            
            AllBuildings.Remove(building);
        }
        
    }
}