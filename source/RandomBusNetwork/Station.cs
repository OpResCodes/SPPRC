using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBusNetwork
{

    public class Station
    {
        public int id;
        private static int counter = 0;
        public int x;
        public int y;
        public bool isReliefPoint;

        public Station(int x_coord, int y_coord, bool isRelief)
        {
            id = counter;
            counter++;
            x = x_coord;
            y = y_coord;
            isReliefPoint = isRelief;
        }

        public int DistanceTo(Station Destination)
        {
            if (Destination != null)
            {
                int d;
                d = Math.Abs(this.x - Destination.x) + Math.Abs(this.y - Destination.y);
                return d;
            }
            else
            {
                return -1;
            }
        }

        public int DistanceTo(int x, int y)
        {
            int d;
            d = Math.Abs(this.x - x) + Math.Abs(this.y - y);
            return d;
        }

        public int TravelTimeTo(Station Destination)
        {
            if (Destination != null)
            {
                int dist = DistanceTo(Destination);
                // 30km/h == 0.5km/m
                double tt = Math.Round(dist * 0.5, 0);
                return (int)tt;
            }
            else
            {
                return -1;
            }
        }

        public override string ToString()
        {
            string ResultString = String.Empty;
            try
            {
                ResultString = String.Format("Station: {0}\n Point: ({1},{2})\n Reliefpoint: {3}",
                id.ToString(), x.ToString(), y.ToString(), isReliefPoint.ToString());
            }
            catch (Exception ex)
            {

                ResultString = ex.Message;
            }

            return ResultString;
        }

    }
}
