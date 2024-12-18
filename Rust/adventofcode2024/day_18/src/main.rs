extern crate grid_helper;
mod position;

use grid_helper::grid::{Grid, Point};
use position::Position;
use std::collections::{BinaryHeap, HashMap, HashSet};
use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = process_part_one(&input, 1024, 71);
    let duration_one = start.elapsed();
    let part_two = process_part_two(&input, 1024, 71);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn process_part_one(input: &str, bytes_to_read: usize, grid_size: usize) -> usize {
    let memory = get_memory_space(input, bytes_to_read, grid_size);

    let end = Point::new(grid_size - 1, grid_size - 1);
    let start = Position::new(Point::new(0, 0), 0, &HashSet::new());
    get_path(start, end, &memory).unwrap().steps
}

fn process_part_two(input: &str, bytes_to_read: usize, grid_size: usize) -> String {
    let mut memory = get_memory_space(input, bytes_to_read, grid_size);
    let end = Point::new(grid_size - 1, grid_size - 1);
    let mut visited = HashSet::new();
    for byte_to_read in bytes_to_read + 1.. {
        if let Some(byte_to_add) = get_byte_position(input, byte_to_read) {
            memory.set(byte_to_add, true);

            if visited.is_empty() || visited.contains(&byte_to_add) {
                let start = Position::new(Point::new(0, 0), 0, &HashSet::new());
                if let Some(position) = get_path(start, end, &memory) {
                    visited = position.visited;
                } else {
                    return format!("{},{}", byte_to_add.x, byte_to_add.y);
                }
            }
        }
    }
    String::from("Could not find it")
}

fn get_byte_position(input: &str, byte_to_read: usize) -> Option<Point> {
    if let Some(line) = input.lines().nth(byte_to_read) {
        if line.is_empty() {
            return None;
        }
        let mut parts = line.split(',');
        let x = parts.next().unwrap().parse::<usize>().unwrap();
        let y = parts.next().unwrap().parse::<usize>().unwrap();

        Some(Point::new(x, y))
    } else {
        None
    }
}

fn get_path(start: Position, end: Point, memory: &Grid<bool>) -> Option<Position> {
    let mut queue = BinaryHeap::new();
    let mut cache = HashMap::new();
    queue.push(start);
    while let Some(position) = queue.pop() {
        if position.point == end {
            return Some(position);
        }
        let known = cache.entry(position.point).or_insert(usize::MAX);
        if position.steps >= *known {
            continue;
        } else {
            *known = position.steps;
        }

        for adjacent in memory.get_filtered_adjacent(&position.point, |c| !c).iter() {
            if let Some(new_position) =
                Position::try_new(*adjacent, position.steps + 1, &position.visited)
            {
                queue.push(new_position);
            }
        }
    }

    None
}

fn get_memory_space(input: &str, bytes_to_read: usize, grid_size: usize) -> Grid<bool> {
    let mut grid = Grid::new(grid_size, grid_size, false);
    let max = if bytes_to_read == 0 {
        usize::MAX
    } else {
        bytes_to_read
    };

    let mut lines = input.lines();

    for _ in 0..max {
        if let Some(line) = lines.next() {
            let mut parts = line.split(',');
            let x = parts.next().unwrap().parse::<usize>().unwrap();
            let y = parts.next().unwrap().parse::<usize>().unwrap();
            grid.set(Point::new(x, y), true);
        }
    }

    grid
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "5,4
4,2
4,5
3,0
2,1
6,3
2,4
1,5
0,6
3,3
2,6
5,1
1,2
5,5
2,5
6,5
1,4
0,4
6,4
1,1
6,1
1,0
0,5
1,6
2,0",
        );

        assert_eq!(22, process_part_one(&input, 12, 7));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "5,4
4,2
4,5
3,0
2,1
6,3
2,4
1,5
0,6
3,3
2,6
5,1
1,2
5,5
2,5
6,5
1,4
0,4
6,4
1,1
6,1
1,0
0,5
1,6
2,0",
        );

        assert_eq!("6,1", process_part_two(&input, 12, 7));
    }
}
