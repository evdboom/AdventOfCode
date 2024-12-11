extern crate grid_helper;

mod points_collection;

use crate::points_collection::PointsCollection;
use grid_helper::grid::{Grid, Point};
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
    let grid = Grid::from_string(input, |c| c.to_digit(10).unwrap() as usize);

    grid.iter()
        .filter(|cell| cell.value == 0)
        .fold(0, |acc, cell| acc + get_scores(&grid, cell.point, true))
}

fn process_part_two(input: &str) -> usize {
    let grid = Grid::from_string(input, |c| c.to_digit(10).unwrap() as usize);

    grid.iter()
        .filter(|cell| cell.value == 0)
        .fold(0, |acc, cell| acc + get_scores(&grid, cell.point, false))
}

fn get_scores(grid: &Grid<usize>, point: Point, unique: bool) -> usize {
    let mut points = PointsCollection::new(unique);

    for next in get_next_points(grid, point, 0) {
        points.insert(next);
    }

    points.len()
}

fn get_next_points(grid: &Grid<usize>, point: Point, value: usize) -> Vec<Point> {
    let mut next_points = Vec::new();

    if value == 9 {
        next_points.push(point);
        return next_points;
    }

    for point in get_adjacent(grid, point, value) {
        for next in get_next_points(grid, point, value + 1) {
            next_points.push(next);
        }
    }

    next_points
}

fn get_adjacent(grid: &Grid<usize>, point: Point, value: usize) -> Vec<Point> {
    let mut adjacent = Vec::new();
    let directions = vec![point.left(), point.right(), point.up(), point.down()];

    for direction in directions {
        if let Some(adjecent_point) = direction {
            if let Some(new_value) = grid.get(&adjecent_point) {
                if value + 1 == *new_value {
                    adjacent.push(adjecent_point);
                }
            }
        }
    }

    adjacent
}
