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

fn part_one(input: &String) -> i32 {
    let mut grid = get_grid(input, 0);
    let mut grains = 0;

    loop {
        let grain = drop_grain(&grid.0, grid.1, grid.2);
        if grain.1 < grid.1 {
            grains += 1;
            grid.0[grain.1][grain.0] = true;
        } else {
            break;
        }
    }

    grains
}

fn part_two(input: &String) -> i32 {
    let mut grid = get_grid(input, 1);
    let mut grains = 0;
    while !grid.0[0][grid.2] {
        let grain = drop_grain(&grid.0, grid.1, grid.2);
        grid.0[grain.1][grain.0] = true;
        grains += 1;
    }

    grains
}

fn get_grid(input: &String, add_lines: usize) -> (Vec<Vec<bool>>, usize, usize) {
    let blocks = get_blocks(input);
    let mut result = vec![];

    let width_parts = blocks.2;
    let width = 1 + 2 * add_lines + width_parts.0 + width_parts.1;
    for j in 0..=(blocks.1 + add_lines) {
        result.push(vec![]);
        for i in 0..=width {
            result[j].push(blocks.0.contains(&(499 - add_lines - width_parts.0 + i, j)));
        }
    }

    (result, blocks.1 + add_lines, width_parts.0 + add_lines + 1)
}

fn drop_grain(grid: &Vec<Vec<bool>>, y_max: usize, x_min: usize) -> (usize, usize) {
    let mut grain = (x_min, 0);
    while grain.1 < y_max {
        if grid[grain.1 + 1][grain.0] {
            if grid[grain.1 + 1][grain.0 - 1] {
                if grid[grain.1 + 1][grain.0 + 1] {
                    break;
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
    }
    grain
}

fn get_blocks(input: &String) -> (HashSet<(usize, usize)>, usize, (usize, usize)) {
    let mut result = HashSet::new();
    let mut max_y = 0;
    let mut width_left = 0;
    let mut width_right = 0;
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
                if point.1 > width_left {
                    width_left = point.1;
                }
                if point.1 > width_right {
                    width_right = point.1;
                }
                if point.0 < 500 && 500 - point.0 > width_left {
                    width_left = 500 - point.0
                } else if point.0 > 500 && point.0 - 500 > width_right {
                    width_right = point.0 - 500;
                }

                result.insert(point);
            }
        }
    }

    (result, max_y, (width_left, width_right))
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
