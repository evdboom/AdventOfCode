extern crate grid_helper;

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
    let galaxies = get_expanded_galaxies(input, 2);
    let with_index = galaxies.iter().enumerate().collect::<Vec<_>>();

    with_index.iter().fold(0, |acc, galaxy| {
        let distances: usize = with_index
            .iter()
            .filter(|&(index, _)| index > &galaxy.0)
            .map(|(_, other)| galaxy.1.manhattan_distance(other))
            .sum();
        acc + distances
    })
}

fn process_part_two(input: &str) -> usize {
    let galaxies = get_expanded_galaxies(input, 1000000);
    let with_index = galaxies.iter().enumerate().collect::<Vec<_>>();

    with_index.iter().fold(0, |acc, galaxy| {
        let distances: usize = with_index
            .iter()
            .filter(|&(index, _)| index > &galaxy.0)
            .map(|(_, other)| galaxy.1.manhattan_distance(other))
            .sum();
        acc + distances
    })
}

fn get_expanded_galaxies(input: &str, factor: usize) -> Vec<Point> {
    let grid: Grid<bool> = Grid::from_string(input, |c| c == '#');

    let mut galaxy_free_columns = Vec::new();
    for i in 0..grid.width() {
        let column = grid.get_column(i).unwrap();
        if column.iter().all(|&cell| !cell) {
            galaxy_free_columns.push(i);
        }
    }

    let mut galaxy_free_rows = Vec::new();
    for i in 0..grid.height() {
        let row = grid.get_row(i).unwrap();
        if row.iter().all(|&cell| !cell) {
            galaxy_free_rows.push(i);
        }
    }

    grid.iter()
        .filter(|cell| cell.value)
        .map(|cell| {
            let x = cell.point.x
                + (factor - 1)
                    * galaxy_free_columns
                        .iter()
                        .filter(|&i| *i < cell.point.x)
                        .count();
            let y = cell.point.y
                + (factor - 1)
                    * galaxy_free_rows
                        .iter()
                        .filter(|&i| *i < cell.point.y)
                        .count();
            Point::new(x, y)
        })
        .collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....",
        );

        assert_eq!(374, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....",
        );

        assert_eq!(82000210, process_part_two(&input));
    }
}
