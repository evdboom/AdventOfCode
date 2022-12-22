using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using System.Drawing;

namespace AdventOfCode2022.Days
{
    public class Day22 : Day
    {
        public int CubeSize { get; set; } = 50;
        public Dictionary<(int Face, int Facing), (int Face, int Facing, Func<Point,Point> Translation)> FaceMappings { get; set; } = new()
        {
            { (1, 0), (2, 0, (p) => p with { X = 0 }) },
            { (1, 1), (3, 1, (p) => p with { Y = 0 }) },
            { (1, 2), (4, 0, (p) => p with { Y = CubeSize - p.Y - 1 }) },
            { (1, 3), (6, 0, (p) => p with { X = 0, Y = p.X }) },

            { (2, 0), (5, 2, (p) => p with { X = CubeSize - 1, Y = CubeSize - p.Y - 1 }) },
            { (2, 1), (3, 2, (p) => p with { X = CubeSize - 1, Y = p.X }) },
            { (2, 2), (1, 2, (p) => p with { X = CubeSize - 1 } )},
            { (2, 3), (6, 3, (p) => p with { Y = CubeSize -1 }) },

            { (3, 0), (2, 3, (p) => p with {  }) },
            { (3, 1), (5, 1, (p) => p with { }) },
            { (3, 2), (4, 1, (p) => p with { }) },
            { (3, 3), (1, 3, (p) => p with { }) },

            { (4, 0), (5, 0, (p) => p with { }) },
            { (4, 1), (6, 1, (p) => p with { }) },
            { (4, 2), (1, 0, (p) => p with { }) },
            { (4, 3), (3, 0, (p) => p with { }) },

            { (5, 0), (2, 2, (p) => p with { }) },
            { (5, 1), (6, 2, (p) => p with { }) },
            { (5, 2), (4, 2, (p) => p with { }) },
            { (5, 3), (3, 3, (p) => p with { }) },

            { (6, 0), (5, 3, (p) => p with { }) },
            { (6, 1), (2, 1, (p) => p with { }) },
            { (6, 2), (1, 1, (p) => p with { }) },
            { (6, 3), (4, 3, (p) => p with { }) },
        };

        public Day22(IFileImporter importer) : base(importer)
        {
        }

