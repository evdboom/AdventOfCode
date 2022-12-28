use std::collections::HashSet;
use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");

    let start = Instant::now();
    let part_one = part_one(&input);
    let duration_one = start.elapsed();
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    part_two(&input);
    let duration_two = start.elapsed() - duration_one;
    println!("Part two took: {:?}", duration_two);
}

fn part_one(input: &String) -> i32 {
    let wanted_cycles = HashSet::from([20, 60, 100, 140, 180, 220]);

    let mut x = 1;
    let mut cycle = 0;
    let mut value = 0;

    for line in input.lines() {
        cycle += 1;
        if wanted_cycles.contains(&cycle) {
            value += x * cycle;
        }
        if line != "noop" {
            cycle += 1;
            if wanted_cycles.contains(&cycle) {
                value += x * cycle;
            }
            x += line.replace("addx ", "").parse::<i32>().unwrap();
        }
    }

    value
}

fn part_two(input: &String) {
    let mut x = 1;
    let mut cycle = 0;
    for line in input.lines() {
        write_cycle(&cycle, &x);
        cycle += 1;
        if line != "noop" {
            write_cycle(&cycle, &x);
            cycle += 1;
            x += line.replace("addx ", "").parse::<i32>().unwrap();
        }
    }
    println!();
}

fn write_cycle(cycle: &i32, x: &i32) {
    let cycle_in_line = cycle % 40;
    if cycle_in_line == 0 {
        println!();
    }

    if (cycle_in_line - x).abs() <= 1 {
        print!("#")
    } else {
        print!(" ");
    }
}
