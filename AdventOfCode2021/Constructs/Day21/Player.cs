namespace AdventOfCode2021.Constructs.Day21
{
    public class Player : Dictionary<int, Dictionary<int, long>>
    {
        new public Dictionary<int, long> this[int key]
        {
            get
            {
                if (!ContainsKey(key))
                {
                    Add(key, new Dictionary<int, long>());
                }
                return base[key];
            }
        }

        public long GetWeight()
        {
            return this.Sum(p => p.Value.Values.Sum());
        }
    }
}
