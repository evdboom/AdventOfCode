use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");

    let start = Instant::now();
    let part_one = process(4, &input);
    let duration_one = start.elapsed();
    let part_two = process(14, &input);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn process(size: usize, s: &String) -> usize {
    let mut result = 0;
    for i in 0..s.len() {
        let x = &s[i..i + size];
        let mut y: Vec<char> = x.chars().collect();
        y.sort_unstable();
        y.dedup();
        if y.len() == size {
            result = i + size;
            break;
        }
    }
    result
}
