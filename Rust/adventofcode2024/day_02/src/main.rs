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
        .filter(|line| {
            let levels: Vec<u32> = line
                .split(" ")
                .map(|part| part.parse::<u32>().unwrap())
                .collect();

            is_valid_sequence(&levels)
        })
        .count() as u32
}

fn process_part_two(input: &String) -> u32 {
    input
        .lines()
        .filter(|line| {
            let levels: Vec<u32> = line
                .split(" ")
                .map(|part| part.parse::<u32>().unwrap())
                .collect();

            if is_valid_sequence(&levels) {
                return true;
            }

            for i in 0..levels.len() {
                let mut modified_levels = levels.clone();
                modified_levels.remove(i);
                if is_valid_sequence(&modified_levels) {
                    return true;
                }
            }

            false
        })
        .count() as u32
}

fn is_valid_sequence(line: &Vec<u32>) -> bool {
    let mut descending: Option<bool> = None;
    let mut current = line[0];

    for &next in line.iter().skip(1) {
        if descending.is_none() {
            descending = Some(current > next);
        }

        if descending != Some(current > next) {
            return false;
        }

        let diff = current.abs_diff(next);
        if diff < 1 || diff > 3 {
            return false;
        }

        current = next;
    }

    true
}
