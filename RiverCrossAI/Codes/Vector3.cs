using RiverCrossAI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiverCrossAI.Codes
{
    public class Vector3 : BindableBase
    {
        #region Constructors

        public Vector3(int x = 0, int y = 0, int z = 0)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public Vector3(Vector3 v)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
        }

        #endregion  

        #region Properties

        private int _x;
        public int X
        {
            get { return _x; }
            private set { Set(ref _x, value); }
        }

        private int _y;
        public int Y
        {
            get { return _y; }
            private set { Set(ref _y, value); }
        }

        private int _z;
        public int Z
        {
            get { return _z; }
            private set { Set(ref _z, value); }
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (!(obj is Vector3))
                return false;

            Vector3 vector = (Vector3)obj;

            // If all components are equal, then the vectors are equal
            return ((X == vector.X) && (Y == vector.Y) && (Z == vector.Z));

        }

        public override string ToString()
        {
            return $"x:{X}, y:{Y}, z:{Z},";
        }
    }
}
