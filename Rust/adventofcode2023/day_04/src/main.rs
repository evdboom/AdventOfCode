use std::collections::{HashMap, HashSet};
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

fn process_part_one(input: &String) -> u32 {
    input
        .lines()
        .map(|line| {
            let count = get_winning_count(line);
            if count == 0 {
                0
            } else {
                let base: u32 = 2;
                1 * base.pow(count - 1)
            }
        })
        .sum()
}

fn process_part_two(input: &String) -> u32 {
    let mut card = 0;
    let mut cards: HashMap<i32, u32> = HashMap::new();
    for line in input.lines() {
        card += 1;
        let mut card_value: u32 = 1;
        if cards.contains_key(&card) {
            card_value = cards[&card] + 1;
        }
        cards.insert(card, card_value);

        let mut count = get_winning_count(line);
        while count > 0 {
            let old = cards.get(&(card + count as i32));
            if old.is_some() {
                cards.insert(card + count as i32, card_value + old.unwrap());
            } else {
                cards.insert(card + count as i32, card_value);
            }
            count -= 1;
        }
    }

    cards.into_iter().map(|card| card.1).sum()
}

fn get_winning_count(input: &str) -> u32 {
    let mut numbers = input.split(": ").last().unwrap().split(" | ");

    let winning = map_to_numbers(numbers.next().unwrap());
    let having = map_to_numbers(numbers.next().unwrap());

    let mut count = 0;
    for winning_number in winning {
        if having.contains(&winning_number) {
            count += 1;
        }
    }

    count
}

fn map_to_numbers(input: &str) -> HashSet<i32> {
    input
        .replace("  ", " ")
        .trim()
        .split(" ")
        .map(|part| part.parse::<i32>().unwrap())
        .collect::<HashSet<i32>>()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11",
        );

        assert_eq!(13, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11",
        );

        assert_eq!(30, process_part_two(&input));
    }
}
