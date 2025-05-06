using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class BuildingManager : MonoBehaviour
    {
        public static BuildingManager Instance { get; private set; }

        public List<Building> AllBuildings { get; private set; }

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
            AllBuildings.Remove(building);
        }
    }
}