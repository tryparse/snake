using System.Collections;
using System.Collections.Generic;
using MonoGame.Extended;

namespace SnakeGame.Shared.ECS
{
    class TileCell
    {
        public Size2 Size;
        public Transform2 Transform;
        private List<TileCellLayer> _layers;

        public TileCell(Transform2 transform, Size2 size)
        {
            Transform = transform;
            Size = size;

            Rectangle = new RectangleF(transform.Position.X, transform.Position.Y, size.Width, size.Height);
            _layers = new List<TileCellLayer>();
        }

        public void AddLayer(TileCellLayer layer)
        {
            _layers.Add(layer);
        }

        public RectangleF Rectangle { get; }

        public IEnumerable Layers => _layers;
    }
}
