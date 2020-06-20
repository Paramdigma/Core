namespace Paramdigma.Core.Geometry
{
    public class DelaunayEdge
    {
        public DelaunayPoint EndPoint;
        public DelaunayPoint StartPoint;

        public DelaunayEdge(DelaunayPoint startPoint, DelaunayPoint endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            var edge = obj as DelaunayEdge;
            var samePoints = this.StartPoint == edge.StartPoint && this.EndPoint == edge.EndPoint;
            var samePointsReversed = this.StartPoint == edge.EndPoint && this.EndPoint == edge.StartPoint;
            return samePoints || samePointsReversed;
        }

        public override int GetHashCode()
        {
            var hCode = (int)this.StartPoint.X ^ (int)this.StartPoint.Y ^ (int)this.EndPoint.X ^ (int)this.EndPoint.Y;
            return hCode.GetHashCode();
        }
    }
}