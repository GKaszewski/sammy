using System;

namespace Sammy.Interfaces
{
    public interface IGameStateService
    {
        event Action OnWin;
        int MaxPoints { get; }
        bool IsGameWon { get; }
        
        void CheckWinCondition(int currentPoints);
        void TriggerWin();
        void GameOver();
    }
}