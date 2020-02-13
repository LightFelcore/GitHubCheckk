namespace SendMeADrink_Official
{
    internal class MapRegionMoveEventArgs
    {
        private double latitude;
        private double longitude;
        private int v;

        public MapRegionMoveEventArgs(double latitude, double longitude, int v)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.v = v;
        }
    }
}