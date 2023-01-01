use std::collections::HashSet;
use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");

    let start = Instant::now();
    let part_one = part_one(&input);
    let duration_one = start.elapsed();
    let part_two = part_two(&input);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn part_one(input: &String) -> usize {
    let mut grains = 0;
    let mut blocks = get_blocks(input);

    loop {
        let grain = drop_grain(&blocks.0, &blocks.1);
        if grain.1 < blocks.1 {
            grains += 1;
            blocks.0.insert(grain);
        } else {
            break;
        }
    }

    grains
}

fn part_two(input: &String) -> i32 {
    let mut grains = 0;
    let mut blocks = get_blocks(input);

    loop {
        let grain = drop_grain(&blocks.0, &(blocks.1 + 1));
        if grain.1 > 0  {
            grains += 1;
            blocks.0.insert(grain);
        } else {
            break;
        }
    }

    grains
}

fn drop_grain(blocks: &HashSet<(usize, usize)>, y_max: &usize) -> (usize, usize) {
    let mut grain = (500usize, 0usize);
    loop {
        if blocks.contains(&(grain.0, grain.1 + 1)) {
            if blocks.contains(&(grain.0 - 1, grain.1 + 1)) {
                if blocks.contains(&(grain.0 + 1, grain.1 + 1)) {
                    return grain;
                } else {
                    grain.0 += 1;
                    grain.1 += 1;
                }
            } else {
                grain.0 -= 1;
                grain.1 += 1;
            }
        } else {
            grain.1 += 1;
        }
        if &grain.1 == y_max {
            break;
        }
    }
    grain
}

fn get_blocks(input: &String) -> (HashSet<(usize, usize)>, usize) {
    let mut result = HashSet::new();
    let mut max_y = 0;
    for line in input.lines() {
        let points: Vec<(usize, usize)> = line
            .split(" -> ")
            .map(|coord| {
                let mut parts = coord.split(",").map(|part| part.parse::<usize>().unwrap());
                (parts.next().unwrap(), parts.next().unwrap())
            })
            .collect();

        for i in 0..(points.len() - 1) {
            for point in get_points(points[i], points[i + 1]) {
                if point.1 > max_y {
                    max_y = point.1;
                }
                result.insert(point);
            }
        }
    }

    (result, max_y)
}

fn get_points(from: (usize, usize), to: (usize, usize)) -> Vec<(usize, usize)> {
    let mut result = vec![];
    if from.0 == to.0 {
        for i in 0..=from.1.abs_diff(to.1) {
            result.push((from.0, from.1.min(to.1) + i));
        }
    } else {
        for i in 0..=from.0.abs_diff(to.0) {
            result.push((from.0.min(to.0) + i, from.1));
        }
    }

    result
}
