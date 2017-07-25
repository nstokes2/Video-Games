
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SmellOfRevenge2011
{

    /// <summary>
    /// best first searches
    /// </summary>
    #region Search Status Enum
    public enum SearchStatus
    {
        Stopped,
        Searching,
        NoPath,
        PathFound,
    }
    #endregion

    public class AIPathFinder
    {

        #region Search Node Struct
        /// <summary>
        /// Reresents one node in the search space
        /// </summary>
        private struct SearchNode
        {
            /// <summary>
            /// Location on the map
            /// </summary>
            public Point Position;

            /// <summary>
            /// Distance to goal estimate
            /// </summary>
            public int DistanceToGoal;

            /// <summary>
            /// Distance traveled from the start
            /// </summary>
            public int DistanceTraveled;

            public SearchNode(
                Point mapPosition, int distanceToGoal, int distanceTraveled)
            {
                Position = mapPosition;
                DistanceToGoal = distanceToGoal;
                DistanceTraveled = distanceTraveled;
            }
        }
        #endregion
        // How much time has passed since the last search step
        private float timeSinceLastSearchStep = 0f;

        // Seconds per search step        
        public float timeStep = .0f;

        public Point start;
        public Point end; 

        private List<SearchNode> openList;

        private List<SearchNode> closedList;

        private Dictionary<Point, Point> paths;


        // Tells us if the search is stopped, started, finished or failed
        public SearchStatus SearchStatus
        {
            get { return searchStatus; }
            set
            {
                searchStatus = value;
            }
        }
        private SearchStatus searchStatus;


        public int TotalSearchSteps
        {
            get { return totalSearchSteps; }
        }
        private int totalSearchSteps = 0;


  

        /// <summary>
        /// reset instead of initialize
        /// </summary>
        public void Initialize()
        {

            openList = new List<SearchNode>();
            closedList = new List<SearchNode>();
            paths = new Dictionary<Point, Point>();
        }

        public void Reset2(Vector3 target, Vector3 position)
        {
            searchStatus = SearchStatus.Searching;
            totalSearchSteps = 0;
            openList.Clear();
            closedList.Clear();
            paths.Clear();
            openList.Add(new SearchNode(fromVec(position), StepDistance(fromVec(position), fromVec(target)), 0));


        }
        public void Reset()
        {

            searchStatus = SearchStatus.Searching;
            totalSearchSteps = 0;
            openList.Clear();
            closedList.Clear();
            paths.Clear();

            ///have to add th part that adds to the open list
            //openList.Add(new SearchNode(

            openList.Add(new SearchNode(fromVec(ScreenManager.es1.World.Translation), StepDistance(fromVec(ScreenManager.es1.World.Translation), fromVec(ScreenManager.ps1.World.Translation)), 0));

        }

        private bool InMap(int column, int row)
        {

            return (row >= 0 && row < 10 && column >= 0 && column < 10);

        }

        private bool IsOpen(int column, int row)
        {
            return InMap(column, row) && ScreenManager.open[column][row] != false;
        }



        public IEnumerable<Point> OpenMapTiles(Point mapLoc)
        {
            
            if (IsOpen(mapLoc.X,mapLoc.Y + 1))
                yield return new Point(mapLoc.X, mapLoc.Y + 1);
            if (IsOpen(mapLoc.X, mapLoc.Y - 1))
                yield return new Point(mapLoc.X, mapLoc.Y - 1);
            if (IsOpen(mapLoc.X + 1, mapLoc.Y))
                yield return new Point(mapLoc.X + 1, mapLoc.Y);
            if (IsOpen(mapLoc.X - 1, mapLoc.Y))
                yield return new Point(mapLoc.X - 1, mapLoc.Y);

        }

        private bool SelectNodeToVisit(out SearchNode result)
        {
            result = new SearchNode();
            bool success = false;
            float smallestDistance = float.PositiveInfinity;
            float currentDistance = 0f;


            if (openList.Count > 0)
            {

                totalSearchSteps++;
                foreach (SearchNode node in openList)
                {
                    currentDistance = node.DistanceToGoal;
                    if (currentDistance < smallestDistance)
                    {
                        success = true;
                        result = node;
                        smallestDistance = currentDistance;

                    }
                }

            }
            return success;
        }

        public LinkedList<Point> FinalPath()
        {

            LinkedList<Point> path = new LinkedList<Point>();
            if (searchStatus == SearchStatus.PathFound)
            {
                Point curPrev = fromVec(ScreenManager.ps1.World.Translation);
                path.AddFirst(curPrev);
                while (paths.ContainsKey(curPrev))
                {
                    curPrev = paths[curPrev];
                    path.AddFirst(curPrev);
                }
            }
            return path;


        }
        /// <summary>
        /// Finds the minimum number of tiles it takes to move from Point A to 
        /// Point B if there are no barriers in the way
        /// </summary>
        /// <param name="pointA">Start position</param>
        /// <param name="pointB">End position</param>
        /// <returns>Distance in tiles</returns>
        public static int StepDistance(Point pointA, Point pointB)
        {
            int distanceX = Math.Abs(pointA.X - pointB.X);
            int distanceY = Math.Abs(pointA.Y - pointB.Y);

            return distanceX + distanceY;
        }

        public Point fromVec(Vector3 vec)
        {

            return new Point((int)vec.X / 60, (int)vec.Z / 60);
        }
        public Vector3 toVec(Point point)
        {
            return new Vector3(point.X * 60, 0.0f, point.Y * 60);
        }
        public int StepDistanceToEnd(Point point)
        {
            return StepDistance(point, fromVec(ScreenManager.ps1.World.Translation));
        }

        /// <summary>
        /// Determines if the given Point is inside the SearchNode list given
        /// </summary>
        private static bool InList(List<SearchNode> list, Point point)
        {
            bool inList = false;
            foreach (SearchNode node in list)
            {
                if (node.Position == point)
                {
                    inList = true;
                }
            }
            return inList;
        }

        /// <summary>
        /// Draw the search space
        /// </summary>
        public void Draw()
        {
            if (searchStatus == SearchStatus.PathFound)
            {
                int i = 0;
                int j = 0;

                Color boxColor = Color.Black;
                ScreenManager.primitiveBatch.Begin(PrimitiveType.LineList);
                foreach (Point point in FinalPath())
                {
                    i = point.X;
                    j = point.Y;
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);


                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);



                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);




                }

                ScreenManager.primitiveBatch.End();















            }
            if (searchStatus != SearchStatus.PathFound)
            {
                //spriteBatch.Begin();
                //foreach (SearchNode node in openList)
                //{
                //    spriteBatch.Draw(nodeTexture,
                //        map.MapToWorld(node.Position, true), null, openColor, 0f,
                //        nodeTextureCenter, scale, SpriteEffects.None, 0f);
                //}
                //foreach (SearchNode node in closedList)
                //{
                //    spriteBatch.Draw(nodeTexture,
                //        map.MapToWorld(node.Position, true), null, closedColor, 0f,
                //        nodeTextureCenter, scale, SpriteEffects.None, 0f);
                //}
                //spriteBatch.End();
                int i = 0;
                int j = 0;
                Color boxColor;
                ScreenManager.primitiveBatch.Begin(PrimitiveType.LineList);
                foreach (SearchNode node in openList)
                {
                    i = node.Position.X;
                    j = node.Position.Y;
                    boxColor = Color.Red;
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);


                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);



                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);




                }

                foreach (SearchNode node in closedList)
                {
                    i = node.Position.X;
                    j = node.Position.Y;
                    boxColor = Color.Blue;
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);


                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);



                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Min.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Max.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);

                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Min.Z), boxColor);
                    ScreenManager.primitiveBatch.AddVertex(new Vector3(ScreenManager.openBoxes[i][j].Min.X, ScreenManager.openBoxes[i][j].Max.Y, ScreenManager.openBoxes[i][j].Max.Z), boxColor);




                }

                ScreenManager.primitiveBatch.End();
            }



        }

        private void DoSearchStep()
        {
            SearchNode newOpenListNode;

            bool foundNewNode = SelectNodeToVisit(out newOpenListNode);
            if (foundNewNode)
            {
                Point currentPos = newOpenListNode.Position;
                foreach (Point point in OpenMapTiles(currentPos))
                {
                    SearchNode mapTile = new SearchNode(point, StepDistanceToEnd(point), newOpenListNode.DistanceTraveled + 1);
                    if (!InList(openList, point) && !InList(closedList, point))
                    {
                        openList.Add(mapTile);
                        paths[point] = newOpenListNode.Position;

                    }
                }
                if (currentPos == fromVec(ScreenManager.ps1.World.Translation))
                {
                    searchStatus = SearchStatus.PathFound;
                }
                openList.Remove(newOpenListNode);
                closedList.Add(newOpenListNode);
                  }
            else
            {
                searchStatus = SearchStatus.NoPath;
            }
        }






        public void Update(GameTime gameTime)
        {

            if (searchStatus == SearchStatus.Searching)
            {
                timeSinceLastSearchStep += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timeSinceLastSearchStep >= timeStep)
                {
                    DoSearchStep();
                    timeSinceLastSearchStep = 0f;
                }
            }


        }





    }
}
