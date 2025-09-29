using System;
using Sammy.Interfaces;
using UniRx;
using VContainer.Unity;

namespace Sammy.Controllers
{
    public class UIPresenter : IStartable, IDisposable
    {
        private readonly IPlayerDataService _playerData;
        private readonly IGameStateService _gameState;
        private readonly IUIView _uiView;
        private readonly CompositeDisposable _disposables = new();

        public UIPresenter(IPlayerDataService playerData, IGameStateService gameState, IUIView uiView)
        {
            _playerData = playerData;
            _gameState = gameState;
            _uiView = uiView;
        }
        
        public void Start()
        {
            _uiView.UpdatePoints(_playerData.Points, _gameState.MaxPoints);
            _uiView.UpdateWaspCount(_playerData.Wasps.Value);

            _playerData.Wasps
                .Subscribe(waspCount => _uiView.UpdateWaspCount(waspCount))
                .AddTo(_disposables);

            _playerData.CurrentCrystal
                .Subscribe(crystalColor => _uiView.UpdateCrystal(crystalColor))
                .AddTo(_disposables);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}