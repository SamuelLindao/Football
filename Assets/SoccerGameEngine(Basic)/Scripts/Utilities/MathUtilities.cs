using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Utilities
{
    public class MathUtilities
    {
        /// <summary>
		/// Gets the tangent points.
		/// </summary>
		/// <returns><c>true</c>, if tangent points was gotten, <c>false</c> otherwise.</returns>
		/// <param name="center">C.</param>
		/// <param name="radius">R.</param>
		/// <param name="point">P.</param>
		/// <param name="tangentPont001">T1.</param>
		/// <param name="tangetPoint002">T2.</param>
		public static bool GetTangentPoints(Vector3 center, float radius, Vector3 point, ref Vector3 tangentPont001, ref Vector3 tangetPoint002)
        {
            //calculate the other two points
            Vector3 PmC = (point - center);
            float SqrLen = PmC.sqrMagnitude;
            float RSqr = radius * radius;
            if (SqrLen <= RSqr)
            {
                // P is inside or on the circle
                return false;
            }

            float InvSqrLen = 1 / SqrLen;
            float Root = Mathf.Sqrt(Mathf.Abs(SqrLen - RSqr));

            tangentPont001.x = (center.x + radius * (radius * PmC.x - PmC.z * Root) * InvSqrLen);
            tangentPont001.z = (center.z + radius * (radius * PmC.z + PmC.x * Root) * InvSqrLen);
            tangetPoint002.x = (center.x + radius * (radius * PmC.x + PmC.z * Root) * InvSqrLen);
            tangetPoint002.z = (center.z + radius * (radius * PmC.z - PmC.x * Root) * InvSqrLen);

            return true;
        }
    }
}
