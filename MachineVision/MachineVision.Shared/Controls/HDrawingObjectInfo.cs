using HalconDotNet; 

namespace MachineVision.Shared.Controls
{
    public class HDrawingObjectInfo
    {
        public HDrawingObject HDrawingObject { get; set; }

        public HTuple[] HTuples { get; set; }

        public string Color { get; set; }
    }
}
