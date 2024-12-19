use std::collections::HashMap;
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
    let (towels, patterns) = get_towels_and_patterns(input);

    let mut count = 0;
    let mut cache = HashMap::new();
    for pattern in patterns {
        count += can_make_pattern(pattern, &towels, &mut cache, false);
    }

    count
}

fn process_part_two(input: &str) -> usize {
    let (towels, patterns) = get_towels_and_patterns(input);

    let mut count = 0;
    let mut cache = HashMap::new();
    for pattern in patterns {
        count += can_make_pattern(pattern, &towels, &mut cache, true);
    }

    count
}

fn can_make_pattern(
    pattern: &str,
    towels: &Vec<&str>,
    cache: &mut HashMap<String, usize>,
    get_all: bool,
) -> usize {
    if pattern.is_empty() {
        return 1;
    }

    if let Some(&result) = cache.get(pattern) {
        return result;
    }

    let mut count = 0;
    let towel_check = towels.clone();
    for towel in towel_check {
        if pattern.starts_with(towel) {
            let add = can_make_pattern(&pattern[towel.len()..], towels, cache, get_all);
            count += add;
            if !get_all && add > 0 {
                break;
            }
        }
    }
    cache.insert(String::from(pattern), count);
    count
}

fn get_towels_and_patterns(input: &str) -> (Vec<&str>, Vec<&str>) {
    let mut lines = input.lines();
    let towels = lines.next().unwrap().split(", ").collect();
    let mut patterns = Vec::new();
    lines.next();

    while let Some(line) = lines.next() {
        patterns.push(line);
    }

    (towels, patterns)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "r, wr, b, g, bwu, rb, gb, br

brwrr
bggr
gbbr
rrbgbr
ubwu
bwurrg
brgr
bbrgwb",
        );

        assert_eq!(6, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "r, wr, b, g, bwu, rb, gb, br

brwrr
bggr
gbbr
rrbgbr
ubwu
bwurrg
brgr
bbrgwb",
        );

        assert_eq!(16, process_part_two(&input));
    }
}
