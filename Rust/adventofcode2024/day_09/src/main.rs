use std::collections::{HashMap, HashSet, VecDeque};
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
    let file_system: Vec<usize> = input
        .chars()
        .filter(|c| c.is_digit(10))
        .map(|c| c.to_digit(10).unwrap() as usize)
        .collect();

    let mut unpacked = VecDeque::new();
    for i in 0..file_system.len() {
        let is_file = i % 2 == 0;
        for _ in 0..file_system[i] {
            let value = if is_file { Some(i / 2) } else { None };
            unpacked.push_back(value);
        }
    }

    let mut result = 0;
    for i in 0..unpacked.len() {
        if let Some(item) = unpacked.pop_front() {
            if let Some(value) = item {
                result += i * value;
            } else {
                let mut last = None;
                while last == None {
                    last = unpacked.pop_back().unwrap();
                }
                result += i * last.unwrap();
            }
        }
    }

    result
}

fn process_part_two(input: &str) -> usize {
    let file_system: Vec<usize> = input
        .chars()
        .filter(|c| c.is_digit(10))
        .map(|c| c.to_digit(10).unwrap() as usize)
        .collect();

    let mut moves = HashMap::new();
    let mut moved = HashSet::new();
    let mut index = file_system.len() - 1;
    if index % 2 != 0 {
        index -= 1;
    }
    while index > 0 {
        let size = file_system[index];
        let mut check_index = 1;
        while check_index < index {
            let free_size =
                file_system[check_index] - moves.get(&check_index).unwrap_or(&(0, None)).0;
            if free_size >= size {
                let entry = moves.entry(check_index).or_insert((0, None));
                entry.0 += size;
                if entry.1.is_none() {
                    entry.1 = Some(VecDeque::new());
                }
                for _ in 0..size {
                    entry.1.as_mut().unwrap().push_back(index / 2);
                }
                moved.insert(index);
                break;
            }
            check_index += 2;
        }

        index -= 2;
    }

    let mut unpacked = VecDeque::new();
    for i in 0..file_system.len() {
        let is_file = i % 2 == 0;
        for _ in 0..file_system[i] {
            let value;
            if is_file {
                if moved.contains(&i) {
                    value = None;
                } else {
                    value = Some(i / 2);
                }
            } else {
                if let Some(m) = moves.get_mut(&i) {
                    if let Some(y) = &m.1 {
                        let mut k = y.to_owned();
                        value = k.pop_front();
                        moves.entry(i).and_modify(|e| e.1 = Some(k));
                    } else {
                        value = None;
                    }
                } else {
                    value = None;
                }
            }
            unpacked.push_back(value);
        }
    }

    let mut result = 0;
    for i in 0..unpacked.len() {
        if let Some(item) = unpacked.pop_front() {
            if let Some(value) = item {
                result += i * value;
            }
        }
    }
    result
}
