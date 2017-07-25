using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework;
namespace SmellOfRevenge2011
{
    /// <summary>
    /// Provides a set of methods for the rendering BoundingBoxs.
    /// </summary>
    public static class BoundingBoxRenderer
    {
  static VertexPositionColor[] verts = new VertexPositionColor[8];
    static short[] indices = new short[]
        {
            0, 1,
            1, 2,
            2, 3,
            3, 0,
            0, 4,
            1, 5,
            2, 6,
            3, 7,
            4, 5,
            5, 6,
            6, 7,
            7, 4,
        };
 
    static BasicEffect effect;
    static VertexDeclaration vertDecl;
 
   
 
    /// &lt;summary&gt;
    /// Renders the bounding box for debugging purposes.
    /// &lt;/summary&gt;
    /// &lt;param name="box"&gt;The box to render.&lt;/param&gt;
    /// &lt;param name="graphicsDevice"&gt;The graphics device to use when rendering.&lt;/param&gt;
    /// &lt;param name="view"&gt;The current view matrix.&lt;/param&gt;
    /// &lt;param name="projection"&gt;The current projection matrix.&lt;/param&gt;
    /// &lt;param name="color"&gt;The color to use drawing the lines of the box.&lt;/param&gt;
    public static void RenderBox(
        BoundingBox box,
        GraphicsDevice graphicsDevice,
        Matrix view,
        Matrix projection,
        Color color)
    {
        if (effect == null)
        {
            effect = new BasicEffect(graphicsDevice);
            effect.VertexColorEnabled = true;
            effect.LightingEnabled = false;
            vertDecl = new VertexDeclaration(VertexPositionColor.VertexDeclaration.GetVertexElements());
         //   vertDecl = new VertexDeclaration(graphicsDevice, VertexPositionColor.VertexElements);
        }
 
        Vector3[] corners = box.GetCorners();
        for (int i = 0; i < 8; i++)
        {
            verts[i].Position = corners[i];
            verts[i].Color = color;
        }
 
       // graphicsDevice.Ver = vertDecl;
 
        effect.View = view;
        effect.Projection = projection;
 
       // effect.
        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
 
            graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList,  verts,   0,   8,  indices,  0, indices.Length / 2, vertDecl);
 
           // pass.End();
        }
       // effect.End();
    }
}
 
}
