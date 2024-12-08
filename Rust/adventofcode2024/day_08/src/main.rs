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

fn process_part_one(input: &str) -> usize {
    let (antennas, (max_x, max_y)) = get_antennas(input);
    antennas
        .iter()
        .fold(HashSet::new(), |mut acc, (_, locations)| {
            for (x, y) in get_antinodes(locations, max_x, max_y, false) {
                acc.insert((x, y));
            }
            acc
        })
        .len()
}

fn process_part_two(input: &str) -> usize {
    let (antennas, (max_x, max_y)) = get_antennas(input);
    antennas
        .iter()
        .fold(HashSet::new(), |mut acc, (_, locations)| {
            for (x, y) in get_antinodes(locations, max_x, max_y, true) {
                acc.insert((x, y));
            }
            acc
        })
        .len()
}

fn get_antinodes(
    locations: &Vec<(usize, usize)>,
    max_x: usize,
    max_y: usize,
    all_locations: bool,
) -> Vec<(usize, usize)> {
    let mut antinodes = Vec::new();
    for i in 0..locations.len() - 1 {
        let current = locations[i];
        if all_locations {
            antinodes.push(current);
        }
        for j in i + 1..locations.len() {
            let next = locations[j];
            let distance_x = current.0 as isize - next.0 as isize;
            let distance_y = current.1 as isize - next.1 as isize;

            let mut run = true;
            let mut point_one = (current.0 as isize, current.1 as isize);
            let mut point_two = (next.0 as isize, next.1 as isize);
            while run
                && (point_on_map(point_one, max_x, max_y) || point_on_map(point_two, max_x, max_y))
            {
                run = all_locations;
                point_one.0 += distance_x;
                point_one.1 += distance_y;
                point_two.0 -= distance_x;
                point_two.1 -= distance_y;

                if point_on_map(point_one, max_x, max_y) {
                    antinodes.push((point_one.0 as usize, point_one.1 as usize));
                }
                if point_on_map(point_two, max_x, max_y) {
                    antinodes.push((point_two.0 as usize, point_two.1 as usize));
                }
            }
        }
        if all_locations {
            antinodes.push(locations[locations.len() - 1]);
        }
    }
    antinodes
}

fn point_on_map(point: (isize, isize), max_x: usize, max_y: usize) -> bool {
    point.0 >= 0 && point.0 <= max_x as isize && point.1 >= 0 && point.1 <= max_y as isize
}

fn get_antennas(input: &str) -> (HashMap<char, Vec<(usize, usize)>>, (usize, usize)) {
    let mut antennas = HashMap::new();
    let mut max = (0, 0);
    for (y, line) in input.lines().enumerate() {
        max.1 = max.1.max(y);
        for (x, c) in line.chars().enumerate() {
            if c != '.' {
                let entry = antennas.entry(c).or_insert(Vec::new());
                entry.push((x, y));
                max.0 = max.0.max(x);
            }
        }
    }
    (antennas, max)
}
