using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBusNetwork
{
    public class BusTrip
    {
        //Fields
        public BusLine Line;
        public int id;
        public static int counter = 0;
        private List<Station> _stationsOfLine;


        //Properties
        public string Label
        {
            get
            {
                return Line.id.ToString() + "_" + id.ToString();
            }
        }
        public int StartTime { get; private set; }
        public int EndTime
        {
            get
            {
                return (StartTime + Line.TripDuration);
            }
        }
        public Station StartStation
        {
            get
            {
                return Line.Start;
            }
        }
        public Station EndStation
        {
            get
            {
                return Line.End;
            }
        }

        //CTOR
        public BusTrip(BusLine l, int stt)
        {
            //nicht threadsafe ;-)
            id = counter;
            counter++;
            Line = l;
            StartTime = stt;
            _stationsOfLine = new List<Station>();
        }

        public override string ToString()
        {
            String rs = "Trip: {0}, Start: {1}, End: {2}, Duration: {3}-{4}";

            return String.Format(rs,
                Label,
                StartStation.id.ToString(),
                EndStation.id.ToString(),
                StartTime.ToString(),
                EndTime.ToString()
                );
        }

    }
}
