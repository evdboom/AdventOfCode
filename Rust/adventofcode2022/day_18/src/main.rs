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
    let grid = get_grid(input);
    let mut start_points = vec![];
    for i in 0..grid.len() {
        for j in 0..grid[0].len() {
            for k in 0..grid[0][0].len() {
                if !grid[i][j][k] {
                    start_points.push((i, j, k));
                }
            }
        }
    }
    get_sides(grid, start_points)
}

fn part_two(input: &String) -> usize {
    let grid = get_grid(input);
    let start_points = vec![(0, 0, 0)];
    get_sides(grid, start_points)
}

fn get_sides(grid: Vec<Vec<Vec<bool>>>, start_points: Vec<(usize, usize, usize)>) -> usize {
    let mut visited = HashSet::new();
    let mut touched = HashSet::new();

    for start_point in start_points {
        if !visited.insert(start_point) {
            continue;
        }

        let mut queue = vec![];
        queue.push(start_point);
        while let Some((x, y, z)) = queue.pop() {
            if x > 0 {
                if grid[x - 1][y][z] {
                    touched.insert((x - 1, y, z, 1));
                } else if visited.insert((x - 1, y, z)) {
                    queue.push((x - 1, y, z));
                }
            }
            if x < grid.len() - 1 {
                if grid[x + 1][y][z] {
                    touched.insert((x + 1, y, z, 2));
                } else if visited.insert((x + 1, y, z)) {
                    queue.push((x + 1, y, z));
                }
            }
            if y > 0 {
                if grid[x][y - 1][z] {
                    touched.insert((x, y - 1, z, 3));
                } else if visited.insert((x, y - 1, z)) {
                    queue.push((x, y - 1, z));
                }
            }
            if y < grid[0].len() - 1 {
                if grid[x][y + 1][z] {
                    touched.insert((x, y + 1, z, 4));
                } else if visited.insert((x, y + 1, z)) {
                    queue.push((x, y + 1, z));
                }
            }
            if z > 0 {
                if grid[x][y][z - 1] {
                    touched.insert((x, y, z - 1, 5));
                } else if visited.insert((x, y, z - 1)) {
                    queue.push((x, y, z - 1));
                }
            }
            if z < grid[0][0].len() - 1 {
                if grid[x][y][z + 1] {
                    touched.insert((x, y, z + 1, 6));
                } else if visited.insert((x, y, z + 1)) {
                    queue.push((x, y, z + 1));
                }
            }
        }
    }

    touched.len()
}

fn get_grid(input: &String) -> Vec<Vec<Vec<bool>>> {
    let mut points = HashSet::new();
    let mut max_x = 0;
    let mut max_y = 0;
    let mut max_z = 0;
    for line in input.lines() {
        let point: Vec<usize> = line
            .split(",")
            .map(|s| s.parse::<usize>().unwrap() + 1)
            .collect();
        if point[0] > max_x {
            max_x = point[0];
        }
        if point[1] > max_y {
            max_y = point[1];
        }
        if point[2] > max_z {
            max_z = point[2];
        }
        points.insert((point[0], point[1], point[2]));
    }
    let mut result = vec![];
    for i in 0..=(max_x + 1) {
        result.push(vec![]);
        for j in 0..=(max_y + 1) {
            result[i].push(vec![]);
            for k in 0..=(max_z + 1) {
                if points.contains(&(i, j, k)) {
                    result[i][j].push(true);
                } else {
                    result[i][j].push(false);
                }
            }
        }
    }
    result
}
