using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Crucible : GameScreen
    {
        ScreenManager ScreenManager;
        public Crucible(ScreenManager screenManager) 
    {
        ScreenManager = screenManager;

      


    }
        public override void LoadContent()
        {
            ScreenManager.asterion.Position = new Vector3(-100.0f, 0.0f, 0.0f); 
            ScreenManager.michael.Position = new Vector3(-100.0f, 0.0f, 0.0f); 
            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                               bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            ScreenManager.medusa.Update(gameTime);
          //  ScreenManager.asterion.UpdateCrucible(gameTime);
            ScreenManager.michael.UpdateCrucible(gameTime);



        }

        public override void Draw(GameTime gameTime)
        {

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                   Color.CornflowerBlue, 0, 0);
            //ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            //ScreenManager.GraphicsDevice.
            bool draw = false;
            Matrix[] transforms = new Matrix[ScreenManager.angel.Bones.Count];
            ScreenManager.angel.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.angel.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(ScreenManager.michael.SkinTrans);
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;
                   // effect.DirectionalLight0 = new DirectionalLight(new EffectParameter(Vector3.Down), Color.Yellow, Color.White, null);
                    effect.EnableDefaultLighting();
                  //  effect.Texture = ScreenManager.white;
                  
                    //effect.AmbientLightColor = Color.White.ToVector3();
                    //effect.DiffuseColor = Color.White.ToVector3();
                    ////effect.EmissiveColor = Color.Brown.ToVector3();

                    //effect.DirectionalLight0.Enabled = true;
                    //effect.DirectionalLight0.DiffuseColor = Color.Blue.ToVector3();
                    //effect.DirectionalLight0.Direction = new Vector3(0, -.7f, .5f);
                    //effect.DirectionalLight0.SpecularColor = Color.Yellow.ToVector3();


                    //effect.DirectionalLight1.Enabled = true;
                    //effect.DirectionalLight1.DiffuseColor = Color.White.ToVector3();
                    //effect.DirectionalLight1.Direction = new Vector3(-.8f, 0, .6f);
                    //effect.DirectionalLight1.SpecularColor = Color.White.ToVector3();

                    //effect.DirectionalLight2.Enabled = true;
                    //effect.DirectionalLight2.DiffuseColor = Color.Blue.ToVector3();
                    //effect.DirectionalLight2.Direction = Vector3.Right;
                    //effect.DirectionalLight2.SpecularColor = Color.Yellow.ToVector3();

                    

                }
                //if (draw)\
            //if(mesh.Name == "wings")
                mesh.Draw();
            }


            transforms = new Matrix[ScreenManager.gorgon.Bones.Count];

            ScreenManager.gorgon.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ScreenManager.gorgon.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(ScreenManager.medusa.SkinTrans);
                    effect.View = ScreenManager.camera.View;
                    effect.Projection = ScreenManager.camera.Projection;
                    effect.EnableDefaultLighting();


                    //effect.AmbientLightColor = Color.White.ToVector3();
                    //effect.DiffuseColor = Color.White.ToVector3();
                    //effect.Texture = ScreenManager.blankTexture;




                }
                //if (draw)
                mesh.Draw();
            }


        }


    }
}
