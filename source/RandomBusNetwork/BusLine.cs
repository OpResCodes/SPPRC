using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBusNetwork
{
    public class BusLine
    {
        public static int counter = 0;
        public int id;
        public Station Start;
        public Station End;
        public int Frequency;
        public int FirstBusStartTime;
        public int NumberOfTrips;
        public int TripDuration;
        public int[] TripStations
        {
            get
            {
                return _tripStations.Select((x) => x.id).ToArray();
            }
        }
        private List<Station> _tripStations;
        private List<BusTrip> _trips;

        public List<BusTrip> MyTrips
        {
            get
            {
                if (_trips == null)
                {
                    _trips = newTripList();
                }
                return _trips;
            }
        }

        public BusLine(Station start, Station end, int freq, int first, int numOfTrips, int tripdur)
        {
            id = counter;
            counter++;

            Start = start;
            End = end;
            Frequency = freq;
            FirstBusStartTime = first;
            NumberOfTrips = numOfTrips;
            TripDuration = tripdur;
            _tripStations = new List<Station>();
        }
        public BusLine(Station start, Station end, int freq, int first, int numOfTrips, int tripdur, List<Station> tripstationlist)
            : this(start, end, freq, first, numOfTrips, tripdur)
        {
            _tripStations = tripstationlist;
        }

        private List<BusTrip> newTripList()
        {
            int nextStart = FirstBusStartTime;
            List<BusTrip> generatedTrips = new List<BusTrip>();
            for (int i = 0; i < NumberOfTrips; i++)
            {
                //nicht threadsafe (shared trip id)
                BusTrip newTrip = new BusTrip(this, nextStart);
                generatedTrips.Add(newTrip);
                nextStart += Frequency;
            }
            return generatedTrips;
        }

        public override string ToString()
        {
            StringBuilder csb = new StringBuilder();
            csb.AppendLine(String.Format("Busline: {0}", id.ToString()));
            csb.AppendLine(String.Format("Startpoint:\n {0}", Start.ToString()));
            csb.AppendLine(String.Format("Endpoint:\n {0}", End.ToString()));
            csb.AppendLine(String.Format("LineStart: {0}, Frequency: {1}, No. Trips: {2}, Tripduration: {3}",
                FirstBusStartTime.ToString(),
                Frequency.ToString(),
                NumberOfTrips.ToString(),
                TripDuration.ToString()));

            String tsString = String.Empty;
            for (int i = 0; i < TripStations.Length; i++)
            {
                tsString += TripStations[i].ToString();
                if (i < TripStations.Length - 1)
                {
                    tsString += "-";
                }
            }
            csb.AppendLine(String.Format("Line: {0}", tsString));

            return csb.ToString();
        }

        public String plotLine()
        {
            StringBuilder csb = new StringBuilder();
            string fstr = "{0};{1};{2}";
            for (int i = 0; i < TripStations.Length-1; i++)
            {
                csb.AppendLine(String.Format(fstr, 
                    _tripStations[i].id.ToString(), 
                    _tripStations[i + 1].id.ToString(),
                    id.ToString()));
            }
            return csb.ToString();
        }

    }
}
