using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SkinnedModel;

namespace SmellOfRevenge2011
{
    public class Factory
    {
        public Vector3 position;
        public Vector3 A1Pos;
        public Vector3 B1Pos;
        public Vector3 X1Pos;
        public Vector3 Y1Pos;
        public int health;
        public int Trash;
        public float A1;
        public float A2;
        public float B1;
        public float B2;
        public float X1;
        public float X2;
        public float Y1;
        public float Y2;
        public float buildA1;
        public float buildA2;
        public float buildB1;
        public float buildB2;
        public float buildX1;
        public float buildX2;
        public float buildY1;
        public float buildY2;
        public Factory(Vector3 Position)
        {
            buildA1 = 2;
            buildX1 = 1;
            buildB1 = 0;
            buildY1 = 0;
            position = Position;
            A1Pos = Position - new Vector3(30.0f, 0.0f, 30.0f);
            B1Pos = Position + new Vector3(30.0f, 0.0f, 30.0f);
            X1Pos = Position - new Vector3(30.0f, 0.0f, -30.0f);
            Y1Pos = Position + new Vector3(30.0f, 0.0f, -30.0f);

        }
        public void Update(GameTime gameTime)
        {
            A1 += buildA1 * (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
            B1 += buildB1 * (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
            X1 += buildX1 * (float)gameTime.ElapsedGameTime.TotalSeconds * 10;
            Y1 += buildY1 * (float)gameTime.ElapsedGameTime.TotalSeconds * 10;

            if (A1 > 100)
            {
                A1 -= 100;
                ScreenManager.dummies.Add(new JuneXnaModel(A1Pos, Vector3.Forward));
                ScreenManager.loadSpheresJuneModel(ScreenManager.lastDummy());
                ScreenManager.lastDummy().type = 0;
              
                ScreenManager.lastDummy().SkinningData = ScreenManager.juneModel.Tag as SkinningData;
                ScreenManager.lastDummy().setAnimationPlayers2();
            }
            if (B1 > 100)
            {
                B1 -= 100;
                ScreenManager.dummies.Add(new JuneXnaModel(B1Pos, Vector3.Forward));
                ScreenManager.lastDummy().type = 2;
                ScreenManager.loadSpheresJuneModel(ScreenManager.lastDummy());
               
                ScreenManager.lastDummy().SkinningData = ScreenManager.juneModel.Tag as SkinningData;
                ScreenManager.lastDummy().setAnimationPlayers2();
            }
            if (X1 > 100)
            {
                X1 -= 100;
                ScreenManager.dummies.Add(new JuneXnaModel(X1Pos, Vector3.Forward));
                ScreenManager.lastDummy().type = 4;
                ScreenManager.lastDummy().SkinningData = ScreenManager.juneModel.Tag as SkinningData;
                ScreenManager.loadSpheresJuneModel(ScreenManager.lastDummy());
                ScreenManager.lastDummy().setAnimationPlayers2();
                
            }
            if (Y1 > 100)
            {
                Y1 -= 100;
                ScreenManager.dummies.Add(new JuneXnaModel(Y1Pos, Vector3.Forward));
                ScreenManager.lastDummy().type = 6;
                ScreenManager.loadSpheresJuneModel(ScreenManager.lastDummy());
              
                ScreenManager.lastDummy().SkinningData = ScreenManager.juneModel.Tag as SkinningData;
                ScreenManager.lastDummy().setAnimationPlayers2();
            }

        }

    }
}
