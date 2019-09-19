using Microsoft.Xna.Framework;
using SnakeGame.Shared.Common;
using SnakeGame.Shared.GameLogic.GameField.Interfaces;
using SnakeGame.Shared.GameLogic.Snake.Interfaces;
using SnakeGame.Shared.Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Shared.GameLogic.Snake
{
    class SnakeMovementSmoothComponent : ISnakeMovementComponent
    {
        private readonly ISnakeEntity _snakeEntity;
        private readonly IGameFieldComponent _gameFieldComponent;
        private readonly IGameSettings _gameSettings;
        private readonly SnakeControlKeys _snakeControlKeys;
        private readonly IMovingCalculator _movingCalculator;
        private readonly Vector2 _unitVector;

        private TimeSpan _elapsedTime;
        private TimeSpan _movingInterval;

        private Direction? _nextDirection;

        public SnakeMovementSmoothComponent(ISnakeEntity snakeEntity, IGameFieldComponent gameFieldComponent, IGameSettings gameSettings, SnakeControlKeys snakeControlKeys, IMovingCalculator movingCalculator)
        {
            _snakeEntity = snakeEntity ?? throw new ArgumentNullException(nameof(snakeEntity));
            _gameFieldComponent = gameFieldComponent ?? throw new ArgumentNullException(nameof(gameFieldComponent));
            _gameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            _snakeControlKeys = snakeControlKeys ?? throw new ArgumentNullException(nameof(snakeControlKeys));
            _movingCalculator = movingCalculator ?? throw new ArgumentNullException(nameof(movingCalculator));
            _unitVector = Vector2.Multiply(Vector2.UnitX, _gameSettings.TileSize);

            _movingInterval = TimeSpan.FromMilliseconds(_gameSettings.DefaultMoveIntervalTime);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            UpdateNextDirection();
        }

        /// <summary>
        /// Update snake's head direction by user input
        /// </summary>
        private void UpdateNextDirection()
        {
            if (InputHandler.IsKeyDown(_snakeControlKeys.Up) && _snakeEntity.Direction != Direction.Down)
            {
                _nextDirection = Direction.Up;
            }
            else if (InputHandler.IsKeyDown(_snakeControlKeys.Down) && _snakeEntity.Direction != Direction.Up)
            {
                _nextDirection = Direction.Down;
            }
            else if (InputHandler.IsKeyDown(_snakeControlKeys.Right) && _snakeEntity.Direction != Direction.Left)
            {
                _nextDirection = Direction.Right;
            }
            else if (InputHandler.IsKeyDown(_snakeControlKeys.Left) && _snakeEntity.Direction != Direction.Right)
            {
                _nextDirection = Direction.Left;
            }
        }
    }
}
