use std::collections::HashMap;
use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = process_part_one(&input, (101, 103));
    let duration_one = start.elapsed();
    let part_two = process_part_two(&input, (101, 103));
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn process_part_one(input: &str, dimension: (isize, isize)) -> usize {
    let mut robots = input
        .lines()
        .map(|line| {
            let mut parts = line.split_whitespace();
            let mut position = parts.next().unwrap().split(',');
            let x: isize = position.next().unwrap()[2..].parse().unwrap();
            let y: isize = position.next().unwrap().parse().unwrap();
            let mut velocity = parts.next().unwrap().split(',');
            let vx: isize = velocity.next().unwrap()[2..].parse().unwrap();
            let vy: isize = velocity.next().unwrap().parse().unwrap();
            ((x, y), (vx, vy))
        })
        .collect::<Vec<_>>();

    for _ in 0..100 {
        let mut new_robots = Vec::new();
        for ((x, y), (vx, vy)) in robots {
            let mut new_x = x + vx;
            if new_x < 0 {
                new_x = dimension.0 + new_x;
            } else if new_x >= dimension.0 {
                new_x = new_x - dimension.0;
            }
            let mut new_y = y + vy;
            if new_y < 0 {
                new_y = dimension.1 + new_y;
            } else if new_y >= dimension.1 {
                new_y = new_y - dimension.1;
            }
            new_robots.push(((new_x, new_y), (vx, vy)));
        }
        robots = new_robots;
    }

    let mut result = 1;
    for j in 0..2 {
        for i in 0..2 {
            let x_start = i * dimension.0 / 2 + i;
            let x_end = (i + 1) * dimension.0 / 2;
            let y_start = j * dimension.1 / 2 + j;
            let y_end = (j + 1) * dimension.1 / 2;

            let quadrant = robots
                .iter()
                .filter(|((x, y), _)| x >= &x_start && x < &x_end && y >= &y_start && y < &y_end)
                .count();
            result *= quadrant;
        }
    }

    result
}

fn process_part_two(input: &str, dimension: (isize, isize)) -> usize {
    let mut robots = input
        .lines()
        .map(|line| {
            let mut parts = line.split_whitespace();
            let mut position = parts.next().unwrap().split(',');
            let x: isize = position.next().unwrap()[2..].parse().unwrap();
            let y: isize = position.next().unwrap().parse().unwrap();
            let mut velocity = parts.next().unwrap().split(',');
            let vx: isize = velocity.next().unwrap()[2..].parse().unwrap();
            let vy: isize = velocity.next().unwrap().parse().unwrap();
            ((x, y), (vx, vy))
        })
        .collect::<Vec<_>>();

    // net actually needed, but feels cleaner :)
    let loop_count = find_loops(&mut robots, dimension);

    for k in 0..loop_count {
        let (in_column, in_row) =
            robots
                .iter()
                .fold((HashMap::new(), HashMap::new()), |mut acc, ((x, y), _)| {
                    acc.0.entry(x).or_insert(0);
                    acc.1.entry(y).or_insert(0);
                    acc.0.insert(x, acc.0[&x] + 1);
                    acc.1.insert(y, acc.1[&y] + 1);
                    (acc.0, acc.1)
                });

        // from moving backward, found the image then made the filter for forward move.. not the prettiest solution
        // better would be to check spread, or neighbour count.. perhaps for c# solution
        let big_enough = in_column.iter().filter(|(_, &v)| v > 30).count() >= 2
            && in_row.iter().filter(|(_, &v)| v > 30).count() >= 2;
        if big_enough {
            return k;
        }

        let mut new_robots = Vec::new();
        for ((x, y), (vx, vy)) in robots {
            let mut new_x = x + vx;
            if new_x < 0 {
                new_x = dimension.0 + new_x;
            } else if new_x >= dimension.0 {
                new_x = new_x - dimension.0;
            }
            let mut new_y = y + vy;
            if new_y < 0 {
                new_y = dimension.1 + new_y;
            } else if new_y >= dimension.1 {
                new_y = new_y - dimension.1;
            }
            new_robots.push(((new_x, new_y), (vx, vy)));
        }
        robots = new_robots;
    }

    1
}

fn find_loops(
    robots: &mut Vec<((isize, isize), (isize, isize))>,
    dimension: (isize, isize),
) -> usize {
    let mut loops = 0;

    for ((x, y), (vx, vy)) in robots.iter() {
        let mut runs = 0;
        let start_point = (x.clone(), y.clone());
        let mut point = (x.clone(), y.clone());
        loop {
            let mut new_x = point.0 + vx;
            if new_x < 0 {
                new_x = dimension.0 + new_x;
            } else if new_x >= dimension.0 {
                new_x = new_x - dimension.0;
            }
            let mut new_y = point.1 + vy;
            if new_y < 0 {
                new_y = dimension.1 + new_y;
            } else if new_y >= dimension.1 {
                new_y = new_y - dimension.1;
            }

            point = (new_x, new_y);
            runs += 1;
            if point == start_point {
                if runs > loops {
                    loops = runs;
                }
                break;
            }
        }
    }
    loops
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3",
        );

        assert_eq!(12, process_part_one(&input, (11, 7)));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3",
        );

        assert_eq!(1, process_part_two(&input, (11, 7)));
    }
}
