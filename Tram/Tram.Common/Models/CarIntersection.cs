using Microsoft.DirectX;
using Tram.Common.Interfaces;

namespace Tram.Common.Models
{
    public class CarIntersection : IObjWithCoordinates
    {
        public Node Node { get; set; }

        public int GreenInterval { get; set; }

        public int RedInterval { get; set; }

        public float TimeToChange { get; set; }

        public Vector2 Coordinates
        {
            get { return Node.Coordinates; }
        }
    }
}
