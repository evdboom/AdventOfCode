use std::collections::HashMap;
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
    let directories = get_directories(input);
    directories.iter().filter(|directory| directory <= &&100000).sum()
}

fn part_two(input: &String) -> i32 {
    let mut directories = get_directories(input);

    let total: &i32 = directories.iter().max().unwrap();
    let free = 70000000 - total;
    let required = 30000000 - free;
    directories.sort_unstable();
    let mut result = 0;
    for directory in directories {
        if directory >= required {
            result = directory;
            break;
        }
    }

    result
}

fn get_directories(input: &String) -> Vec<i32> {
    let mut directories = HashMap::new();
    let mut path = String::from(".");

    directories.insert(path.clone(), 0);

    for line in input.lines() {
        if line.starts_with("$ cd ") {
            let new_path = line.replace("$ cd ", "");
            match &new_path[..] {
                "/" => path.replace_range(.., "."),
                ".." => {
                    while let Some(char) = path.pop() {
                        if char == '/' {
                            break;
                        }
                    }
                }
                _ => {
                    path.push_str(&format!("/{}", &new_path[..])[..]);
                    directories.insert(path.clone(), 0);
                }
            }
        } else if !line.starts_with("dir") && !line.starts_with("$") {
            let value = line
                .split_whitespace()
                .next()
                .unwrap()
                .parse::<i32>()
                .unwrap();
            let current = directories.get_mut(&path[..]).unwrap();
            *current += value;
        }
    }

    let mut result = Vec::new();
    for (path, _) in &directories {
        let path_sum = directories
            .iter()
            .filter(|(key, _)| key.starts_with(path))
            .fold(0, |sum, (_, value)| sum + value);
            result.push(path_sum);
    }

    result
}
