using System.Collections.Generic;
using Tram.Common.Models;

namespace Tram.Controller.Repositories
{
    public interface IRepository
    {
        void LoadData(params string[] parameters);

        List<TramIntersection> GetTramIntersections();

        List<Node> GetMap();

        List<TramLine> GetLines();

        List<CarIntersection> GetCarIntersections();
    }
}
