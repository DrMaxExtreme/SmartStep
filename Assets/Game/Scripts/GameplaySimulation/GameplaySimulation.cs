using UnityEngine;

namespace GameplaySimulation
{
    public class GameplaySimulation : MonoBehaviour
    {
        [Header("Simulations Count")]
        [SerializeField] private int _simulationsCount = 10;

        private GameplaySimulationRunner _simulationRunner;

        private void Start()
        {
            _simulationRunner = new GameplaySimulationRunner(_simulationsCount);
            _simulationRunner.RunSimulations();
        }
    }
}
