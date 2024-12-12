mod hand;

use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = process_part_one(&input);
    let duration_one = start.elapsed();
    let part_two = process_part_two(&input);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn process_part_one(input: &str) -> usize {
    let mut hands = input
        .lines()
        .map(|line| {
            let mut parts = line.split_whitespace();
            hand::Hand::new(
                parts.next().unwrap().chars().collect(),
                parts.next().unwrap().parse().unwrap(),
                false,
            )
        })
        .collect::<Vec<_>>();
    hands.sort_by(|a, b| a.cmp(b));

    hands
        .iter()
        .enumerate()
        .map(|(index, hand)| hand.bid * (index + 1))
        .sum()
}

fn process_part_two(input: &str) -> usize {
    let mut hands = input
        .lines()
        .map(|line| {
            let mut parts = line.split_whitespace();
            hand::Hand::new(
                parts.next().unwrap().chars().collect(),
                parts.next().unwrap().parse().unwrap(),
                true,
            )
        })
        .collect::<Vec<_>>();
    hands.sort_by(|a, b| a.cmp(b));

    hands
        .iter()
        .enumerate()
        .map(|(index, hand)| hand.bid * (index + 1))
        .sum()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483",
        );

        assert_eq!(6440, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483",
        );

        assert_eq!(5905, process_part_two(&input));
    }
}
