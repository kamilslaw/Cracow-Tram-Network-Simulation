using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using Tram.Common.Extensions;

namespace Tram.Common.Helpers
{
    public static class DirectxHelper
    {
        public static CustomVertex.PositionColored[] CreateCircle(float pX, float pY, int color, float radius, int precision)
        {
            CustomVertex.PositionColored[] vertex = new CustomVertex.PositionColored[precision + 1]; 
            
            float WedgeAngle = (float)((2 * Math.PI) / precision);
            for (int i = 0; i <= precision; i++)
            {
                float theta = i * WedgeAngle;
                vertex[i].Position = new Vector3((float)(pX + radius * Math.Cos(theta)), 
                                                 (float)(pY - radius * Math.Sin(theta)), 
                                                 0f);
            }
            
            for (int i = 0; i < vertex.Length; i++)
            {
                vertex[i].Color = color;
            }

            return vertex;
        }

        public static CustomVertex.PositionColored[] CreateLine(float pX1, float pY1, float pX2, float pY2, int color, float thickness)
        {
            var aLine = GeometryHelper.GetPerpendicularPointsWithDecimalOperations(pX1, pY1, pX2, pY2, thickness);
            var bLine = GeometryHelper.GetPerpendicularPointsWithDecimalOperations(pX2, pY2, pX1, pY1, thickness);

            CustomVertex.PositionColored[] vertex = new CustomVertex.PositionColored[4];
            vertex[0].Position = aLine.Item1.ToVector3();
            vertex[1].Position = aLine.Item2.ToVector3();
            vertex[3].Position = bLine.Item2.ToVector3();
            vertex[2].Position = bLine.Item1.ToVector3();

            for (int i = 0; i < vertex.Length; i++)
            {
                vertex[i].Color = color;
            }

            return vertex;
        }
    }
}
