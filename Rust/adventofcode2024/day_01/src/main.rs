use std::collections::{BinaryHeap, HashMap};
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
    let mut one: BinaryHeap<u32> = BinaryHeap::new();
    let mut two: BinaryHeap<u32> = BinaryHeap::new();

    input.lines().into_iter().for_each(|line| {
        let mut parts = line.split("   ");
        let first: u32 = parts.next().unwrap().parse().unwrap();
        one.push(first);
        let second: u32 = parts.next().unwrap().parse().unwrap();
        two.push(second);
    });
    let mut sum: u32 = 0;
    while !one.is_empty() {
        let first = one.pop().unwrap();
        let second = two.pop().unwrap();
        if first > second {
            sum += first - second;
        } else {
            sum += second - first;
        }
    }

    sum
}

fn process_part_two(input: &String) -> u32 {
    let mut one: HashMap<u32, u32> = HashMap::new();
    let mut two: HashMap<u32, u32> = HashMap::new();

    input.lines().into_iter().for_each(|line| {
        let mut parts = line.split("   ");
        let first: u32 = parts.next().unwrap().parse().unwrap();
        let count_one = one.entry(first).or_insert(0);
        *count_one += 1;
        let second: u32 = parts.next().unwrap().parse().unwrap();
        let count_two = two.entry(second).or_insert(0);
        *count_two += 1;
    });

    let mut sum: u32 = 0;

    for (id, count_one) in one.iter() {
        let count_two = two.get(id).unwrap_or(&0);
        sum += id * count_one * count_two
    }

    sum
}
