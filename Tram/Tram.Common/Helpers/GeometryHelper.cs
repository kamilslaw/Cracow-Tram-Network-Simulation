using Microsoft.DirectX;
using System;
using System.Device.Location;
using Tram.Common.Consts;

namespace Tram.Common.Helpers
{
    public static class GeometryHelper
    {
        public static float GetDistance(Vector2 pA, Vector2 pB) => (float)Math.Sqrt((pB.X - pA.X) * (pB.X - pA.X) + (pB.Y - pA.Y) * (pB.Y - pA.Y));

        public static float GetRealDistance(Vector2 pA, Vector2 pB)
        {
            var aCoord = new GeoCoordinate(pA.Y, pA.X);
            var bCoord = new GeoCoordinate(pB.Y, pB.X);
            return (float)aCoord.GetDistanceTo(bCoord);
        }

        // Finds the straight line (|AC|) that is perpendicular to |AB| line and cuts point A, then returns 2 points located on this line with 'distance' from point A 
        [Obsolete("Doesn't work well, use GetPerpendicularPointsWithDecimalOperations instead (ugly and slow, but good enough)")]
        public static Tuple<Vector2, Vector2> GetPerpendicularPoints(float pAX, float pAY, float pBX, float pBY, float distance)
        {
            if (Math.Abs(pAY - pBY) < CalculationConsts.EPSILON)
            {
                return Tuple.Create(new Vector2(pAX, pAY - distance), new Vector2(pAX, pAY + distance));
            }
            else if (Math.Abs(pAX - pBX) < CalculationConsts.EPSILON)
            {
                return Tuple.Create(new Vector2(pAX - distance, pAY), new Vector2(pAX + distance, pAY));
            }

            // Computes |AB| lines equation
            double aAB = (pBY - pAY) / (pBX - pAX);
            //double bAB = ((-pAX) * (pBY - pAY) - (-pAY) * (pBX - pAX)) / (pBX - pAX);
            // Computes |AC| lines equation
            double aAC = (-1) / aAB;
            double bAC = pAY - (aAC * pAX);
            // Computes quadratic equation for x-coordinates
            double xA = 1 + aAC * aAC;
            double xB = (-2) * pAX + (2) * aAC * (bAC - pAY);
            double xC = pAX * pAX + (bAC - pAY) * (bAC - pAY) - distance * distance;
            double delta = xB * xB - 4 * xA * xC;
            float x1 = (float)((-xB - Math.Sqrt(delta)) / (2 * xA));
            float x2 = (float)((-xB + Math.Sqrt(delta)) / (2 * xA));
            // Computes y-coordinates
            float y1 = (float)(aAC * x1 + bAC);
            float y2 = (float)(aAC * x2 + bAC);

            return Tuple.Create(new Vector2(x1, y1), new Vector2(x2, y2));
        }

        public static Tuple<Vector2, Vector2> GetPerpendicularPointsWithDecimalOperations(float pAX, float pAY, float pBX, float pBY, float distance)
        {
            if (Math.Abs(pAY - pBY) < CalculationConsts.EPSILON)
            {
                return Tuple.Create(new Vector2(pAX, pAY - distance), new Vector2(pAX, pAY + distance));
            }
            else if (Math.Abs(pAX - pBX) < CalculationConsts.EPSILON)
            {
                return Tuple.Create(new Vector2(pAX - distance, pAY), new Vector2(pAX + distance, pAY));
            }

            // Computes |AB| lines equation
            decimal aAB = (decimal)(pBY - pAY) / (decimal)(pBX - pAX);
            //double bAB = ((-pAX) * (pBY - pAY) - (-pAY) * (pBX - pAX)) / (pBX - pAX);
            // Computes |AC| lines equation
            decimal aAC = (-1) / aAB;
            decimal bAC = (decimal)pAY - (aAC * (decimal)pAX);
            // Computes quadratic equation for x-coordinates
            decimal xA = 1 + aAC * aAC;
            decimal xB = (-2) * (decimal)pAX + (2) * aAC * (bAC - (decimal)pAY);
            decimal xC = (decimal)pAX * (decimal)pAX + (bAC - (decimal)pAY) * (bAC - (decimal)pAY) - (decimal)distance * (decimal)distance;
            decimal delta = xB * xB - 4 * xA * xC;
            float x1 = (float)((-xB - (decimal)Math.Sqrt((double)delta)) / (2 * xA));
            float x2 = (float)((-xB + (decimal)Math.Sqrt((double)delta)) / (2 * xA));
            // Computes y-coordinates
            float y1 = (float)(aAC * (decimal)x1 + bAC);
            float y2 = (float)(aAC * (decimal)x2 + bAC);

            return Tuple.Create(new Vector2(x1, y1), new Vector2(x2, y2));
        }

        // The point that is on |AB| line, with displacment from pA (in %)
        public static Vector2 GetLocactionBetween(float displacment, Vector2 pA, Vector2 pB)
        {
            if (Math.Abs(pA.X - pB.X) < CalculationConsts.EPSILON)
            {
                double d = GetDistance(pA, pB) * displacment / 100;
                return new Vector2(pA.X, pA.Y > pB.Y ? pA.Y - (float)d : pA.Y + (float)d);
            }
            else if (Math.Abs(pA.Y - pB.Y) < CalculationConsts.EPSILON)
            {
                double d = GetDistance(pA, pB) * displacment / 100;
                return new Vector2(pA.X > pB.X ? pA.X - (float)d : pA.X + (float)d, pA.Y);
            }

            float x = Math.Abs(pB.X - pA.X) * displacment / 100;
            float y = Math.Abs(pB.Y - pA.Y) * displacment / 100;
            float cX = pA.X > pB.X ? pA.X - x : pA.X + x;
            float cY = pA.Y > pB.Y ? pA.Y - y : pA.Y + y;

            return new Vector2(cX, cY);
        }

        private static double Radians(double x) => x * Math.PI / 180;

        private static double Degrees(double x) => x * 180 / Math.PI;
    }
}
