extern crate grid_helper;

use grid_helper::grid::{Direction, Grid, Point};
use std::collections::HashSet;
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
    let grid = Grid::from_string(input, |c| c);
    follow_pipe(&grid).0
}

fn process_part_two(input: &str) -> usize {
    let grid = Grid::from_string(input, |c| c);
    let pipes = follow_pipe(&grid).1;
    get_contained_parts(&grid, &pipes)
}

fn get_contained_parts(grid: &Grid<char>, pipe: &HashSet<Point>) -> usize {
    let mut count = 0;
    for cell in grid.iter() {
        if pipe.contains(&cell.point) {
            continue;
        }

        let mut matches = 0;
        let mut match_start = None;
        for j in 0..=cell.point.x {
            let i = cell.point.x - j;
            if pipe.contains(&Point::new(i, cell.point.y)) {
                let part = grid.get(&Point::new(i, cell.point.y)).unwrap();

                if part == &'|' {
                    matches += 1;
                } else if match_start.is_none() {
                    match_start = Some(part);
                } else if part != &'-' {
                    if part == &'F' && match_start.unwrap() == &'J' {
                        matches += 1;
                    } else if part == &'L' && match_start.unwrap() == &'7' {
                        matches += 1;
                    }
                    match_start = None;
                }
            }
        }

        if matches % 2 != 0 {
            count += 1;
        }
    }
    count
}

fn follow_pipe(grid: &Grid<char>) -> (usize, HashSet<Point>) {
    let start = grid.iter().find(|c| c.value == 'S').unwrap().point;
    let start_pipes = find_start_pipes(&grid, &start);
    let mut position1 = (start_pipes[0].0.point(), start_pipes[0].1);
    let mut position2 = (start_pipes[1].0.point(), start_pipes[1].1);
    let mut from1 = Direction::from_direction(&start_pipes[0].0, start);
    let mut from2 = Direction::from_direction(&start_pipes[1].0, start);

    let mut visited = HashSet::new();
    visited.insert(start);
    visited.insert(position1.0);
    visited.insert(position2.0);

    let mut length = 1;
    loop {
        let new_position = find_next_position(&position1, &from1);
        from1 = Direction::from_direction(&new_position, position1.0);
        position1 = (
            new_position.point(),
            *grid.get(&new_position.point()).unwrap(),
        );
        if !visited.insert(position1.0) {
            break;
        }
        let new_position = find_next_position(&position2, &from2);
        from2 = Direction::from_direction(&new_position, position2.0);
        position2 = (
            new_position.point(),
            *grid.get(&new_position.point()).unwrap(),
        );

        length += 1;
        if !visited.insert(position2.0) {
            break;
        }
    }

    (length, visited)
}

fn find_start_pipes(grid: &Grid<char>, start: &Point) -> Vec<(Direction, char)> {
    let initial = grid.get_adjacent_with_direction(start);

    let mut result = Vec::new();
    for direction in initial {
        match direction {
            Direction::Up(up) => {
                let value = grid.get(&up).unwrap();
                if value == &'|' || value == &'7' || value == &'F' {
                    result.push((direction, *value));
                }
            }
            Direction::Down(down) => {
                let value = grid.get(&down).unwrap();
                if value == &'|' || value == &'L' || value == &'J' {
                    result.push((direction, *value));
                }
            }
            Direction::Right(right) => {
                let value = grid.get(&right).unwrap();
                if value == &'-' || value == &'7' || value == &'J' {
                    result.push((direction, *value));
                }
            }
            Direction::Left(left) => {
                let value = grid.get(&left).unwrap();
                if value == &'-' || value == &'F' || value == &'L' {
                    result.push((direction, *value));
                }
            }
        }
    }
    result
}

fn find_next_position(position: &(Point, char), from: &Direction) -> Direction {
    match position.1 {
        '|' => match from {
            Direction::Up(_) => Direction::up(position.0.up().unwrap()),
            _ => Direction::down(position.0.down().unwrap()),
        },
        '-' => match from {
            Direction::Left(_) => Direction::left(position.0.left().unwrap()),
            _ => Direction::right(position.0.right().unwrap()),
        },
        '7' => match from {
            Direction::Up(_) => Direction::left(position.0.left().unwrap()),
            _ => Direction::down(position.0.down().unwrap()),
        },
        'F' => match from {
            Direction::Up(_) => Direction::right(position.0.right().unwrap()),
            _ => Direction::down(position.0.down().unwrap()),
        },
        'L' => match from {
            Direction::Down(_) => Direction::right(position.0.right().unwrap()),
            _ => Direction::up(position.0.up().unwrap()),
        },
        'J' => match from {
            Direction::Down(_) => Direction::left(position.0.left().unwrap()),
            _ => Direction::up(position.0.up().unwrap()),
        },
        _ => panic!("Invalid pipe"),
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "7-F7-
.FJ|7
SJLL7
|F--J
LJ.LJ",
        );

        assert_eq!(8, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "FF7FSF7F7F7F7F7F---7
L|LJ||||||||||||F--J
FL-7LJLJ||||||LJL-77
F--JF--7||LJLJ7F7FJ-
L---JF-JLJ.||-FJLJJ7
|F|F-JF---7F7-L7L|7|
|FFJF7L7F-JF7|JL---7
7-L-JL7||F7|L7F-7F7|
L.L7LFJ|||||FJL7||LJ
L7JLJL-JLJLJL--JLJ.L",
        );

        assert_eq!(10, process_part_two(&input));
    }
}