        public override int DayNumber => 22;
        protected override long ProcessPartOne(string[] input)
        {
            var grid = GetGrid(input, out int startingColumn);
            var instructions = GetInstructions(input);
            var instructionPoint = 0;
            (Point position, int facing) currentPosition = (new Point(startingColumn, 0), 0);

            while(instructionPoint < instructions.Length) 
            {
                var instruction = GetNextInstruction(instructions, instructionPoint);
                instructionPoint += instruction.Length;

                if (int.TryParse(instruction, out int steps))
                {
                    for(int i = 0; i < steps; i++) 
                    {
                        var nextPosition = GetNextPosition(currentPosition, grid);
                        if (grid[nextPosition.X, nextPosition.Y] == '.')
                        {
                            currentPosition.position = nextPosition;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    currentPosition.facing = instruction switch
                    {
                        "R" => (currentPosition.facing + 1) % 4,
                        "L" => (currentPosition.facing + 3) % 4,
                        _ => throw new InvalidOperationException("Unknown rotation")
                    }; ;
                }

            }

            return (currentPosition.position.Y + 1) * 1000 + (currentPosition.position.X + 1) * 4 + currentPosition.facing;
        }

        protected override long ProcessPartTwo(string[] input)
        {
            var grid = GetGrid(input, out int startingColumn);
            var faces = GetFaces(grid);
            var instructions = GetInstructions(input);
            var instructionPoint = 0;
            (Point position, int face, int facing) currentPosition = (new Point(GetFirstValidColumn(faces[1]), 0), 1, 0);

            while (instructionPoint < instructions.Length)
            {
                var instruction = GetNextInstruction(instructions, instructionPoint);
                instructionPoint += instruction.Length;

                if (int.TryParse(instruction, out int steps))
                {
                    for (int i = 0; i < steps; i++)
                    {
                        var nextPosition = GetNextPosition(currentPosition, faces);
                        if (grid[nextPosition.X, nextPosition.Y] == '.')
                        {
                            currentPosition.position = nextPosition;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    currentPosition.facing = instruction switch
                    {
                        "R" => (currentPosition.facing + 1) % 4,
                        "L" => (currentPosition.facing + 3) % 4,
                        _ => throw new InvalidOperationException("Unknown rotation")
                    }; ;
                }

            }

            return (currentPosition.position.Y + 1) * 1000 + (currentPosition.position.X + 1) * 4 + currentPosition.facing;
        }

        private int GetFirstValidColumn((char[,] Grid, Point StartPoint) value)
        {
            for (int i = 0; < value.Grid.GetLength(0); i++)
            {
                if (value.Grid == '.')
                {
                    return i;
                }
            }
        }

        private Dictionary<int, (char[,] Grid, Point StartPoint)> GetFaces(char[,] grid)
        {
            var result = new Dictionary<int, char[,]>();
            for (int faceNumber = 1; faceNumber < 7; faceNumber++)
            {
                var startPoint = GetStartPoint(grid, faceNumber);
                var face = new char[CubeSize, CubeSize];
                for (int j = 0; j < CubeSize; j++)
                {
                    for (int i = 0; i < CubeSize; i++)
                    {
                        face[i, j] = grid[i + startPoint.X, j + startPoint.Y];
                    }
                }
                result[faceNumber] = (face, startPoint);
            }

            return result;
        }

        private Point GetStartPoint(char[,] grid, int faceNumber)
        {
            var result = Point.Empty;
            var found = 0;
            var index = 0;
            var perLine = grid.GetLength(0) / CubeSize;
            while (found < faceNumber)
            {
                result.X = (index % perLine) * CubeSize;
                result.Y = (index / perLine) * CubeSize;
                if (grid[result.X, result.Y] != ' ')
                {
                    found++;
                }
                index++;
            }

            return result;
        }

        private (Point Point, int Face, int Facing) GetNextPosition((Point position, int face, int facing) currentPosition, Dictionary<int,(char[,] Grid, Point StartPoint)> faces)
        {
            var grid = faces[currentPosition.face].Grid;
            switch (currentPosition.facing)
            {
                case 0:
                    if (currentPosition.position.X + 1 < grid.GetLength(0))
                    {
                        var newPoint = currentPosition.position with                        
                        {
                            X = currentPosition.position.X + 1
                        };
                        return (newPoint, currentPosition.face, currentPosition.facing);
                    }
                    else
                    {
                        var (Face, Facing) = FaceMappings[(currentPosition.face, currentPosition.facing)];



                    }
                case 1:
                    if (currentPosition.position.Y + 1 < grid.GetLength(1) && grid[currentPosition.position.X, currentPosition.position.Y + 1] != ' ')
                    {
                        return currentPosition.position with
                        {
                            Y = currentPosition.position.Y + 1
                        };
                    }
                    else
                    {
                        int index = 0;
                        while (index < currentPosition.position.Y)
                        {
                            if (grid[currentPosition.position.X, index] != ' ')
                            {
                                return currentPosition.position with
                                {
                                    Y = index
                                };
                            }
                            index++;
                        }
                        throw new InvalidOperationException("no valid point found");
                    }
                case 2:
                    if (currentPosition.position.X - 1 >= 0 && grid[currentPosition.position.X - 1, currentPosition.position.Y] != ' ')
                    {
                        return currentPosition.position with
                        {
                            X = currentPosition.position.X - 1
                        };
                    }
                    else
                    {
                        int index = grid.GetLength(0) - 1;
                        while (index > currentPosition.position.X)
                        {
                            if (grid[index, currentPosition.position.Y] != ' ')
                            {
                                return currentPosition.position with
                                {
                                    X = index
                                };
                            }
                            index--;
                        }
                        throw new InvalidOperationException("no valid point found");
                    }
                case 3:
                    if (currentPosition.position.Y - 1 >= 0 && grid[currentPosition.position.X, currentPosition.position.Y - 1] != ' ')
                    {
                        return currentPosition.position with
                        {
                            Y = currentPosition.position.Y - 1
                        };
                    }
                    else
                    {
                        int index = grid.GetLength(1) - 1;
                        while (index > currentPosition.position.Y)
                        {
                            if (grid[currentPosition.position.X, index] != ' ')
                            {
                                return currentPosition.position with
                                {
                                    Y = index
                                };
                            }
                            index--;
                        }
                        throw new InvalidOperationException("no valid point found");
                    }
                default:
                    throw new InvalidOperationException("no known facing");
            }
        }

        private Point GetNextPosition((Point position, int facing) currentPosition, char[,] grid)
        {
            switch(currentPosition.facing)
            {
                case 0:
                    if (currentPosition.position.X + 1 < grid.GetLength(0) && grid[currentPosition.position.X + 1, currentPosition.position.Y] != ' ')
                    {
                        return currentPosition.position with
                        {
                            X = currentPosition.position.X + 1
                        };
                    }
                    else
                    {
                        int index = 0;
                        while(index < currentPosition.position.X)
                        {
                            if (grid[index, currentPosition.position.Y] != ' ')
                            {
                                return currentPosition.position with
                                {
                                    X = index
                                };
                            }
                            index++;
                        }
                        throw new InvalidOperationException("no valid point found");                                                
                    }
                case 1:
                    if (currentPosition.position.Y + 1 < grid.GetLength(1) && grid[currentPosition.position.X, currentPosition.position.Y + 1] != ' ')
                    {
                        return currentPosition.position with
                        {
                            Y = currentPosition.position.Y + 1
                        };
                    }
                    else
                    {
                        int index = 0;
                        while (index < currentPosition.position.Y)
                        {
                            if (grid[currentPosition.position.X, index] != ' ')
                            {
                                return currentPosition.position with
                                {
                                    Y = index
                                };
                            }
                            index++;
                        }
                        throw new InvalidOperationException("no valid point found");
                    }
                case 2:
                    if (currentPosition.position.X - 1 >= 0 && grid[currentPosition.position.X - 1, currentPosition.position.Y] != ' ')
                    {
                        return currentPosition.position with
                        {
                            X = currentPosition.position.X - 1
                        };
                    }
                    else
                    {
                        int index = grid.GetLength(0) - 1;
                        while (index > currentPosition.position.X)
                        {
                            if (grid[index, currentPosition.position.Y] != ' ')
                            {
                                return currentPosition.position with
                                {
                                    X = index
                                };
                            }
                            index--;
                        }
                        throw new InvalidOperationException("no valid point found");
                    }
                case 3:
                    if (currentPosition.position.Y - 1 >= 0 && grid[currentPosition.position.X, currentPosition.position.Y - 1] != ' ')
                    {
                        return currentPosition.position with
                        {
                            Y = currentPosition.position.Y - 1
                        };
                    }
                    else
                    {
                        int index = grid.GetLength(1) - 1;
                        while (index > currentPosition.position.Y)
                        {
                            if (grid[currentPosition.position.X, index] != ' ')
                            {
                                return currentPosition.position with
                                {
                                    Y = index
                                };
                            }
                            index--;
                        }
                        throw new InvalidOperationException("no valid point found");
                    }
                default:
                    throw new InvalidOperationException("no known facing");
            }
        }

        private string GetNextInstruction(string instructions, int instructionPoint)
        {
            var instruction = $"{instructions[instructionPoint]}";
            if (!int.TryParse(instruction, out _))
            {
                return instruction;
            }

            return instructions[instructionPoint..]
                .Split('L', 'R')[0];

        }

        private string GetInstructions(string[] input)
        {
            return input[input.Length - 1];
        }

        private char[,] GetGrid(string[] input, out int startingColumn)
        {
            startingColumn = 0;
            bool startSet = false;
            var list = input.ToList();
            list.RemoveRange(input.Length - 2, 2);
            var maxLength = list.Max(line => line.Length);
            var result = new char[maxLength, list.Count];
            for (int j = 0; j < result.GetLength(1); j++)
            {
                for (int i = 0; i < result.GetLength(0); i ++)
                {
                    var set = i >= input[j].Length
                        ? ' '
                        : input[j][i];

                    if (!startSet && set == '.')
                    {
                        startingColumn = i;
                        startSet = true;
                    }

                    result[i, j] = set;
                }
            }

            return result;
        }
    }
}