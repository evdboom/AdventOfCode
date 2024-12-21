extern crate grid_helper;

use grid_helper::grid::{Grid, Point};
use std::collections::HashSet;
use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = process_part_one(&input, 100);
    let duration_one = start.elapsed();
    let part_two = process_part_two(&input, 100);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn process_part_one(input: &str, time_to_save: usize) -> usize {
    let map = Grid::from_string(input, |c| c);
    let points = get_cheatless_data(&map);
    get_cheats(&points, time_to_save, 2)
}

fn process_part_two(input: &str, time_to_save: usize) -> usize {
    let map = Grid::from_string(input, |c| c);
    let points = get_cheatless_data(&map);
    get_cheats(&points, time_to_save, 20)
}

fn get_cheats(points: &Vec<Point>, time_to_save: usize, cheat_length: usize) -> usize {
    let mut result = 0;
    for i in 0..points.len() {
        for j in 0..points.len() {
            let distance = get_distance(&points[i], &points[j]);
            if j + distance >= i || distance > cheat_length {
                continue;
            } else if i - (j + distance) >= time_to_save {
                result += 1;
            }
        }
    }
    result
}

fn get_distance(point: &Point, target: &Point) -> usize {
    point.x.abs_diff(target.x) + point.y.abs_diff(target.y)
}

fn get_cheatless_data(map: &Grid<char>) -> Vec<Point> {
    let start = map.iter().find(|c| c.value == 'S').unwrap().point;
    let mut position = map.iter().find(|c| c.value == 'E').unwrap().point;

    let mut path = vec![position];
    let mut points = HashSet::new();
    points.insert(position);
    while position != start {
        for point in map.get_filtered_adjacent(&position, |c| *c != '#').iter() {
            if points.insert(*point) {
                position = *point;
                path.push(position);
                break;
            }
        }
    }

    path
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one_one() {
        let input = String::from(
            "###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############",
        );

        assert_eq!(1, process_part_one(&input, 64));
    }

    #[test]
    fn part_one_two() {
        let input = String::from(
            "###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############",
        );

        assert_eq!(5, process_part_one(&input, 20));
    }

    #[test]
    fn part_two_one() {
        let input = String::from(
            "###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############",
        );

        assert_eq!(285, process_part_two(&input, 50));
    }

    #[test]
    fn part_two_two() {
        let input = String::from(
            "###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############",
        );

        assert_eq!(129, process_part_two(&input, 60));
    }
}
