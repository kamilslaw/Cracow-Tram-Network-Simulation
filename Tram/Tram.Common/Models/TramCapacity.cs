using System.Collections.Generic;

namespace Tram.Common.Models
{
    public class TramCapacity
    {
        public List<int> GotIn { get; set; }

        public List<int> GotOut { get; set; }

        public List<int> CurrentState { get; set; }

        public TramCapacity()
        {
            GotIn = new List<int>();
            GotOut = new List<int>();
            CurrentState = new List<int>();
        }
    }
}
