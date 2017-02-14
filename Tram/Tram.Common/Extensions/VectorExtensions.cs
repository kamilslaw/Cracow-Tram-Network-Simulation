using Microsoft.DirectX;
using Tram.Common.Helpers;
using Tram.Common.Interfaces;

namespace Tram.Common.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 ToVector3(this Vector2 v2)
        {
            return new Vector3(v2.X, v2.Y, 0);
        } 

        public static float RealDistanceTo(this Vector2 v2, IObjWithCoordinates point)
        {
            return GeometryHelper.GetRealDistance(v2, point.Coordinates);
        }
    }
}
