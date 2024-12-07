use std::collections::VecDeque;
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
    input.lines().fold(0, |acc, line| {
        let (result, parts) = parse_line(line);
        if process_part(result, parts, false) {
            acc + result
        } else {
            acc
        }
    })
}

fn process_part_two(input: &str) -> usize {
    input.lines().fold(0, |acc, line| {
        let (result, parts) = parse_line(line);
        if process_part(result, parts, true) {
            acc + result
        } else {
            acc
        }
    })
}

fn parse_line(line: &str) -> (usize, Vec<usize>) {
    let mut all = line.split(": ");
    let result: usize = all.next().unwrap().parse().unwrap();
    let parts: Vec<usize> = all
        .next()
        .unwrap()
        .split(" ")
        .map(|part| part.parse::<usize>().unwrap())
        .collect();

    (result, parts)
}

fn process_part(result: usize, parts: Vec<usize>, include_concat: bool) -> bool {
    let mut queue = VecDeque::new();
    queue.push_back(0 as usize);
    for part in parts {
        let mut new_values = Vec::new();
        while let Some(current) = queue.pop_front() {
            let sum = current + part;
            if sum <= result {
                new_values.push(sum);
            }

            let mul = current * part;
            if mul <= result {
                new_values.push(mul);
            }

            if include_concat {
                let concat = format!("{}{}", current, part).parse::<usize>().unwrap();
                if concat <= result {
                    new_values.push(concat);
                }
            }
        }
        for value in new_values {
            queue.push_back(value);
        }
    }

    let mut any_valid = false;
    while let Some(value) = queue.pop_front() {
        if result == value {
            any_valid = true;
            break;
        }
    }

    any_valid
}
