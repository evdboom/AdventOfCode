namespace AdventOfCode2021.Constructs.Day22
{
    public class Instruction
    {
        public bool On { get; set; }
        public long Xfrom { get; set; }
        public long Yfrom { get; set; }
        public long Zfrom { get; set; }
        public long Xto { get; set; }
        public long Yto { get; set; }
        public long Zto { get; set; }

        public long Volume => Valid ? (Xto - Xfrom + 1) * (Yto - Yfrom + 1) * (Zto - Zfrom + 1) : 0;

        public bool Valid =>
            Xfrom <= Xto &&
            Yfrom <= Yto &&
            Zfrom <= Zto;

        public Instruction Intersect(Instruction intersect)
        {
            return new Instruction
            {
                On = intersect.On,
                Xfrom = Math.Max(Xfrom, intersect.Xfrom),
                Xto = Math.Min(Xto, intersect.Xto),
                Yfrom = Math.Max(Yfrom, intersect.Yfrom),
                Yto = Math.Min(Yto, intersect.Yto),
                Zfrom = Math.Max(Zfrom, intersect.Zfrom),
                Zto = Math.Min(Zto, intersect.Zto),
            };
        }


        public override string ToString()
        {
            var on = On ? "On" : "Off";
            return Valid
                ? $"{on} x:{Xfrom}..{Xto} y:{Yfrom}..{Yto} z:{Zfrom}..{Zto} ({Volume})"
                : $"Invalid"; 
        }
    }
}
