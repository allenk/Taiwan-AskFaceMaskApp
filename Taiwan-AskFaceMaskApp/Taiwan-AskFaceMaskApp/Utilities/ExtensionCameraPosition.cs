using Xamarin.Forms.GoogleMaps;

namespace Utilities
{
    /// <summary>
    /// 為了讓 JsonConvert 可以反序列化轉換 CameraPosition 而衍生的 Class
    /// 因為 CameraPosition 沒有 Default Constructor
    /// </summary>
    public class ExtensionCameraPosition
    {
        public Position Target { get; set; }
        public double Bearing { get; set; }
        public double Tilt { get; set; }
        public double Zoom { get; set; }

        public ExtensionCameraPosition() {}

        public ExtensionCameraPosition(CameraPosition cameraPosition)
        {
            Target = cameraPosition.Target;
            Bearing = cameraPosition.Bearing;
            Tilt = cameraPosition.Tilt;
            Zoom = cameraPosition.Zoom;
        }
        public CameraPosition ConvertToCameraPosition()
        {
            return new CameraPosition(Target, Zoom, Bearing, Tilt);
        }
    }
}