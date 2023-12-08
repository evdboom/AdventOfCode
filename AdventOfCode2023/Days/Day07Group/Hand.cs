namespace AdventOfCode2023.Days.Day07Group
{
    public class Hand : IComparable<Hand>
    {
        private readonly List<char> _cardStrength =
            [
                '2',
                '3',
                '4',
                '5',
                '6',
                '7',
                '8',
                '9',
                'T',
                'J',
                'Q',
                'K',
                'A',
            ];

        public long Bid { get; set; }
        public string Cards { get; set; } = string.Empty;
        public bool JokerGame { get; set; }
        public HandType HandType => GetTypeValue();

        public int CompareTo(Hand? other)
        {
            if (other == null)
            {
                return 1;
            }

            if (other.HandType < HandType)
            {
                return -1;
            }
            else if (other.HandType > HandType)
            {
                return 1;
            }
            else
            {
                foreach (var (card, otherCard) in Cards.Zip(other.Cards))
                {
                    var strength = GetCardStrength(card);
                    var otherStrength = GetCardStrength(otherCard);
                    if (otherStrength < strength) 
                    {
                        return -1;
                    }
                    else if (otherStrength > strength)
                    {
                        return 1;
                    }
                }
            }

            return 0;
        }

        private HandType GetTypeValue()
        {
            var distinct = Cards
                .GroupBy(card => card)
                .Select(group => new { Card = group.Key, Count = group.Count() })
                .OrderByDescending(card => card.Count)
                .ToList();

            if (JokerGame && distinct.Count > 1 && distinct.FirstOrDefault(card => card.Card == 'J') is var joker && joker is not null)
            {
                distinct.Remove(joker);
                distinct[0] = new { distinct[0].Card, Count = distinct[0].Count + joker.Count };
            }

            return distinct.Count switch
            {
                1 => HandType.FiveOfAKind,
                2 => distinct.Any(card => card.Count == 4)
                    ? HandType.FourOfAKind
                    : HandType.FullHouse,
                3 => distinct.Any(card => card.Count == 3)
                    ? HandType.ThreeOfAKind
                    : HandType.TwoPair,
                4 => HandType.Pair,
                _ => HandType.HighCard
            };
        }

        private int GetCardStrength(char card)
        {
            return JokerGame && card == 'J'
                ? -1
                : _cardStrength.IndexOf(card);
        }

    }
}
