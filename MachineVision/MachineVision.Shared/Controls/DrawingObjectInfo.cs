using HalconDotNet;

namespace MachineVision.Shared.Controls
{
    public enum ShapeType
    {
        Rectangle,
        Ellipse,
        Circle,
        Region
    }

    public class DrawingObjectInfo
    {
        public ShapeType ShapeType { get; set; }

        public HObject Hobject { get; set; }

        public HTuple[] HTuples { get; set; }
    }
}
