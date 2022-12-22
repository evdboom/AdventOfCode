using AdventOfCode.Shared.Days;
using AdventOfCode.Shared.Services;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode2022.Days
{
    public class Day22 : Day
    {
        public int CubeSize { get; set; } = 50;
        public Dictionary<(int Face, int Facing), (int Face, int Facing, Func<int, Point,Point> Translation)> FaceMappings { get; set; } = new()
        {
            { (1, 0), (2, 0, (c, p) => p with { X = 0}) },
            { (1, 1), (3, 1, (c, p) => p with { Y = 0 }) },
            { (1, 2), (4, 0, (c, p) => p with { Y = c - p.Y - 1 }) },
            { (1, 3), (6, 0, (c, p) => p with { X = 0, Y = p.X }) },

            { (2, 0), (5, 2, (c, p) => p with { X = c - 1, Y = c - p.Y - 1 }) },
            { (2, 1), (3, 2, (c, p) => p with { X = c - 1, Y = p.X }) },
            { (2, 2), (1, 2, (c, p) => p with { X = c - 1 } )},
            { (2, 3), (6, 3, (c, p) => p with { Y = c - 1 }) },

            { (3, 0), (2, 3, (c, p) => p with { X = p.Y, Y = c - 1 }) },
            { (3, 1), (5, 1, (c, p) => p with { Y = 0 }) },
            { (3, 2), (4, 1, (c, p) => p with { X = p.Y, Y = 0 }) },
            { (3, 3), (1, 3, (c, p) => p with { Y = c - 1 }) },

            { (4, 0), (5, 0, (c, p) => p with { X = 0 }) },
            { (4, 1), (6, 1, (c, p) => p with { Y = 0 }) },
            { (4, 2), (1, 0, (c, p) => p with { X = 0, Y = c - 1 - p.Y }) },
            { (4, 3), (3, 0, (c, p) => p with { X = 0, Y = p.X }) },

            { (5, 0), (2, 2, (c, p) => p with { X = c - 1, Y = c - 1 - p.Y }) },
            { (5, 1), (6, 2, (c, p) => p with { X = c - 1, Y = p.X }) },
            { (5, 2), (4, 2, (c, p) => p with { X = c - 1 }) },
            { (5, 3), (3, 3, (c, p) => p with { Y = c - 1 }) },

            { (6, 0), (5, 3, (c, p) => p with { Y = c - 1, X = p.Y }) },
            { (6, 1), (2, 1, (c, p) => p with { Y = 0 }) },
            { (6, 2), (1, 1, (c, p) => p with { X = p.Y, Y = 0 }) },
            { (6, 3), (4, 3, (c, p) => p with { Y = c - 1 }) },
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
                        if (faces[nextPosition.Face].Grid[nextPosition.Point.X, nextPosition.Point.Y] == '.')
                        {
                            currentPosition.position = nextPosition.Point;
                            currentPosition.facing = nextPosition.Facing;
                            currentPosition.face = nextPosition.Face;
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

            return (currentPosition.position.Y + 1 + faces[currentPosition.face].StartPoint.Y) * 1000 + (currentPosition.position.X + 1 + faces[currentPosition.face].StartPoint.X) * 4 + currentPosition.facing;
        }

        private int GetFirstValidColumn((char[,] Grid, Point StartPoint) value)
        {
            for (int i = 0; i < value.Grid.GetLength(0); i++)
            {
                if (value.Grid[i, 0] == '.')
                {
                    return i;
                }
            }

            throw new InvalidOperationException("No valid column found");
        }

        private Dictionary<int, (char[,] Grid, Point StartPoint)> GetFaces(char[,] grid)
        {
            var result = new Dictionary<int, (char[,] Grid, Point StartPoint)>();
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
                        var (Face, Facing, Func) = FaceMappings[(currentPosition.face, currentPosition.facing)];
                        var newPoint = Func(CubeSize, currentPosition.position with { X = currentPosition.position.X + 1 });
                        return (newPoint, Face, Facing);
                    }
                case 1:
                    if (currentPosition.position.Y + 1 < grid.GetLength(1))
                    {
                        var newPoint = currentPosition.position with
                        {
                            Y = currentPosition.position.Y + 1
                        };
                        return (newPoint, currentPosition.face, currentPosition.facing);
                    }
                    else
                    {
                        var (Face, Facing, Func) = FaceMappings[(currentPosition.face, currentPosition.facing)];
                        var newPoint = Func(CubeSize, currentPosition.position with { Y = currentPosition.position.Y + 1 });
                        return (newPoint, Face, Facing);
                    }
                case 2:
                    if (currentPosition.position.X - 1 >= 0)
                    {
                        var newPoint = currentPosition.position with
                        {
                            X = currentPosition.position.X - 1
                        };
                        return (newPoint, currentPosition.face, currentPosition.facing);
                    }
                    else
                    {
                        var (Face, Facing, Func) = FaceMappings[(currentPosition.face, currentPosition.facing)];
                        var newPoint = Func(CubeSize, currentPosition.position with { X = 0 });
                        return (newPoint, Face, Facing);
                    }
                case 3:
                    if (currentPosition.position.Y - 1 >= 0)
                    {
                        var newPoint = currentPosition.position with
                        {
                            Y = currentPosition.position.Y - 1
                        };
                        return (newPoint, currentPosition.face, currentPosition.facing);
                    }
                    else
                    {
                        var (Face, Facing, Func) = FaceMappings[(currentPosition.face, currentPosition.facing)];
                        var newPoint = Func(CubeSize,currentPosition.position with { Y = 0 });
                        return (newPoint, Face, Facing);
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