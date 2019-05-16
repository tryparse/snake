using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.GameEntities
{
    interface ISprite
    {
        Vector2 Position { get; }

        Vector2 Scale { get; }

        Vector2 Origin { get; }

        Texture2D Texture2D { get; }

        Color Color { get; }

        SpriteEffect Effect { get; }

        float Rotation { get; }

        float LayerDepth { get; }
    }
}
