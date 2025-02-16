using System;
using UnityEngine;

namespace GameplaySimulation
{
    public class SpawnManager
    {
        private const int StartCountDestroyToIncreaseSpawn = 200;
        private const int MinCountDestroyToIncreaseSpawn = 25;
        private const int ChangeCountDestroyIntervalRank = 30000;

        private int _countDestroyToIncreaseSpawn = StartCountDestroyToIncreaseSpawn;

        public void AdjustSpawnConditions(int rank)
        {
            _countDestroyToIncreaseSpawn = Mathf.Max(
                MinCountDestroyToIncreaseSpawn,
                StartCountDestroyToIncreaseSpawn - Convert.ToInt32(Mathf.Round(
                    StartCountDestroyToIncreaseSpawn - MinCountDestroyToIncreaseSpawn) * ((float)rank / ChangeCountDestroyIntervalRank))
            );
        }

        public int GetCurrentSpawnThreshold()
        {
            return _countDestroyToIncreaseSpawn;
        }
    }
}
