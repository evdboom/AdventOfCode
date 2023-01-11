use regex::Regex;
use std::fs;
use std::time::Instant;

struct Instruction {
    value: isize,
    is_direction: bool
}

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

fn part_one(input: &String) -> isize {
    let grid_rows = get_grid(input);
    let instructions = get_instructions(input.lines().last().unwrap());
    let start = grid_rows[0].iter().position(|v| v.unwrap_or(false)).unwrap() as isize;
    let mut position = (start, 0isize, 0isize);
    for instruction in instructions {
        if instruction.is_direction {
            position = (position.0, position.1, (position.2 + instruction.value) % 4);
        } else {
            position = move_on_grid(position, instruction.value, &grid_rows);
        }
    }
    (position.1 + 1) * 1000 + (position.0 + 1) * 4 + position.2
}

fn part_two(input: &String) -> isize {
    0
}

fn move_on_grid(mut position: (isize, isize, isize), steps: isize, grid_rows: &Vec<Vec<Option<bool>>>) -> (isize, isize, isize) {  
    for _ in 0..steps {
        let new_position = match position.2 {
            0 => (position.0 + 1, position.1, position.2),
            1 => (position.0, position.1 + 1, position.2),
            2 => (position.0 - 1, position.1, position.2),
            3 => (position.0, position.1 - 1, position.2),
            _ => panic!("Unknown facing")
        };
        let temp = vec![];
        let on_grid = if new_position.0 < 0 || new_position.1 < 0 {
            &None
        } else {
            grid_rows.get(new_position.1 as usize).unwrap_or(&temp).get(new_position.0 as usize).unwrap_or(&None)
        };
        if on_grid.is_some() {
            if on_grid.unwrap() {
                position = new_position;
            } else {
                break;
            }
        } else if new_position.0 < 0 {
            let mut target = (grid_rows[0].len() - 1, new_position.1 as usize);
            while grid_rows[target.1][target.0].is_none() {
                target.0 -= 1;
            }
            
            if grid_rows[target.1][target.0].unwrap() {
                position = (target.0 as isize, new_position.1, new_position.2);
            } else {
                break;
            }
        } else if new_position.0 != position.0 {
            let mut target = (0, new_position.1 as usize);
            while grid_rows[target.1][target.0].is_none() {
                target.0 += 1;
            }
            
            if grid_rows[target.1][target.0].unwrap() {
                position = (target.0 as isize, new_position.1, new_position.2);
            } else {
                break;
            }
        } else if new_position.1 < 0 {
            let mut target = (new_position.0 as usize, grid_rows.len() - 1);
            while grid_rows[target.1][target.0].is_none() {
                target.1 -= 1;
            }
            
            if grid_rows[target.1][target.0].unwrap() {
                position = (new_position.0, target.1 as isize, new_position.2);
            } else {
                break;
            }
        } else {
            let mut target = (new_position.0 as usize, 0);
            while grid_rows[target.1][target.0].is_none() {
                target.1 += 1;
            }
            
            if grid_rows[target.1][target.0].unwrap() {
                position = (new_position.0, target.0 as isize, new_position.2);
            } else {
                break;
            }
        }
        
        
    }

    position
}

fn get_grid(input: &String) -> Vec<Vec<Option<bool>>> {
    let mut result = vec![];

    let mut size = 0;
    for line in input.lines() {
        if line.is_empty() {
            break;
        }
        let chars = line.chars().count();
        if chars > size {
            size = chars;
        }
    }

    for line in input.lines() {
        if line.is_empty() {
            break;
        }
        let mut row = vec![None; size];
        let mut i = 0;
        for char in line.chars() {
            match  char {                
                '.' => row[i] = Some(true),
                '#' => row[i] = Some(false),
                _ => ()
            }
            i += 1;
        }
        result.push(row);
    }
    result
}

fn get_instructions(line: &str) -> Vec<Instruction> {    
    Regex::new(r"(?P<s>\d+)")
        .unwrap()
        .replace_all(line, r",$s,")        
        .trim_matches(',')
        .split(",")
        .map(|v| {
            let value = v.parse::<isize>();
            if value.is_ok() {
                Instruction {
                    is_direction: false,
                    value: value.unwrap()
                }
            } else {
                match v {
                    "L" => Instruction { value: 3, is_direction: true },
                    "R" => Instruction { value: 1, is_direction: true },
                    _ => panic!("Unknown value")
                }
            }
        })
        .collect()        
}