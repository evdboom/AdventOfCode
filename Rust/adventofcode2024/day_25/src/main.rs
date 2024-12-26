extern crate grid_helper;

use grid_helper::grid::{Grid, Point};

use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = process_part_one(&input);
    let duration_one = start.elapsed();
    println!("Part one: {}, took: {:?}", part_one, duration_one);
}

fn process_part_one(input: &str) -> usize {
    let (locks, keys) = get_locks_and_keys(input);

    let mut result = 0;
    for lock in locks.iter() {
        for key in keys.iter() {
            if lock
                .iter()
                .zip(key.iter())
                .all(|(l, k)| !l.value || !k.value)
            {
                result += 1;
            }
        }
    }

    result
}

fn get_locks_and_keys(input: &str) -> (Vec<Grid<bool>>, Vec<Grid<bool>>) {
    let parts = input.split("\n\n");

    let mut locks = Vec::new();
    let mut keys = Vec::new();

    for part in parts {
        let grid = Grid::from_string(part, |c| c == '#');
        if *grid.get(&Point::new(0, 0)).unwrap() {
            locks.push(grid);
        } else {
            keys.push(grid);
        }
    }

    (locks, keys)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "#####
.####
.####
.####
.#.#.
.#...
.....

#####
##.##
.#.##
...##
...#.
...#.
.....

.....
#....
#....
#...#
#.#.#
#.###
#####

.....
.....
#.#..
###..
###.#
###.#
#####

.....
.....
.....
#....
#.#..
#.#.#
#####",
        );

        assert_eq!(3, process_part_one(&input));
    }
}
