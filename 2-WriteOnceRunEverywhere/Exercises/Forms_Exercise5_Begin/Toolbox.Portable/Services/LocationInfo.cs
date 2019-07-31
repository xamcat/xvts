namespace Toolbox.Portable.Services
{
    public class LocationInfo
	{
        public LocationInfo(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

		public double Latitude { get; set; } = 0;
		public double Longitude { get; set; } = 0;
	}
}