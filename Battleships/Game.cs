using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    // Imagine a game of battleships.
    //   The player has to guess the location of the opponent's 'ships' on a 10x10 grid
    //   Ships are one unit wide and 2-4 units long, they may be placed vertically or horizontally
    //   The player asks if a given co-ordinate is a hit or a miss
    //   Once all cells representing a ship are hit - that ship is sunk.
    public class Game
    {
        // ships: each string represents a ship in the form first co-ordinate, last co-ordinate
        //   e.g. "3:2,3:5" is a 4 cell ship horizontally across the 4th row from the 3rd to the 6th column
        // guesses: each string represents the co-ordinate of a guess
        //   e.g. "7:0" - misses the ship above, "3:3" hits it.
        // returns: the number of ships sunk by the set of guesses
        public static int Play(string[] ships, string[] guesses)
        {

            try
            {
                var shipsLt = new List<Ship>();
                foreach (var sh in ships)
                {
                    if (Ship.TryCreateShip(sh, out var ship, out var errorMsg))
                    {
                        shipsLt.Add(ship);
                    }
                    else
                    {
                        Console.WriteLine(errorMsg);
                    }
                }

                //Hit the ships

                int hit = 0;

                foreach (var guess in guesses)
                {
                    foreach (var ship in shipsLt)
                    {
                        ship.Hit(guess);
                    }
                }

                //Get the ships sunk
                var shipsSunk = shipsLt.Where(s => s.IsSunk());

                return shipsSunk.Count();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return -1;
        }
    }

    public class ShipCoordinate
    {
        public bool IsHit { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Ship
    {
        

        private readonly ShipCoordinate _c1;
        private readonly ShipCoordinate _c2;

        private readonly Dictionary<string, ShipCoordinate> _coordinateArrayLt;


        public void Hit(string coordinate)
        {
            if(_coordinateArrayLt.ContainsKey(coordinate))
            {
                _coordinateArrayLt[coordinate].IsHit = true;
            }
        }

        public bool IsSunk()
        {
            //if any of the coordinate is not hit then ship is staying afloat
            if (_coordinateArrayLt.Any(kv => !kv.Value.IsHit))
                return false;
            return true;
        }

        private Ship(ShipCoordinate c1, ShipCoordinate c2)
        {
            _c1 = c1;
            _c2 = c2;
            _coordinateArrayLt = new Dictionary<string, ShipCoordinate>();
            if (_c1.X == _c2.X)
            {
                for(int i=_c1.Y; i <= _c2.Y; i++)
                {
                    _coordinateArrayLt.Add( $"{_c1.X}:{i}", new ShipCoordinate { X = _c1.X, Y = i });
                }
            }
            if (_c1.Y == _c2.Y)
            {
                for (int i = _c1.X; i <= _c2.X; i++)
                {
                    _coordinateArrayLt.Add($"{i}:{_c1.Y}" ,new ShipCoordinate { X = i, Y = _c1.Y });
                }
            }
        }

        /// <summary>
        /// Parses the ship co-ordinate and validates the ships size and then creates ship object
        /// </summary>
        /// <param name="ships"></param>
        /// <param name="ship"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool TryCreateShip(string shipCoordinate, out Ship ship, out string errorMsg)
        {
            bool isSuccessFul = false;
            errorMsg = string.Empty;
            var coordinateArray = shipCoordinate.Split(',');

            //Parse the co-ordinates
            var typeCoordinate = coordinateArray.Select(c =>
            {
                var data = c.Split(':');
                if(data.Length == 2)
                {
                   
                    if (int.TryParse(data[0], out int x))
                    {
                        if (int.TryParse(data[1], out int y))
                        {
                            return new ShipCoordinate { X = x, Y = y };
                        }
                    }
                    
                }

                return null;

            }).Where(c => c != null).ToArray();

            //if there are less than 2 typed coordinates then something wrong with the shipCoordinate string
            if(typeCoordinate.Length == 2)
            {
                //Check the size of the ship
                //one unit wide and 2-4 units long
                var c1 = typeCoordinate[0];
                var c2 = typeCoordinate[1];

                bool isSizeValid = false;

                int len = 0;

                if (c1.X == c2.X) //Parked Horizontally
                {
                    len = Math.Abs(c2.Y - c1.Y) + 1;// - 1);

                    if (len >= 1 && len <= 4)
                    {
                        isSizeValid = true;
                    }
                    else
                    {
                        errorMsg = $"Invalid Ship Length {len}: Ship {shipCoordinate}";
                    }

                }
                else if (c1.Y == c2.Y) // Parked Vertically
                {
                    len = Math.Abs(c2.X - c1.X) + 1;// - 1);

                    if (len >= 1 && len <= 4)
                    {
                        isSizeValid = true;
                    }
                    else
                    {
                        errorMsg = $"Invalid Ship Length {len}: Ship {shipCoordinate}";
                    }
                }
                else
                {
                    isSizeValid = false;

                    errorMsg = $"Invalid Invalid Ship-Coordinate value : {shipCoordinate}. Neither parked Horizontally or Vertically";
                }

                if(isSizeValid)
                {
                    ship = new Ship(typeCoordinate[0], typeCoordinate[1]);
                    isSuccessFul = true;
                    errorMsg = string.Empty;

                    return isSuccessFul;
                }
                
            }
            
            ship = null;
            errorMsg = string.IsNullOrWhiteSpace(errorMsg)?  $"Invalid Ship-Coordinate value : {shipCoordinate}": errorMsg;
            

            return isSuccessFul;
        }

        
    }
}
