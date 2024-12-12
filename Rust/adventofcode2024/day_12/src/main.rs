extern crate grid_helper;

use grid_helper::grid::{Grid, Point};
use std::collections::{HashSet, VecDeque};
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
    let regions = get_regions(&grid);

    regions.iter().fold(0, |acc, region| {
        acc + (region.1.len() * get_perimeter(&region.1))
    })
}

fn process_part_two(input: &str) -> usize {
    let grid = Grid::from_string(input, |c| c);
    let regions = get_regions(&grid);

    regions.iter().fold(0, |acc, region| {
        acc + (region.1.len() * get_sides(&region.1))
    })
}

fn get_regions(grid: &Grid<char>) -> Vec<(char, Vec<Point>)> {
    let mut regions = Vec::new();
    let mut added = HashSet::new();
    for cell in grid.iter() {
        if !added.insert(cell.point) {
            continue;
        }
        let mut region = vec![cell.point];

        let mut points = VecDeque::new();
        points.push_back(cell.point);
        while let Some(next) = points.pop_front() {
            let neighbors = grid.get_adjacent(&next);
            for neighbor in neighbors {
                if grid.get(&neighbor).unwrap() != &cell.value {
                    continue;
                }

                if !added.insert(neighbor) {
                    continue;
                }
                region.push(neighbor);
                points.push_back(neighbor);
            }
        }

        regions.push((cell.value, region));
    }
    regions
}

fn get_sides(points: &Vec<Point>) -> usize {
    let mut corners = 0;
    for point in points {
        let mut neighbors = vec![];
        if let Some(p) = point.up() {
            neighbors.push(p);
        }
        if let Some(p) = point.down() {
            neighbors.push(p);
        }
        if let Some(p) = point.left() {
            neighbors.push(p);
        }
        if let Some(p) = point.right() {
            neighbors.push(p);
        }
        let mut diagonal_neighbors = vec![];
        if let Some(p) = point.up_left() {
            diagonal_neighbors.push(p);
        }
        if let Some(p) = point.up_right() {
            diagonal_neighbors.push(p);
        }
        if let Some(p) = point.down_left() {
            diagonal_neighbors.push(p);
        }
        if let Some(p) = point.down_right() {
            diagonal_neighbors.push(p);
        }

        let cardinal: Vec<Point> = neighbors
            .iter()
            .filter(|&p| points.contains(p))
            .cloned()
            .collect();
        let diagonal: Vec<Point> = diagonal_neighbors
            .iter()
            .filter(|&p| points.contains(p))
            .cloned()
            .collect();

        match cardinal.len() {
            0 => {
                corners += 4;
            }
            1 => {
                corners += 2;
            }
            2 => {
                let up = point.up();
                let down = point.down();
                let left = point.left();
                let right = point.right();
                let up_left = point.up_left();
                let up_right = point.up_right();
                let down_left = point.down_left();
                let down_right = point.down_right();

                if up.is_some()
                    && left.is_some()
                    && cardinal.contains(&up.unwrap())
                    && cardinal.contains(&left.unwrap())
                {
                    corners += 2;
                    if up_left.is_some() && diagonal.contains(&up_left.unwrap()) {
                        corners -= 1;
                    }
                } else if up.is_some()
                    && right.is_some()
                    && cardinal.contains(&up.unwrap())
                    && cardinal.contains(&right.unwrap())
                {
                    corners += 2;
                    if up_right.is_some() && diagonal.contains(&up_right.unwrap()) {
                        corners -= 1;
                    }
                } else if down.is_some()
                    && left.is_some()
                    && cardinal.contains(&down.unwrap())
                    && cardinal.contains(&left.unwrap())
                {
                    corners += 2;
                    if down_left.is_some() && diagonal.contains(&down_left.unwrap()) {
                        corners -= 1;
                    }
                } else if down.is_some()
                    && right.is_some()
                    && cardinal.contains(&down.unwrap())
                    && cardinal.contains(&right.unwrap())
                {
                    corners += 2;
                    if down_right.is_some() && diagonal.contains(&down_right.unwrap()) {
                        corners -= 1;
                    }
                }
            }
            3 => {
                corners += 2;
                let up = point.up();
                let down = point.down();
                let left = point.left();
                let right = point.right();
                let up_left = point.up_left();
                let up_right = point.up_right();
                let down_left = point.down_left();
                let down_right = point.down_right();

                if up.is_none() || !cardinal.contains(&up.unwrap()) {
                    if down_left.is_some() && diagonal.contains(&down_left.unwrap()) {
                        corners -= 1;
                    }
                    if down_right.is_some() && diagonal.contains(&down_right.unwrap()) {
                        corners -= 1;
                    }
                } else if down.is_none() || !cardinal.contains(&down.unwrap()) {
                    if up_left.is_some() && diagonal.contains(&up_left.unwrap()) {
                        corners -= 1;
                    }
                    if up_right.is_some() && diagonal.contains(&up_right.unwrap()) {
                        corners -= 1;
                    }
                } else if left.is_none() || !cardinal.contains(&left.unwrap()) {
                    if up_right.is_some() && diagonal.contains(&up_right.unwrap()) {
                        corners -= 1;
                    }
                    if down_right.is_some() && diagonal.contains(&down_right.unwrap()) {
                        corners -= 1;
                    }
                } else if right.is_none() || !cardinal.contains(&right.unwrap()) {
                    if up_left.is_some() && diagonal.contains(&up_left.unwrap()) {
                        corners -= 1;
                    }
                    if down_left.is_some() && diagonal.contains(&down_left.unwrap()) {
                        corners -= 1;
                    }
                }
            }
            _ => {
                corners += 4 - diagonal.len();
            }
        }
    }
    corners
}

fn get_perimeter(points: &Vec<Point>) -> usize {
    let mut perimeter = 0;
    for point in points {
        let mut neighbors = vec![];
        if let Some(p) = point.up() {
            neighbors.push(p);
        }
        if let Some(p) = point.down() {
            neighbors.push(p);
        }
        if let Some(p) = point.left() {
            neighbors.push(p);
        }
        if let Some(p) = point.right() {
            neighbors.push(p);
        }

        let adjacent_count = neighbors.iter().filter(|&p| points.contains(p)).count();
        perimeter += 4 - adjacent_count;
    }
    perimeter
}
