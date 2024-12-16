extern crate grid_helper;
mod position;

use grid_helper::grid::{Direction, Grid, Point};
use position::Position;
use std::collections::{BinaryHeap, HashMap, HashSet};
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
    let maze = grid_helper::grid::Grid::from_string(input, |c| c);

    let start = maze
        .iter()
        .find(|cell| cell.value == 'S')
        .unwrap()
        .point
        .clone();
    let end = maze
        .iter()
        .find(|cell| cell.value == 'E')
        .unwrap()
        .point
        .clone();

    get_best_paths(maze, start, end, false).0
}

fn process_part_two(input: &str) -> usize {
    let maze = grid_helper::grid::Grid::from_string(input, |c| c);

    let start = maze
        .iter()
        .find(|cell| cell.value == 'S')
        .unwrap()
        .point
        .clone();
    let end = maze
        .iter()
        .find(|cell| cell.value == 'E')
        .unwrap()
        .point
        .clone();

    get_best_paths(maze, start, end, true).1.len()
}

fn get_best_paths(
    maze: Grid<char>,
    start: Point,
    end: Point,
    get_all: bool,
) -> (usize, HashSet<Point>) {
    let mut visited = HashMap::new();
    let mut points_visited = HashSet::new();
    let mut moves = BinaryHeap::new();
    let start_direction = Direction::right(start);
    moves.push(Position::new(start_direction.clone(), 0, &HashSet::new()));
    let mut best = usize::MAX;
    while let Some(position) = moves.pop() {
        let exists = visited
            .entry(position.direction.clone())
            .or_insert(usize::MAX);
        if *exists < position.cost {
            continue;
        } else {
            *exists = position.cost;
        }

        if position.point() == end && position.cost <= best {
            best = position.cost;
            for point in position.visited.iter() {
                points_visited.insert(point.clone());
            }
            if !get_all {
                break;
            }
        }
        maze.get_filtered_directions(&position.point(), |value| value != &'#')
            .iter()
            .for_each(|adjacent| {
                if position.direction.same_direction(adjacent) {
                    moves.push(Position::new(
                        adjacent.clone(),
                        position.cost + 1,
                        &position.visited,
                    ));
                } else {
                    moves.push(Position::new(
                        adjacent.in_same_direction(position.point()),
                        position.cost + 1000,
                        &position.visited,
                    ));
                };
            });
    }

    (best, points_visited)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one_one() {
        let input = String::from(
            "###############
#.......#....E#
#.#.###.#.###.#
#.....#.#...#.#
#.###.#####.#.#
#.#.#.......#.#
#.#.#####.###.#
#...........#.#
###.#.#####.#.#
#...#.....#.#.#
#.#.#.###.#.#.#
#.....#...#.#.#
#.###.#.#.#.#.#
#S..#.....#...#
###############",
        );

        assert_eq!(7036, process_part_one(&input));
    }

    #[test]
    fn part_one_two() {
        let input = String::from(
            "#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################",
        );

        assert_eq!(11048, process_part_one(&input));
    }

    #[test]
    fn part_two_one() {
        let input = String::from(
            "###############
#.......#....E#
#.#.###.#.###.#
#.....#.#...#.#
#.###.#####.#.#
#.#.#.......#.#
#.#.#####.###.#
#...........#.#
###.#.#####.#.#
#...#.....#.#.#
#.#.#.###.#.#.#
#.....#...#.#.#
#.###.#.#.#.#.#
#S..#.....#...#
###############",
        );

        assert_eq!(45, process_part_two(&input));
    }

    #[test]
    fn part_two_two() {
        let input = String::from(
            "#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################",
        );

        assert_eq!(64, process_part_two(&input));
    }
}
