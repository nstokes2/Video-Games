using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SkinningSample
{
    class Group
    {
       
     // public Group();
      //public ~Group();


      protected int id;
      public int ID
      {
          get
          {
              return id;
          }
          set
          {
              id = value;
          }
      }
      protected int type;
      public int Type
      {
          get
          {
              return type;
          }
          set
          {
              type = value;
          }
      }
      protected Vector2 centroid;
        public Vector2 Centroid{
            get{
                return centroid;
            }
            set{
                centroid = value;
            }
        }
      protected float maxSpeed;
      public float MaxSpeed
      {
          get
          {
              return maxSpeed;
          }
          set
          {
              maxSpeed = value;
          }
      }
      protected int commanderID;
      public int CommanderID
      {
          get
          {
              return commanderID;
          }
          set
          {
              commanderID = value;
          }
      }
      protected int formationID;
      public int FormationID
      {
          get
          {
              return formationID;
          }
          set
          {
              formationID = value;
          }
      }
      protected int numberUnits;
      List<Vector2> unitPositions;
      List<Vector2> desiredPositions;


      //public bool addUnit(int unitID);
      //public bool removeUnit(int unitID);
        
      //  public bool update();
        

    }
}
