use std::collections::{HashMap, HashSet};
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
    let (blocks_by_column, blocks_by_row, start, (width, height)) = get_blocks(input);
    let mut visited = HashSet::new();
    visited.insert(start);

    let mut direction = '^';
    let mut position = start.clone();
    while let Some(block) = get_next_block(&blocks_by_column, &blocks_by_row, &direction, &position)
    {
        let distance = position.0.abs_diff(block.0) + position.1.abs_diff(block.1);
        for _ in 1..distance {
            position = match direction {
                '^' => (position.0, position.1 - 1),
                '>' => (position.0 + 1, position.1),
                'v' => (position.0, position.1 + 1),
                '<' => (position.0 - 1, position.1),
                _ => panic!("Invalid direction"),
            };
            visited.insert(position);
        }
        direction = turn_right(direction);
    }
    let distance_to_edge = match direction {
        '^' => position.1,
        '>' => width - position.0 - 1,
        'v' => height - position.1 - 1,
        '<' => position.0,
        _ => panic!("Invalid direction"),
    };
    for _ in 0..distance_to_edge {
        position = match direction {
            '^' => (position.0, position.1 - 1),
            '>' => (position.0 + 1, position.1),
            'v' => (position.0, position.1 + 1),
            '<' => (position.0 - 1, position.1),
            _ => panic!("Invalid direction"),
        };
        visited.insert(position);
    }
    visited.len() as u32
}

fn process_part_two(input: &str) -> u32 {
    2
}

fn get_next_block(
    blocks_by_column: &HashMap<usize, Vec<usize>>,
    blocks_by_row: &HashMap<usize, Vec<usize>>,
    direction: &char,
    position: &(usize, usize),
) -> Option<(usize, usize)> {
    match direction {
        '^' => {
            if let Some(column) = blocks_by_column.get(&position.0) {
                column
                    .iter()
                    .rev()
                    .map(|&row| (position.0, row))
                    .find(|row| row.1 < position.1)
            } else {
                None
            }
        }
        'v' => {
            if let Some(column) = blocks_by_column.get(&position.0) {
                column
                    .iter()
                    .map(|&row| (position.0, row))
                    .find(|row| row.1 > position.1)
            } else {
                None
            }
        }
        '>' => {
            if let Some(row) = blocks_by_row.get(&position.1) {
                row.iter()
                    .map(|&column| (column, position.1))
                    .find(|column| column.0 > position.0)
            } else {
                None
            }
        }
        '<' => {
            if let Some(row) = blocks_by_row.get(&position.1) {
                row.iter()
                    .rev()
                    .map(|&column| (column, position.1))
                    .find(|column| column.0 < position.0)
            } else {
                None
            }
        }
        _ => None,
    }
}

fn get_blocks(
    input: &str,
) -> (
    HashMap<usize, Vec<usize>>,
    HashMap<usize, Vec<usize>>,
    (usize, usize),
    (usize, usize),
) {
    let mut result_by_column = HashMap::new();
    let mut result_by_row = HashMap::new();
    let mut start: (usize, usize) = (0, 0);
    let mut grid_size: (usize, usize) = (0, 0);
    input.lines().enumerate().for_each(|(row, line)| {
        grid_size.1 = row + 1;
        grid_size.0 = line.len();
        line.chars().enumerate().for_each(|(column, char)| {
            if char == '#' {
                let column_entry = result_by_column.entry(column).or_insert_with(Vec::new);
                column_entry.push(row);
                column_entry.sort();
                let row_entry = result_by_row.entry(row).or_insert_with(Vec::new);
                row_entry.push(column);
                row_entry.sort();
            } else if char == '^' {
                start.0 = column;
                start.1 = row;
            }
        });
    });

    (result_by_column, result_by_row, start, grid_size)
}

fn turn_right(direction: char) -> char {
    match direction {
        '^' => '>',
        '>' => 'v',
        'v' => '<',
        '<' => '^',
        _ => panic!("Invalid direction"),
    }
}
