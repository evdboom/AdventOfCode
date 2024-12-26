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

fn process_part_one(input: &str) -> usize {
    let mut result = 0;
    for line in input.lines() {
        let mut number = line.parse().unwrap();
        for _ in 0..2000 {
            number = get_secret_number(number);
        }
        result += number;
    }
    result
}

fn process_part_two(input: &str) -> usize {
    let mut patterns = HashMap::new();
    for line in input.lines() {
        let mut line_patterns = HashSet::new();
        let mut number = line.parse().unwrap();
        let mut pattern = vec![];
        for i in 0..2000 {
            let last: i8 = (number % 10) as i8;
            number = get_secret_number(number);
            let next: i8 = (number % 10) as i8;
            pattern.push(last - next);

            if i > 2
                && line_patterns.insert((
                    pattern[i - 3],
                    pattern[i - 2],
                    pattern[i - 1],
                    pattern[i],
                ))
            {
                let entry = patterns
                    .entry((pattern[i - 3], pattern[i - 2], pattern[i - 1], pattern[i]))
                    .or_insert(0);
                *entry += next as usize;
            }
        }
    }

    patterns.values().max().unwrap().clone()
}

fn get_secret_number(number: usize) -> usize {
    let step_one = ((number * 64) ^ number) % 16777216;
    let step_two = ((step_one / 32) ^ step_one) % 16777216;
    let step_three = ((step_two * 2048) ^ step_two) % 16777216;
    step_three
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "1
10
100
2024",
        );

        assert_eq!(37327623, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "1
2
3
2024",
        );

        assert_eq!(23, process_part_two(&input));
    }
}
