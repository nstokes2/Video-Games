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
    public class RealTimeActionStrategy : GameScreen
    {
        public int UltTimer = 100;
        public int CaptTimer = 125;
        public int LtTimer = 150;

        Vector2 MichaelVec;
        Vector2 DargaheVec;
        Vector2 redLt1Vec;
        Vector2 redLt2Vec;
        Vector2 blueLt1Vec;
        Vector2 blueLt2Vec;
        Vector2 redCptVec;
        Vector2 blueCptVec;
            
        int michael = 0;
        int dargahe = 0;
        int redLt1 = 0;
        int redLt2 = 0;
        int blueLt1 = 0;
        int blueLt2 = 0;
        int redCpt = 0;
        int blueCpt = 0;

        int index  = 1;
        
        int x = 0;
        int y = 0; 
       
        public RealTimeActionStrategy()
        {
            //player goes first

            MichaelVec = new Vector2(2, 1);
            DargaheVec = new Vector2(2, 4);
            redLt1Vec = new Vector2(1, 1);
            redLt2Vec = new Vector2(3, 1);
            blueLt1Vec = new Vector2(1, 4);
            blueLt2Vec = new Vector2(3, 4);
            blueCptVec = new Vector2(4, 4);


        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
       bool coveredByOtherScreen)
        {



            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }

        public void DrawHud()
        {

            Rectangle backgroundRectangle;// = new Rectangle();
            ScreenManager.SpriteBatch.Begin();

            for(int i = 0; i<5; i++)
                for (int j = 0; j < 5; j++)
                {
                    if (i == MichaelVec.X && j == MichaelVec.Y)
                    {
                        ScreenManager.SpriteBatch.Draw(ScreenManager.gradient, new Rectangle((int)(MichaelVec.X ) * 70 + 5, (int)(MichaelVec.Y) * 70 + 5, 70, 70), Color.White);
                        ScreenManager.SpriteBatch.Draw(ScreenManager.gradient, new Rectangle((int)(MichaelVec.X + x) * 70 + 5, (int)(MichaelVec.Y + y) * 70 + 5, 70, 70), Color.White);
                    }
                    
                    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, ScreenManager.board[i][j], new Vector2(i * 70+ 5, j * 70 + 5), Color.Blue);
                } 
            ScreenManager.SpriteBatch.End();




        }

        public override void Draw(GameTime gameTime)
        {


            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);



            DrawHud();



            base.Draw(gameTime);
        }














    }
}
