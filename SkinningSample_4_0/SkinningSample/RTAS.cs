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
   
    public class RTAS : GameScreen
    {
        GamePadState oldGamePadstate, currentgamePadState;
        Vector2 MichaelVec = Vector2.Zero;
        int x = 0;
        int y = 0;
        int adjY = 0;
        int attackY = 0;
        
        PlayerIndex pIndex;
        bool noSelection = false;

        bool destSelected = false;
        bool sameTile = false;
        bool adjacent = false;
        bool move = false;
        bool attack = false;
        bool distance = false;
        bool melee = false;

        public RTAS()
        {
            oldGamePadstate = new GamePadState();
            currentgamePadState = new GamePadState();


        }
        public void Move(Point boardIndex)
        {
 
            MichaelVec = new Vector2(x, y);
        }
        public void Attack(Point boardIndex)
        {

        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
bool coveredByOtherScreen)
        {
            oldGamePadstate = currentgamePadState;
            currentgamePadState = GamePad.GetState(PlayerIndex.One);
            if (ScreenManager.globalInput.IsMenuUp(null))
            {
               
                if (attack)
                {
                    attackY++;
                    if (attackY > 1)
                        attackY = 0;
                }
                else if (adjacent)
                {
                    adjY++;
                    if (adjY > 1)
                        adjY = 0; 
                }
                else
                y--;
                

            }
            if (ScreenManager.globalInput.IsMenuDown(null))
                y++;
            if (ScreenManager.globalInput.IsMenuRight(null))
                x++;
            if (ScreenManager.globalInput.IsMenuLeft(null))
                x--;
            if (ScreenManager.globalInput.IsMenuSelect(null, out pIndex ))
            {
                pIndex = 0;

                if (Math.Abs(MichaelVec.X - x) <= 1 && Math.Abs(MichaelVec.Y - y)<= 1)
                    adjacent = true;
                if (adjacent)
                    if (adjY == 0)
                        move = true;
                    else
                    {
                        attack = true;
                        attackY = 0;
                    }
                if (attack)
                {
                    if (attackY == 0)
                    {
                        melee = true;
                        
                    }
                    if (attackY == 1)
                        distance = true;
                }
                if (move)
                {
                    Move(new Point((int)MichaelVec.X, (int)MichaelVec.Y));

                }
                                    
            }
            
            ///Console.WriteLine(x);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }
        public void DrawHud()
        {

            Rectangle backgroundRectangle;// = new Rectangle();
            ScreenManager.SpriteBatch.Begin();

            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    if (i == MichaelVec.X && j == MichaelVec.Y)
                    {
                        ScreenManager.SpriteBatch.Draw(ScreenManager.gradient, new Rectangle((int)(MichaelVec.X) * 70 + 5, (int)(MichaelVec.Y) * 70 + 5, 70, 70), Color.White);
                        ScreenManager.SpriteBatch.Draw(ScreenManager.gold1, new Rectangle((int)(x) * 70 + 5, (int)(y) * 70 + 5, 70, 70), Color.White);
                    }
                    ScreenManager.SpriteBatch.Draw(ScreenManager.hexGrid, new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), Color.White);
                  //  ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, ScreenManager.board[i][j], new Vector2(i * 70 + 5, j * 70 + 5), Color.Blue);
                }

            if (adjacent)
            {
                ScreenManager.SpriteBatch.Draw(ScreenManager.gradient, new Rectangle(x * 70 + 5, y * 70 + 5, 70, 70), Color.Blue);


                ScreenManager.SpriteBatch.Draw(ScreenManager.gradient, new Rectangle(x * 70 + 25 , y * 70 + 25, 70, 70), Color.Blue);

                if (adjY == 0)
                {
                    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Move", new Vector2(x * 70 + 5, y * 70 + 5), Color.Yellow);
                    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Attack", new Vector2(x * 70 + 5, y * 70 + 25), Color.Red);
                }
                if (adjY == 1)
                {
                    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Move", new Vector2(x * 70 + 5, y * 70 + 5), Color.Red);
                    ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Attack", new Vector2(x * 70 + 5, y * 70 + 25), Color.Yellow);
                }


            }




            //if(
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
