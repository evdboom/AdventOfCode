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
    let mut regions: Vec<(char, Vec<Point>)> = Vec::new();
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
                if (grid.get(&neighbor).unwrap() != &cell.value) {
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

    regions.iter().fold(0, |acc, region| {
        acc + (region.1.len() * get_perimeter(&region.1))
    })
}

fn process_part_two(input: &str) -> usize {
    2
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
