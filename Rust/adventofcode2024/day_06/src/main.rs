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
    let (blocks, start, (width, height)) = get_blocks(input);
    let mut visited = HashSet::new();
    visited.insert(start);

    let mut direction = '^';
    let mut position = start.clone();
    while let Some(block) = get_next_block(&blocks, &direction, &position) {
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
    let (mut blocks, start, (width, height)) = get_blocks(input);
    let mut visited = HashMap::new();

    let mut direction = '^';
    let mut position = start.clone();
    while let Some(block) = get_next_block(&blocks, &direction, &position) {
        let distance = position.0.abs_diff(block.0) + position.1.abs_diff(block.1);
        for _ in 1..distance {
            position = match direction {
                '^' => (position.0, position.1 - 1),
                '>' => (position.0 + 1, position.1),
                'v' => (position.0, position.1 + 1),
                '<' => (position.0 - 1, position.1),
                _ => panic!("Invalid direction"),
            };
            visited.entry(position).or_insert(direction);
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
        visited.entry(position).or_insert(direction);
    }
    visited.remove(&start);

    let mut valid_blocks = HashSet::new();
    for option in visited {
        blocks.insert(option.0);
        position = get_step_back(&(option.0 .0, option.0 .1, option.1));
        direction = turn_right(option.1);
        let mut visited_with_direction = HashSet::new();
        while let Some(block) = get_next_block(&blocks, &direction, &position) {
            position = get_step_back(&(block.0, block.1, direction));
            if !visited_with_direction.insert((position.0, position.1, direction)) {
                valid_blocks.insert(option.0);
                break;
            }
            direction = turn_right(direction);
        }

        blocks.remove(&(option.0));
    }

    valid_blocks.len() as u32
}

fn get_step_back(option: &(usize, usize, char)) -> (usize, usize) {
    match option.2 {
        '^' => (option.0, option.1 + 1),
        '>' => (option.0 - 1, option.1),
        'v' => (option.0, option.1 - 1),
        '<' => (option.0 + 1, option.1),
        _ => panic!("Invalid direction"),
    }
}

fn get_next_block(
    blocks: &HashSet<(usize, usize)>,
    direction: &char,
    position: &(usize, usize),
) -> Option<(usize, usize)> {
    match direction {
        '^' => blocks
            .iter()
            .filter(|block| block.0 == position.0 && block.1 < position.1)
            .max_by(|a, b| a.1.cmp(&b.1))
            .copied(),
        'v' => blocks
            .iter()
            .filter(|block| block.0 == position.0 && block.1 > position.1)
            .min_by(|a, b| a.1.cmp(&b.1))
            .copied(),
        '>' => blocks
            .iter()
            .filter(|block| block.1 == position.1 && block.0 > position.0)
            .min_by(|a, b| a.0.cmp(&b.0))
            .copied(),
        '<' => blocks
            .iter()
            .filter(|block| block.1 == position.1 && block.0 < position.0)
            .max_by(|a, b| a.0.cmp(&b.0))
            .copied(),
        _ => None,
    }
}

fn get_blocks(input: &str) -> (HashSet<(usize, usize)>, (usize, usize), (usize, usize)) {
    let mut result = HashSet::new();
    let mut start: (usize, usize) = (0, 0);
    let mut grid_size: (usize, usize) = (0, 0);
    input.lines().enumerate().for_each(|(row, line)| {
        if grid_size.0 == 0 {
            grid_size.0 = line.len();
        }
        grid_size.1 = row + 1;
        line.chars().enumerate().for_each(|(column, char)| {
            if char == '#' {
                result.insert((column, row));
            } else if char == '^' {
                start.0 = column;
                start.1 = row;
            }
        });
    });

    (result, start, grid_size)
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
