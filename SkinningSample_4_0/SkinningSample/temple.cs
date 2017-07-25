using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

namespace SmellOfRevenge2011
{
    public class temple
    {
        public Matrix world;
        public Vector3 position;
        public Vector3 direction;
        int level = 0;
        int health = 100; 
        public temple(Vector3 pos, Vector3 dir)
        {
            position = pos;
            direction = dir;
            world.Translation = position;
            world.Forward = direction;
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(world.Forward, world.Up);
            ScreenManager.pathBoard[(int)position.X / 30][(int)position.Z / 30] = false;

            ScreenManager.pathBoard[(int)position.X / 30 + 1][(int)position.Z / 30 + 1] = false;

            ScreenManager.pathBoard[(int)position.X / 30][(int)position.Z / 30 + 1] = false;

            ScreenManager.pathBoard[(int)position.X / 30 + 1][(int)position.Z / 30] = false;

                ScreenManager.pathBoard[(int)position.X / 30 + 2][(int)position.Z / 30 + 2] = false;

            ScreenManager.pathBoard[(int)position.X / 30][(int)position.Z / 30 + 2] = false;

            ScreenManager.pathBoard[(int)position.X / 30 + 2][(int)position.Z / 30] = false;


            ScreenManager.pathBoard[(int)position.X / 30 + 1][(int)position.Z / 30 + 2] = false;

            ScreenManager.pathBoard[(int)position.X / 30 + 2][(int)position.Z / 30 + 1] = false;


        }
        
    }
}
