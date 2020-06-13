namespace Paramdigma.Core.Geometry
{
    public class DelaunayEdge
    {
        public DelaunayPoint StartPoint;
        public DelaunayPoint EndPoint;

        public DelaunayEdge(DelaunayPoint startPoint, DelaunayPoint endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            var edge = obj as DelaunayEdge;
            var samePoints = StartPoint == edge.StartPoint && EndPoint == edge.EndPoint;
            var samePointsReversed = StartPoint == edge.EndPoint && EndPoint == edge.StartPoint;
            return samePoints || samePointsReversed;
        }

        public override int GetHashCode()
        {
            int hCode = (int)StartPoint.X ^ (int)StartPoint.Y ^ (int)EndPoint.X ^ (int)EndPoint.Y;
            return hCode.GetHashCode();
        }



    }
}
