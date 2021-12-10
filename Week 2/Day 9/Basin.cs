using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RobotGryphon.AdventOfCode2021.Day8
{
    public class Basin
    {
        public Vector2 Lowpoint;
        public HashSet<Vector2> Containment = new();
        public HashSet<Vector2> Perimeter = new();
        public bool DoneGrowing = false;
        public PointMap Map;

        public int NumPoints => Containment.Count;
        public int Size => Containment.Select(p => Map.ElevationMap[p]).Sum();

        public Basin(PointMap map, Vector2 point)
        {
            Map = map;
            Lowpoint = point;
            Perimeter.Add(point);
            Containment.Add(point);
        }

        /// <summary>
        /// True if done growing; false otherwise
        /// </summary>
        /// <returns></returns>
        internal bool Grow()
        {
            if (DoneGrowing) return true;

            HashSet<Vector2> didExpand = new();
            HashSet<Vector2> perimLeft = new(Perimeter);
            foreach (Vector2 point in perimLeft)
            {
                var surrounding = Map.GetSurroundingPoints(point);
                foreach (var s in surrounding)
                {
                    if (Containment.Contains(s) || Map.ElevationMap[s] == 9)
                        continue;

                    didExpand.Add(point);
                    Perimeter.Add(s);
                    Containment.Add(s);
                    Perimeter.Remove(point);
                }
            }

            if(didExpand.Count == 0)
            {
                DoneGrowing = true;
                return true;
            }

            return false;
        }
    }
}
