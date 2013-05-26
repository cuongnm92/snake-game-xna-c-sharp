using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Chapter6.Engine.Scenegraph;
using Chapter6.Engine.Objects;
using Chapter6.Engine;
using Microsoft.Xna.Framework.Input.Touch;

namespace Chapter6.Game.Game3D
{
    public class Hero3D : GameObject3D
    {
        private GameAnimatedModel _hero;

        private int _direction = 1;//1 = Right / -1 = Left
        private const int Speed = 75;
        private float _animationSpeedScale = 1.0f;

        public override void Initialize()
        {
            _hero = new GameAnimatedModel("Game3D/Vampire");
            _hero.SetAnimationSpeed(_animationSpeedScale);
            AddChild(_hero);
            Translate(0, -147, -100);

            base.Initialize();
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            _hero.PlayAnimation("Run");
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);

            var heroPos = WorldPosition;
            var projVec = renderContext.GraphicsDevice.Viewport.Project(heroPos, renderContext.Camera.Projection,
                                                          renderContext.Camera.View, Matrix.Identity);

            if (_direction == 1 && projVec.X >= TouchPanel.DisplayWidth)
            {
                _direction = -1;
            }
            else if (_direction == -1 && projVec.X <= 0)
            {
                _direction = 1;
            }

            heroPos += Vector3.Right * (float)(Speed * renderContext.GameTime.ElapsedGameTime.TotalSeconds * _direction);
            Rotate(0, 90 * _direction, 0);
            Translate(heroPos);
        }
    }
}
