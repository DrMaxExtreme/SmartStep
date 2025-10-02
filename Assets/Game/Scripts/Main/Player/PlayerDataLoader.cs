using UnityEngine;

public class PlayerDataLoader : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    void Start()
    {
        _playerData.Load();
    }

    void EndGame(int steps, int collected)
    {
        _playerData.AddGameResult(steps, collected);
        _playerData.Save();
    }
}
