using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SmellOfRevenge2011
{
    public struct SearchNode
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
    public enum SearchStatus
    {
        Stopped,
        Searching,
        NoPath,
        PathFound,
    }
    public enum SearchMethod
    {
        BreadthFirst,
        BestFirst,
        AStar,
        Max,
    }
    public class UnitPathing
    {
        public Point start, end;

        // Holds search nodes that are avaliable to search
        public List<SearchNode> openList;
        // Holds the nodes that have already been searched
        public List<SearchNode> closedList;
        // Holds all the paths we've creted so far
        private Dictionary<Point, Point> paths;
        // Tells us if the search is stopped, started, finished or failed
        public SearchStatus SearchStatus
        {
            set
            {
                searchStatus = value;
            }
            get { return searchStatus; }
        }
        private SearchStatus searchStatus;

        // Tells us which search type we're using right now
        public SearchMethod SearchMethod
        {
            get { return searchMethod; }
        }
        private SearchMethod searchMethod = SearchMethod.BestFirst;

        public bool IsSearching
        {

            get { return searchStatus == SearchStatus.Searching; }
            set
            {
                if (searchStatus == SearchStatus.Searching)
                {
                    searchStatus = SearchStatus.Stopped;
                }
                else if (searchStatus == SearchStatus.Stopped)
                {
                    searchStatus = SearchStatus.Searching;
                }
            }
        }

        public int TotalSearchSteps
        {
            get { return totalSearchSteps; }
        }
        private int totalSearchSteps = 0;

        public UnitPathing(Point Start, Point End)
        {

            searchStatus = new SearchStatus();

            searchStatus = SearchStatus.Stopped;
            openList = new List<SearchNode>();
            closedList = new List<SearchNode>();
            paths = new Dictionary<Point, Point>();

            start = Start;
            end = End;

            Reset(start, end);
            
        }

        public void Reset(Point StartPt, Point EndPt)
        {
            start = StartPt;
            end = EndPt;
            searchStatus = SearchStatus.Stopped;
            totalSearchSteps = 0;
            openList.Clear();
            closedList.Clear();
            paths.Clear();
            openList.Add(new SearchNode(start, StepDistance(start, end), 0));

        }
        /// <summary>
        /// Cycle through the search method to the next type
        /// </summary>
        public void NextSearchType()
        {
            searchMethod = (SearchMethod)(((int)searchMethod + 1) %
                (int)SearchMethod.Max);
        }

        private bool InMap(int column, int row)
        {
            return (row >= 0 && row < 24 &&
                column >= 0 && column < 24);
        }

        /// <summary>
        /// Returns true if the given map location exists and is not 
        /// blocked by a barrier
        /// </summary>
        /// <param name="column">column position(x)</param>
        /// <param name="row">row position(y)</param>
        private bool IsOpen(int column, int row)
        {
            return InMap(column, row) && ScreenManager.pathBoard[column][row] != false;
        }

        /// <summary>
        /// Enumerate all the map locations that can be entered from the given 
        /// map location
        /// </summary>
        public IEnumerable<Point> OpenMapTiles(Point mapLoc)
        {

            if (IsOpen(mapLoc.X, mapLoc.Y + 1))
                yield return new Point(mapLoc.X, mapLoc.Y + 1);
            if (IsOpen(mapLoc.X, mapLoc.Y - 1))
                yield return new Point(mapLoc.X, mapLoc.Y - 1);
            if (IsOpen(mapLoc.X + 1, mapLoc.Y))
                yield return new Point(mapLoc.X + 1, mapLoc.Y);
            if (IsOpen(mapLoc.X - 1, mapLoc.Y))
                yield return new Point(mapLoc.X - 1, mapLoc.Y);

            if (IsOpen(mapLoc.X - 1, mapLoc.Y - 1))
                yield return new Point(mapLoc.X - 1, mapLoc.Y - 1);
            if (IsOpen(mapLoc.X - 1, mapLoc.Y + 1))
                yield return new Point(mapLoc.X - 1, mapLoc.Y + 1);
            if (IsOpen(mapLoc.X + 1, mapLoc.Y - 1))
                yield return new Point(mapLoc.X + 1, mapLoc.Y - 1);
            if (IsOpen(mapLoc.X + 1, mapLoc.Y + 1))
                yield return new Point(mapLoc.X + 1, mapLoc.Y + 1);
        }

        public void DoSearchStep()
        {
            SearchNode newOpenListNode;

            bool foundNewNode = SelectNodeToVisit(out newOpenListNode);
            if (foundNewNode)
            {
                Point currentPos = newOpenListNode.Position;
                foreach (Point point in OpenMapTiles(currentPos))
                {
                    SearchNode mapTile = new SearchNode(point,
                        StepDistanceToEnd(point),
                        newOpenListNode.DistanceTraveled + 1);
                    if (!InList(openList, point) &&
                        !InList(closedList, point))
                    {
                        openList.Add(mapTile);
                        paths[point] = newOpenListNode.Position;
                    }
                }
                if (currentPos == end)
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
        private bool SelectNodeToVisit(out SearchNode result)
        {
            result = new SearchNode();
            bool success = false;
            float smallestDistance = float.PositiveInfinity;
            float currentDistance = 0f;
            if (openList.Count > 0)
            {
                switch (searchMethod)
                {
                    // Breadth first search looks at every possible path in the 
                    // order that we see them in.
                    case SearchMethod.BreadthFirst:
                        totalSearchSteps++;
                        result = openList[0];
                        success = true;
                        break;
                    // Best first search always looks at whatever path is closest to
                    // the goal regardless of how long that path is.
                    case SearchMethod.BestFirst:
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
                        break;
                    // A* search uses a heuristic, an estimate, to try to find the 
                    // best path to take. As long as the heuristic is admissible, 
                    // meaning that it never over-estimates, it will always find 
                    // the best path.
                    case SearchMethod.AStar:
                        totalSearchSteps++;
                        foreach (SearchNode node in openList)
                        {
                            currentDistance = Heuristic(node);
                            // The heuristic value gives us our optimistic estimate 
                            // for the path length, while any path with the same 
                            // heuristic value is equally ‘good’ in this case we’re 
                            // favoring paths that have the same heuristic value 
                            // but are longer.
                            if (currentDistance <= smallestDistance)
                            {
                                if (currentDistance < smallestDistance)
                                {
                                    success = true;
                                    result = node;
                                    smallestDistance = currentDistance;
                                }
                                else if (currentDistance == smallestDistance &&
                                    node.DistanceTraveled > result.DistanceTraveled)
                                {
                                    success = true;
                                    result = node;
                                    smallestDistance = currentDistance;
                                }
                            }
                        }
                        break;
                }
            }
            return success;
        }


        private static float Heuristic(SearchNode location)
        {
            return location.DistanceTraveled + location.DistanceToGoal;
        }

        public LinkedList<Point> FinalPath()
        {
            LinkedList<Point> path = new LinkedList<Point>();
            if (searchStatus == SearchStatus.PathFound)
            {
                Point curPrev = end;
                path.AddFirst(curPrev);
                while (paths.ContainsKey(curPrev))
                {
                    curPrev = paths[curPrev];
                    path.AddFirst(curPrev);
                }
            }
            return path;
        }
        public static int StepDistance(Point pointA, Point pointB)
        {
            int distanceX = Math.Abs(pointA.X - pointB.X);
            int distanceY = Math.Abs(pointA.Y - pointB.Y);

            return distanceX + distanceY;
        }
        public int StepDistanceToEnd(Point point)
        {
            return StepDistance(point, end);
        }




    }
}
