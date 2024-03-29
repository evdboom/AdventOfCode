﻿using AdventOfCode.Shared.Services;

namespace AdventOfCode2021.Constructs.Day18
{
    public class SnailNumber
    {
        public SnailNumber? Parent { get; set; }

        private SnailNumber? _left;
        public SnailNumber? Left 
        {
            get => _left;
            set
            {
                _left = value;
                if (_left is not null)
                {
                    _left.IsLeft = true;
                    _left.Parent = this;
                    _left.Depth = Depth;
                    _left.AddDepth();
                }
            }
        }

        private SnailNumber? _right;
        public SnailNumber? Right
        {
            get => _right;
            set
            {
                _right = value;
                if (_right is not null)
                {
                    _right.IsRight = true;
                    _right.Parent = this;
                    _right.Depth = Depth;
                    _right.AddDepth();
                }
            }
        }

        private bool _isLeft;
        public bool IsLeft 
        { 
            get => Parent is not null && _isLeft;
            set => _isLeft = value; 
        }
        public bool IsRight 
        {
            get => Parent is not null && !_isLeft;
            set => _isLeft = !value;
        }

        public int Depth { get; set; }
        public long LeftValue { get; set; }
        public long RightValue { get; set; }


        private void AddLeftValue(long value)
        {
            if (Left is not null)
            {
                Left.AddLeftValue(value);
            }
            else
            {
                LeftValue += value;
            }
        }

        private void AddRightValue(long value)
        {
            if (Right is not null)
            {
                Right.AddRightValue(value);
            }
            else
            {
                RightValue += value;
            }
        }

        public void AddDepth()
        {
            Depth++;
            Left?.AddDepth();
            Right?.AddDepth();            
        }

        public void Reduce(IScreenWriter writer)
        {
            writer.WriteLine(ToString());
            while(TryReduce())
            {
                writer.WriteLine(ToString());
            }
        }

        private bool TryReduce()
        {
            return TryExplode() || TrySplit();
        }

        public long GetMagnitude()
        {
            var left = Left?.GetMagnitude() ?? LeftValue;
            var right = Right?.GetMagnitude() ?? RightValue;                
            
            return left * 3 + right * 2;
        }
        
        private bool TryExplode()
        {
            if (Depth < 4)
            {
                if (Left is not null && Left.TryExplode())
                {
                    return true;
                }
                else if (Right is not null && Right.TryExplode())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (IsLeft)
                {
                    Parent!.ExplodeLeft(LeftValue, RightValue, true);
                }
                else if (IsRight)
                {
                    Parent!.ExplodeRight(LeftValue, RightValue, true);
                }

                return true;
            }            
        }

        private void ExplodeRight(long left, long right, bool initial = false)
        {
            if (IsLeft)
            {
                if (Parent?.Right is not null)
                {
                    Parent.Right.AddLeftValue(right);
                }
                else
                {
                    Parent!.RightValue += right;
                }
            }
            else
            {
                Parent?.ExplodeRight(left, right);
            }
           

            if (initial)
            {
                if (Left is not null)
                {
                    Left.AddRightValue(left);
                }
                else
                {
                    LeftValue += left;
                }

                RightValue = 0;
                Right = null;
            }
        }

        private void ExplodeLeft(long left, long right, bool initial = false)
        {
            if (IsRight)
            {
                if (Parent?.Left is not null)
                {
                    Parent.Left.AddRightValue(left);
                }
                else
                {
                    Parent!.LeftValue += left;
                }
            }
            else
            {
                Parent?.ExplodeLeft(left, right);
            }

            if (initial)
            {
                if (Right is not null)
                {
                    Right.AddLeftValue(right);
                }
                else
                {
                    RightValue += right;
                }

                LeftValue = 0;
                Left = null;
            }
        }

        private bool TrySplit()
        {
            bool split = false;
            if (Left is not null && Left.TrySplit())
            {
                return true;
            }
            else if (LeftValue >= 10)
            {
                split = true;
                Left = new SnailNumber
                {                                        
                    LeftValue = (int)Math.Floor(LeftValue / 2D),
                    RightValue = (int)Math.Ceiling(LeftValue / 2D),                 
                };
                LeftValue = 0;
            }
            else if (Right is not null && Right.TrySplit())
            {
                return true;
            }
            else if (RightValue >= 10)
            {
                split = true;
                Right = new SnailNumber
                {                                       
                    LeftValue = (int)Math.Floor(RightValue / 2D),
                    RightValue = (int)Math.Ceiling(RightValue / 2D),                    
                };
                RightValue = 0;
            }

            return split;
        }

        public override string ToString()
        {
            var left = Left?.ToString() ?? $"{LeftValue}";
            var right = Right?.ToString() ?? $"{RightValue}";
            return $"[{left},{right}]";
        }
    }
}
