using RiverCrossAI.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiverCrossAI.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 Add(this Vector3 thisVector, Vector3 vector)
        {
            return new Vector3(thisVector.X + vector.X,
                               thisVector.Y + vector.Y,
                               thisVector.Z + vector.Z);
        }

        public static Vector3 Add(this Vector3 thisVector, int x, int y, int z)
        {
            return new Vector3(thisVector.X + x,
                               thisVector.Y + y,
                               thisVector.Z + z);
        }

        public static Vector3 Subtract(this Vector3 thisVector, Vector3 vector)
        {
            return new Vector3(thisVector.X - vector.X,
                               thisVector.Y - vector.Y,
                               thisVector.Z - vector.Z);
        }

        public static Vector3 Subtract(this Vector3 thisVector, int x, int y, int z)
        {
            return new Vector3(thisVector.X - x,
                               thisVector.Y - y,
                               thisVector.Z - z);
        }
    }
}
