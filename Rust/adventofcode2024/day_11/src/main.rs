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
    let mut cache = HashMap::new();
    input
        .split(" ")
        .filter(|x| x.parse::<usize>().is_ok())
        .map(|x| perform_blinks(x.parse::<usize>().unwrap(), 25, &mut cache))
        .sum()
}

fn process_part_two(input: &str) -> usize {
    2
    // let mut cache = HashMap::new();
    // input
    //     .split(" ")
    //     .filter(|x| x.parse::<usize>().is_ok())
    //     .map(|x| perform_blinks(x.parse::<usize>().unwrap(), 75, &mut cache))
    //     .sum()
}

fn perform_blinks(stone: usize, steps: usize, cache: &mut HashMap<(usize, usize), usize>) -> usize {
    if steps == 0 {
        return stone;
    }
    if let Some(&value) = cache.get(&(stone, steps)) {
        return value;
    }

    let mut result = 0;
    for b in blink(stone) {
        result += perform_blinks(b, steps - 1, cache);
    }
    cache.insert((stone, steps), result);
    result
}

fn blink(stone: usize) -> Vec<usize> {
    let mut result = Vec::new();

    if stone == 0 {
        result.push(1);
        return result;
    }

    let stone_str = stone.to_string();
    let len = stone_str.len();

    if len % 2 == 0 {
        let mid = len / 2;
        let first_half = &stone_str[..mid];
        let second_half = &stone_str[mid..];

        result.push(first_half.parse::<usize>().unwrap());
        result.push(second_half.parse::<usize>().unwrap());
    } else {
        result.push(stone * 2024);
    }

    result
}
