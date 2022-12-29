use std::collections::{HashMap, HashSet, VecDeque};
use std::fs;
use std::time::Instant;

struct Node {
    end: bool,
    connections: Vec<(usize, usize)>,
}

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");

    let start = Instant::now();
    let part_one = part_one(&input);
    let duration_one = start.elapsed();
    println!("Part one: {}, took: {:?}", part_one, duration_one);

    let part_two = part_two(&input);
    let duration_two = start.elapsed() - duration_one;
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn part_one(input: &String) -> i32 {
    let nodes = get_nodes(input, false);

    distance(&nodes.1[0], &nodes.0, &i32::MAX).unwrap()
}

fn part_two(input: &String) -> i32 {
    let nodes = get_nodes(input, true);
    let mut smallest = i32::MAX;
    for start in nodes.1 {
        let distance =distance(&start, &nodes.0, &smallest);
        if distance.is_some() && distance.unwrap() < smallest {
            smallest = distance.unwrap();
        }        
    }

    smallest
}

fn distance(start: &(usize, usize), nodes: &HashMap<(usize, usize), Node>, cut_off: &i32) -> Option<i32> {
    let mut queue = VecDeque::new();
    let mut visited = HashSet::new();

    queue.push_back((0, start));
    visited.insert(start);

    while let Some((distance, node)) = queue.pop_front() {
        if &distance >= cut_off {
            return None;
        }
        if nodes[node].end {
            return Some(distance);
        }
        for connection in &nodes[node].connections {            
            if visited.insert(connection) {                
                queue.push_back((distance + 1, connection));
            }
        }
    }
    None
}

fn get_nodes(input: &String, include_a: bool) -> (HashMap<(usize, usize), Node>, Vec<(usize, usize)>) {
    let mut map = HashMap::new();
    let mut start = vec![];
    let mut height_map = HashMap::new();
    let mut height = 0usize;
    let mut width = 0usize;
    for (j, row) in input.lines().enumerate() {
        if j + 1 > height {
            height = j + 1;
        }
        for (i, cell) in row.bytes().enumerate() {
            if i + 1 > width {
                width = i + 1;
            }
            if cell == b'S' || (include_a && cell == b'a'){
                start.push((i, j));
            }
            map.insert(
                (i, j),
                Node {
                    end: cell == b'E',
                    connections: vec![],
                },
            );
            height_map.insert(
                (i, j),
                if cell == b'S' {
                    b'a'
                } else if cell == b'E' {
                    b'z'
                } else {
                    cell
                } % 32
                    - 1,
            );
        }
    }
    for j in 0..height {
        for i in 0..width {
            let current = map.get_mut(&(i, j)).unwrap();
            let value = height_map[&(i, j)];
            if j > 0 && height_map[&(i, j - 1)] <= value + 1 {
                current.connections.push((i, j - 1));
            }
            if j < height - 1 && height_map[&(i, j + 1)] <= value + 1 {
                current.connections.push((i, j + 1));
            }
            if i > 0 && height_map[&(i - 1, j)] <= value + 1 {
                current.connections.push((i - 1, j));
            }
            if i < width - 1 && height_map[&(i + 1, j)] <= value + 1 {
                current.connections.push((i + 1, j));
            }
        }
    }

    (map, start)
}
