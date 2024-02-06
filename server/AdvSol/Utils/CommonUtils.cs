using AdvSol.Services;
using NetTopologySuite.Geometries;

namespace AdvSol.Utils
{
    public static class CommonUtils
    {
        public static Coordinate[] PointsToCoordinate(double[][] points)
        {
            var coordinates = new List<Coordinate>();
            foreach (var point in points)
            {
                coordinates.Add(new Coordinate(point[0], point[1]));
            }

            return coordinates.ToArray();
        }

        public static string GetErrorDetail(this Dictionary<string, List<string>> errors)
        {
            var fileErrorDetail = new ErrorDetail(errors);
            return fileErrorDetail.ToString();
        }
    }
}
