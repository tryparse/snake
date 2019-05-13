﻿using Microsoft.Xna.Framework;
using snake.Common;
using snake.Configuration;
using snake.GameComponents;
using snake.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;

namespace snake.GameEntities
{
    public class Snake : IGameComponent, IUpdateable
    {
        private readonly ILogger _logger;
        private readonly Field _field;
        private readonly SnakeKeys _controls;
        private readonly List<SnakePart> _parts;

        private readonly int _stepTime;
        private int elapsedTime;

        private bool _enabled;
        private int _updateOrder;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public Snake(ILogger logger, Field field, Vector2 headPosition, SnakeKeys controls, Direction direction = Direction.Right)
        {
            this._logger = logger;
            this._field = field;
            this._controls = controls;
            this._stepTime = 1000;
            this._parts = new List<SnakePart>();
            _parts.Add(new SnakePart(headPosition, TileMetrics.Size, direction));

            AddPart();
            AddPart();
        }

        public List<SnakePart> Parts => _parts;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                EnabledChanged?.Invoke(this, new EventArgs());
                _enabled = value;
            }
        }

        public int UpdateOrder
        {
            get => _updateOrder;
            set
            {
                UpdateOrderChanged?.Invoke(this, new EventArgs());
                _updateOrder = value;
            }
        }

        public void Initialize()
        {
            // Nothing to initialize
        }

        public void AddPart()
        {
            var tail = _parts.Last();

            var x = tail.Position.X;
            var y = tail.Position.Y;

            CalculateStep(DirectionHelper.GetOppositeDirection(tail.Direction), ref x, ref y, TileMetrics.Size);

            _parts.Add(new SnakePart(new Vector2(x, y), tail.Size, tail.Direction ));
        }

        public void Update(GameTime gameTime)
        {
            var head = _parts.First();

            if (InputHandler.IsKeyDown(_controls.Up) && head.Direction != Direction.Down)
            {
                head.Direction = Direction.Up;
            }
            else if (InputHandler.IsKeyDown(_controls.Down) && head.Direction != Direction.Up)
            {
                head.Direction = Direction.Down;
            }
            else if (InputHandler.IsKeyDown(_controls.Right) && head.Direction != Direction.Left)
            {
                head.Direction = Direction.Right;
            }
            else if (InputHandler.IsKeyDown(_controls.Left) && head.Direction != Direction.Right)
            {
                head.Direction = Direction.Left;
            }

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime >= _stepTime)
            {
                elapsedTime = elapsedTime - _stepTime;

                if (Enabled)
                {
                    MoveTail();
                    MoveHead();
                }
            }
        }

        private void MoveHead()
        {
            var head = _parts.First();
            var x = head.Position.X;
            var y = head.Position.Y;

            var step = TileMetrics.Size;
            CalculateStep(head.Direction, ref x, ref y, step);

            head.Position = new Vector2(x, y);
        }

        private void CalculateStep(Direction direction, ref float x, ref float y, Vector2 step)
        {
            switch (direction)
            {
                case Direction.Up:
                    {
                        y -= step.Y;
                        break;
                    }
                case Direction.Down:
                    {
                        y += step.Y;
                        break;
                    }
                case Direction.Right:
                    {
                        x += step.X;
                        break;
                    }
                case Direction.Left:
                    {
                        x -= step.X;
                        break;
                    }
            }

            x = x > _field.Bounds.Width ? step.X / 2 : x < 0 ? _field.Bounds.Width - step.X / 2 : x;
            y = y > _field.Bounds.Height ? step.Y / 2 : y < 0 ? _field.Bounds.Height - step.Y / 2 : y;
        }

        private void MoveTail()
        {
            for (int i = _parts.Count - 1; i >= 1; i--)
            {
                _parts[i].Position = _parts[i - 1].Position;
                _parts[i].Direction = _parts[i - 1].Direction;
            }
        }
    }
}
