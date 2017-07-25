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
    public class LetterStream : GameScreen
    {
        /// <summary>
        /// you can change octaves to move around, and you can play chords with certain letters to do combos and crits
        /// </summary>
        /// 
        GamePadState oldGamePadState;
        GamePadState currentGamePadState;

        List<Rectangle> noteRecs;
        List<string> keys;
        List<float> keyTimes;
        Random myRandom;
        float keyTimer = 0;
        List<int> deadNotes;

        public LetterStream()
        {

            keyTimes = new List<float>();
            keys = new List<string>();
            myRandom = new Random(50);
            noteRecs = new List<Rectangle>();
            deadNotes = new List<int>();
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
bool coveredByOtherScreen)
        {
            oldGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            keyTimer +=(float) gameTime.ElapsedGameTime.TotalMilliseconds /4;

            for (int i = 0; i < keyTimes.Count; i++)
            {
                keyTimes[i] +=(float) gameTime.ElapsedGameTime.TotalMilliseconds / 4;
                noteRecs[i] = new Rectangle((int)keyTimes[i], 200, noteRecs[i].Width, noteRecs[i].Height);
                if (keyTimes[i] > 225)
                {
                    //keyTimes.RemoveAt(i);
                    deadNotes.Add(i);
                }
//                    keyTimes[i].

            }
            for (int i = 0; i < deadNotes.Count; i++)
            {
                keyTimes.RemoveAt(deadNotes[i]);
                keys.RemoveAt(deadNotes[i]);
            }
            deadNotes.Clear();
            if (keyTimer > 30)
            {
                keyTimes.Add(0);
                keys.Add("A");
                keyTimer = 0; 
                noteRecs.Add(new Rectangle(0, 200, (int)ScreenManager.Font.MeasureString("A").X, (int)ScreenManager.Font.MeasureString("A").Y));
            }

            //if(currentGamePadState.IsButtonUp(Buttons.A) && oldGamePadState.IsButtonDown(Buttons.A))
                
            
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }

        public void DrawHud()
        {

            ScreenManager.SpriteBatch.Begin();

        for(int i = 0; i<keys.Count; i++)
            {
                ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, keys[i], new Vector2(keyTimes[i], 200), Color.Green);
            }
        ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "O", new Vector2(200, 200), Color.Gray);

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
