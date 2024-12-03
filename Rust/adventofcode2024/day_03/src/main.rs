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

fn process_part_one(input: &str) -> u32 {
    process_line(input)
}

fn process_part_two(input: &str) -> u32 {
    let line = clean_line(input);
    process_line(&line)
}

fn clean_line(input: &str) -> String {
    let mut result = String::new();

    let primary = input.split("do()");
    for part in primary {
        let mut stopper = part.split("don't()");
        let first = stopper.next();
        if let Some(x) = first {
            result.push_str(x);
        }
    }
    result
}

fn process_line(input: &str) -> u32 {
    let parts = input.split("mul(");
    let mut result: u32 = 0;
    for part in parts {
        let mut inner = part.split(',');
        let first = inner.next();
        let mut remainder: String = String::new();
        inner.for_each(|x| remainder.push_str(x));
        let mut outer = remainder.split(')');
        let second = outer.next();

        if first.is_some() && second.is_some() {
            let first = first.unwrap().parse::<u32>().unwrap_or(0);
            let second = second.unwrap().parse::<u32>().unwrap_or(0);
            if first <= 999 && second <= 999 {
                result += first * second;
            }
        }
    }
    result
}
