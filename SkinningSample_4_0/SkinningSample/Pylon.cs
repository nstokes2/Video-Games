using System;
using System.Collections.Generic;
using System.Text;
using SkinnedModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SmellOfRevenge2011
{
    public class Pylon
    {
        public Vector3 Translation;
        public bool active = true;


        public Pylon(Vector3 trans)
        {
            Translation = trans;

        }
    }
}
