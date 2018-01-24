using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomBusNetwork
{
    public class Network 
    {
        private Random _rnd;
        private List<BusTrip> _trips;
        private List<BusLine> _lines;
        private List<Station> _stations;
        private int _numOfLines;
        private int _numOfStations;

        //Properties
        public List<BusLine> BusLines
        {
            get
            {
                if (_lines != null)
                {
                   return _lines; 
                }
                else
                {
                    return new List<BusLine>();
                }
                
            }
        }
        public List<Station> Stations
        {
            get
            {
                if (_stations != null)
                {
                    return _stations;
                }
                else
                {
                    return new List<Station>();
                }
            }
        }
        public List<BusTrip> BusTrips
        {
            get
            {
                if (_trips != null)
                {
                    return _trips;
                }
                else
	            {
                    return new List<BusTrip>();
	            }
            }
        }

        //CTOR
        /// <summary>
        /// Create a new random Bus network
        /// </summary>
        /// <param name="num_lines">Number of different bus lines</param>
        /// <param name="num_stations">Number of Stations</param>
        /// <param name="x_width">width of plane</param>
        /// <param name="y_width">height of plane</param>
        /// <param name="perc_reliefpoints">Percentage of stations that act as reliefpoints (0-100)</param>
        public Network(int num_lines, int num_stations, int x_width, int y_width, int perc_reliefpoints)
        {
            _rnd = new Random();
            _trips = new List<BusTrip>();
            _lines = new List<BusLine>();
            _stations = new List<Station>();


            int[] x_axis;
            int[] y_axis;
            if (x_width < 1 || y_width < 1)
            {
                int[] Axis = {0,25};
                x_axis = Axis;
                y_axis = Axis;
            }
            else
            {
                x_axis = new int[2];
                y_axis = new int[2];
                x_axis[0] = 0;
                x_axis[1] = x_width;
                y_axis[0] = 0;
                y_axis[1] = y_width;
            }
            
            int percRelief = (perc_reliefpoints >= 0 && perc_reliefpoints <= 100) ? perc_reliefpoints : 30;
            _numOfLines = (num_lines > 0) ? num_lines : 5;
            _numOfStations = (num_stations > 1) ? num_stations : 50;
            //generate Stations, Lines, Trips
            _stations = generateStations(x_axis, y_axis, percRelief,5);
            _lines = generateBusLines();
            _trips = generateBustrips();
        }

        private List<Station> newRandomLine()
        {
            int[] StationRange = { 0, _stations.Count()-1 };
            //Number of Stations to visit during trip
            //at least 2 or more Stations, i.e. 10% of all stations
            int minNumOfStations = (int)(Math.Max(2, Math.Round(0.1 * _stations.Count, 0)));
            //maximum 50% of stations, but >= 5 Stations
            int maxNumOfStations = (int)(Math.Max(5, Math.Round(0.3 * _stations.Count, 0)));
            //random number of stations in this interval
            int rnd_numOfStationsInLine = _rnd.Next(minNumOfStations, maxNumOfStations);

            //determine order of visited Stations
            //int[] visitedStations = new int[rnd_numOfStationsInLine];
            List<Station> visited_Stations = new List<Station>();

            //Starting point of line
            int k = _rnd.Next(StationRange[0], StationRange[1]);
            visited_Stations.Add(_stations[k]);
            Station currentStat = visited_Stations[0];
            //loop over potential successors, use nearest or 2nd nearest for continuation
            while (visited_Stations.Count() <= rnd_numOfStationsInLine)
            {
                int currentDist = int.MaxValue;
                Station nextStat=null;
                int dist;
                Station next_option =null;

                //add nearest neighbour
                foreach (Station ps in _stations)
                {
                    dist = currentStat.DistanceTo(ps);
                    if (!visited_Stations.Contains(ps) && dist < currentDist)
                    {
                        currentDist = dist;
                        next_option = nextStat;
                        nextStat = ps;
                    }
                }

                //choose nearest or second-nearest next station
                if (nextStat != null && next_option != null)
                {
                    if (_rnd.NextDouble() > 0.5)
                    {
                        currentStat = next_option;
                    }
                    else
                    {
                        currentStat = nextStat;
                    }
                } //choose nearest neighbour
                else if (nextStat != null)
                {
                    currentStat = nextStat;
                } //no neighbour found
                else
                {
                    throw new Exception("could not find next station!");
                }

                visited_Stations.Add(currentStat);
            }
            return visited_Stations;
        }

        private int calcLineTime(List<Station> visitedStations)
        {
            //Generate trip duration and visited Stations:
            int duration = 0;

            //determine total Traveltime
            for (int j = 0; j < visitedStations.Count()-1; j++)
            {
                Station s1 = visitedStations[j];
                Station s2 = visitedStations[j + 1];
                duration += s1.TravelTimeTo(s2);
            }
            return duration;
        }

        private int calcLatestStartTime(int HorizonEnd, int numOfTrips,int singleTripDuration, int freqency)
        {
            return HorizonEnd - singleTripDuration - ((numOfTrips - 1) * freqency);
        }

        private List<Station> generateStations(int[] xborder, int[] yborder, int perc_relief, int constraining_distance)
        {
            var newList = new List<Station>();

            if (_numOfStations < 2)
            {
                return newList;
            }

            for (int i = 0; i < _numOfStations; i++)
            {
                int x_coord;
                int y_coord;
                int loops = 0;
                //generate Location
                do
                {
                    loops++;
                    x_coord = _rnd.Next(xborder[0], xborder[1]);
                    y_coord = _rnd.Next(yborder[0], yborder[1]);
                    if (loops > 100)
                    {
                        Console.WriteLine("problems finding Station locations");
                    }
                } while (loops < 200 && isNear(y_coord, y_coord,constraining_distance));

                //determine if reliefpoint
                bool rp = (_rnd.NextDouble() <= (perc_relief/100));
                Station newStation = new Station(x_coord, y_coord, rp);
                newList.Add(newStation);
            }

            return newList;
        }

        private bool isNear(int x, int y,int inDistance)
        {
            bool r = false;
            r = (_stations.Where( (s) => (s.DistanceTo(x, y) <= inDistance)).Count() > 0);
            return r;
        }

        private List<BusLine> generateBusLines()
        {
            List<BusLine> result = new List<BusLine>();

            if (_stations == null || _stations.Count < 5)
            {
                return result;
            }

            for (int i = 0; i < _numOfLines; i++)
            {
                //random frequency
                int rnd_freq = _rnd.Next(5, 15);
                //random number of trips
                int rnd_numOfTrips = _rnd.Next(5, 20);
                //get a new BusLine:
                var visitedStations = newRandomLine();
                //calculate time for each bustrip of line
                int rnd_tripDuration = calcLineTime(visitedStations);
                //latest starting time of line given trip duration, number of trips and frequency (infeasibility possible)
                int latest_starting_point = calcLatestStartTime(288, rnd_numOfTrips, rnd_tripDuration, rnd_freq);
                int h = 0;
                //reduce number of trips or frequency if infeasible starting time for line occurs
                while (latest_starting_point < 1)
                {
                    //20% chance to reduce freqency, 80% chance to reduce number of trips
                    if (_rnd.NextDouble() > 0.8)
                    {
                        if (rnd_freq > 5)
                        {
                            rnd_freq--;
                        }
                    }
                    else
                    {
                        if (rnd_numOfTrips > 3)
                        {
                            rnd_numOfTrips--;
                        }
                    }
                    //recalc
                    latest_starting_point = calcLatestStartTime(288, rnd_numOfTrips, rnd_tripDuration, rnd_freq);
                    //change BusLine if needed
                    if (h > 100)
                    {
                        visitedStations = newRandomLine();
                        rnd_tripDuration = calcLineTime(visitedStations);
                        rnd_numOfTrips = _rnd.Next(5, 20);
                        rnd_freq = _rnd.Next(5, 15);
                    }
                    h++;
                }

                int rnd_firstStart = _rnd.Next(0,latest_starting_point);

                BusLine newBusLine = new BusLine(
                    visitedStations[0],
                    visitedStations[visitedStations.Count()-1],
                    rnd_freq,
                    rnd_firstStart,
                    rnd_numOfTrips,
                    rnd_tripDuration,
                    visitedStations);

                result.Add(newBusLine);
            }
            return result;
        }

        private List<BusTrip> generateBustrips()
        {
            List<BusTrip> resultlist = new List<BusTrip>();

            if (_lines == null || _lines.Count < 1)
            {
                return resultlist;
            }

            foreach (BusLine bl in _lines)
            {
                resultlist.AddRange(bl.MyTrips);
            }

            return resultlist;
        }

        public string plotStations()
        {
            StringBuilder csb = new StringBuilder();
            csb.AppendLine("Station_id;X_coord;Y_coord;Relief");
            string fstr = "{0};{1};{2};{3}";
            foreach (Station item in _stations)
            {
                csb.AppendLine(String.Format(fstr,
                    item.id.ToString(),item.x.ToString(),item.y.ToString(),item.isReliefPoint.ToString()
                    ));
            }
            return csb.ToString();
        }

        public string plotLines()
        {
            StringBuilder csb = new StringBuilder();
            csb.AppendLine("s1;s2;line");
            foreach (BusLine item in _lines)
            {
                csb.Append(item.plotLine());
            }
            return csb.ToString();
        }

        //public double calc_deadhead_costs(BusTrip trip1, BusTrip trip2)
        //{

        //}

    }
}
