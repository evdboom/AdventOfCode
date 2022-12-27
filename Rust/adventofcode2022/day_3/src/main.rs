use itertools::Itertools;
use std::fs;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    
    let part_one = part_one(&input);
    println!("{part_one}");
    let part_two = part_two(&input);
    println!("{part_two}");
}

fn part_one(input: &String) -> i32 {
    input.lines().fold(0i32, |sum, line| {
        let bytes: Vec<u8> = line.bytes().collect();
        let mut i = 0;
        let half = bytes.len() / 2;
        let mut compartment1 = Vec::new();
        let mut value = 0;
        for byte in bytes {
            let v = char_value(byte);
            if i < half {
                compartment1.push(v);
            } else if compartment1.contains(&v) {
                value = v;
                break;
            }
            i = i + 1;
        }

        sum + value
    })
}

fn part_two(input: &String) -> i32 {
    let mut sum = 0;
    let mut index = 0;
    let mut elves = Vec::new();
    for line in input.lines() {
        elves.push((index / 3, line));
        index = index + 1;
    }
    for mut group in &elves.iter().group_by(|elf| elf.0) {
        let elf_one: Vec<i32> = group.1.next().unwrap().1.bytes().map(|b| char_value(b)).collect();
        let elf_two: Vec<i32> = group.1.next().unwrap().1.bytes().map(|b| char_value(b)).collect();
        let elf_three: Vec<i32> = group.1.next().unwrap().1.bytes().map(|b| char_value(b)).collect();        
        for item in elf_one {
            if elf_two.contains(&item) && elf_three.contains(&item) {
                sum = sum + item;
                break;
            }
        }
    }

    sum
}

fn char_value(byte: u8) -> i32 {
    let moved: i32 = (byte % 64).into();
    if moved > 32 {
        moved - 32 // lowercase, move back 32 slots
    } else {
        moved + 26 // uppercase, add 26 for value
    }
}
