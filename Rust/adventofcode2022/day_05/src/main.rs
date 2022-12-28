use std::collections::HashMap;
use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");

    let start = Instant::now();
    let part_one = part_one(&input);
    let duration_one = start.elapsed();
    let part_two = part_two(&input);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn part_one(input: &str) -> String {
    let mut stacks = get_stacks(input);
    let procedures = get_procesdures(input, &stacks.1);

    for procedure in procedures {
        let mut i = 0;
        while i < procedure[0] {
            stacks.0 = move_stack(&procedure[1], &procedure[2], &1, stacks.0); 
            i += 1;
        }
    }

    get_top(stacks.0)
}

fn part_two(input: &str) -> String {
    let mut stacks = get_stacks(input);
    let procedures = get_procesdures(input, &stacks.1);

    for procedure in procedures {
        stacks.0 = move_stack(&procedure[1], &procedure[2], &procedure[0], stacks.0); 
    }

    get_top(stacks.0)
}

fn move_stack(from: &i32, to: &i32, stack_size: &i32, mut stacks: HashMap<i32, Vec<char>>) -> HashMap<i32, Vec<char>> {
    let mut i = 0;
    let mut crane = Vec::new();
    while &i < stack_size {
        crane.push(stacks.get_mut(from).unwrap().pop().unwrap());
        i += 1;
    }

    while let Some(supply_crate) = crane.pop() {
        stacks.get_mut(to).unwrap().push(supply_crate);
    }

    stacks
}

fn get_top (stacks: HashMap<i32, Vec<char>>) -> String {
    let mut top = Vec::new();    
    let mut keys: Vec<&i32> = stacks.keys().collect();
    keys.sort();
    for key in keys {
        top.push(stacks[key].last().unwrap());
    }
    
    String::from_iter(top)
}

fn get_stacks(input: &str) -> (HashMap<i32, Vec<char>>, usize) {
    let mut raw = Vec::new();
    let mut procedure_start = 0;
    for line in input.lines() {
        procedure_start += 1;
        if line.is_empty() {
            break;
        } else {
            raw.push(line);
        }
    }
    let result = process_stacks(raw);

    (result, procedure_start)
}

fn process_stacks(mut raw: Vec<&str>) -> HashMap<i32, Vec<char>> {
    let stack_line = raw.pop().unwrap();
    let stacks: Vec<(i32, usize)> = stack_line
        .split(" ")
        .filter(|s| !s.is_empty())
        .map(|stack| {
            (
                stack.parse::<i32>().unwrap(),
                stack_line.find(stack).unwrap(),
            )
        })
        .collect();

    let mut result = HashMap::new();
    for stack in &stacks {
        result.insert(stack.0, Vec::new());
    }

    while let Some(row) = raw.pop() {        
        let crates: Vec<char> = row.chars().collect();
        for stack in &stacks {
            let supply_crate = crates[stack.1];
            if supply_crate != ' ' {
                result.get_mut(&stack.0).unwrap().push(supply_crate);
            }
        }
    }

    result
}

fn get_procesdures(input: &str, procedure_start: &usize) -> Vec<Vec<i32>> {
    let mut result = Vec::new();
    for line in input
        .lines()
        .enumerate()
        .filter(|&(index, _)| &index >= procedure_start)
    {
        let parts: Vec<i32> = line
            .1
            .replace("move ", "")
            .replace(" from ", " ")
            .replace(" to ", " ")
            .split_whitespace()
            .map(|s| s.parse::<i32>().unwrap())
            .collect();
        result.push(parts);
    }

    result
}
