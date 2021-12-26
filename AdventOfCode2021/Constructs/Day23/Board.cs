namespace AdventOfCode2021.Constructs.Day23
{
    public record Board
    {
        public const char Amber = 'A';
        public const char Bronze = 'B';
        public const char Copper = 'C';
        public const char Desert = 'D';
        public const char Empty = '.';

        private static Dictionary<char, int> _amphipodToRoom = new()
        {
            { Amber, 2 },
            { Bronze, 4 },
            { Copper, 6 },
            { Desert, 8 }
        };

        private static Dictionary<char, int> _amphipodStepCost = new()
        {
            { Amber, 1 },
            { Bronze, 10 },
            { Copper, 100 },
            { Desert, 1000 }
        };

        // buggy
        public int Estimate => GetMiniumEnergy();

        public int Cost { get; init; }
        public string Hallway { get; init; }
        public Dictionary<int, string> Rooms { get; init; }

        public Board(string hallway, string amberRoom, string bronzeRoom, string copperRoom, string desertRoom)
        {
            Hallway = hallway;
            Rooms = new();
            Rooms[_amphipodToRoom[Amber]] = amberRoom;
            Rooms[_amphipodToRoom[Bronze]] = bronzeRoom;
            Rooms[_amphipodToRoom[Copper]] = copperRoom;
            Rooms[_amphipodToRoom[Desert]] = desertRoom;
        }        

        public IEnumerable<Board> ValidMoves()
        {
            foreach(var move in MovesFromHallway())
            {
                yield return move;
            }
            foreach(var move in ValidMovesFromRooms())
            {
                yield return move;
            }

        }

        private IEnumerable<Board> ValidMovesFromRooms()
        {
            foreach (var room in Rooms)
            {
                var firstAmphipodIndex = room.Value.LastIndexOf(Empty) + 1;
                if (firstAmphipodIndex == room.Value.Length)
                {
                    // empty room;
                    continue;
                }

                if (firstAmphipodIndex == 0 && room.Value.All(r => room.Key == _amphipodToRoom[r]))
                {
                    // finished room;
                    continue;
                }

                var firstAmphipod = room.Value[firstAmphipodIndex];
                var depth = firstAmphipodIndex + 1;
                var updatedRoom = room.Value.Remove(firstAmphipodIndex, 1).Insert(firstAmphipodIndex, $"{Empty}");
                for (int i = room.Key + 1; i < Hallway.Length; i++)
                {
                    if (Rooms.ContainsKey(i))
                    {
                        // cannot stop directly outside room;
                        continue;
                    }
                    if (Hallway[i] != Empty)
                    {
                        // found blocking amphipod
                        break;
                    }
                    var path = i - room.Key;
                    var addedCost = (depth + path) * _amphipodStepCost[firstAmphipod];

                    yield return this with
                    {
                        Cost = Cost + addedCost,
                        Hallway = Hallway.Remove(i, 1).Insert(i, $"{firstAmphipod}"),
                        Rooms = Rooms.ToDictionary(r => r.Key, r => r.Key == room.Key ? updatedRoom : r.Value)
                    };
                }
                for (int i = room.Key - 1; i >= 0; i--)
                {
                    if (Rooms.ContainsKey(i))
                    {
                        // cannot stop directly outside room;
                        continue;
                    }
                    if (Hallway[i] != Empty)
                    {
                        // found blocking amphipod
                        break;
                    }
                    var path = room.Key - i;
                    var addedCost = (depth + path) * _amphipodStepCost[firstAmphipod];

                    yield return this with
                    {
                        Cost = Cost + addedCost,
                        Hallway = Hallway.Remove(i, 1).Insert(i, $"{firstAmphipod}"),
                        Rooms = Rooms.ToDictionary(r => r.Key, r => r.Key == room.Key ? updatedRoom : r.Value)
                    };
                }
            }
        }

        private int GetMiniumEnergy()
        {
            int result = 0;
            result += MinimumFromHallway();
            result += MinimumFromRooms();

            return result;
        }

        private int MinimumFromHallway()
        {
            int result = 0;
            for (int i = 0; i < Hallway.Length; i++)
            {
                if (Hallway[i] == Empty)
                {
                    continue;
                }

                var distance = Math.Abs(i - _amphipodToRoom[Hallway[i]]);
                result += (distance + 1) * _amphipodStepCost[Hallway[i]];
            }
                
            return result;
        }

        private int MinimumFromRooms()
        {
            int result = 0;
            foreach(var room in Rooms)
            {
                for (int i = room.Value.LastIndexOf(Empty) + 1; i < room.Value.Length; i++)
                {
                    var distance = Math.Abs(room.Key - _amphipodToRoom[room.Value[i]]);
                    var depth = i + 1;

                    result += (distance + depth + 1) * _amphipodStepCost[room.Value[i]];
                }
            }

            return result;
        }

        public bool Wins()
        {
            return 
                Hallway.All(h => h == Empty) &&
                Rooms.All(r => r.Value.All(a => r.Key == _amphipodToRoom[a]));
        }

        private IEnumerable<Board> MovesFromHallway()
        {
            for (int i = 0; i < Hallway.Length; i++)
            {
                var amphipod = Hallway[i];
                if (amphipod == Empty)
                {
                    // Empty spot
                    continue;
                }

                var wantedRoomIndex = _amphipodToRoom[amphipod];
                var wantedRoom = Rooms[wantedRoomIndex];

                var path = Hallway.Substring(Math.Min(i + 1, wantedRoomIndex), Math.Abs(wantedRoomIndex - i));
                if (path.Any(p => p != Empty) || wantedRoom.Any(r => r != amphipod && r != Empty))
                {
                    // Path is blocked, or room can't receive correct amphipods yet.
                    continue;
                }

                var depth = wantedRoom.LastIndexOf(Empty) + 1;
                var addedCost = (path.Length + depth) * _amphipodStepCost[amphipod];

                var updatedRoom = wantedRoom.Remove(depth - 1, 1).Insert(depth - 1, $"{amphipod}");

                yield return this with
                {
                    Cost = Cost + addedCost,
                    Hallway = Hallway.Remove(i, 1).Insert(i, $"{Empty}"),
                    Rooms = Rooms.ToDictionary(r => r.Key, r => r.Key == wantedRoomIndex ? updatedRoom : r.Value)                    
                };
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                Hallway.GetHashCode(), 
                Rooms[_amphipodToRoom[Amber]].GetHashCode(),
                Rooms[_amphipodToRoom[Bronze]].GetHashCode(), 
                Rooms[_amphipodToRoom[Copper]].GetHashCode(),
                Rooms[_amphipodToRoom[Desert]].GetHashCode());
        }

        public override string ToString()
        {
            return $"{Hallway} {Rooms[_amphipodToRoom[Amber]]} {Rooms[_amphipodToRoom[Bronze]]} {Rooms[_amphipodToRoom[Copper]]} {Rooms[_amphipodToRoom[Desert]]} ({Cost}, {Estimate})";
        }
    }
}
