use std::collections::{HashMap, HashSet, VecDeque};
use std::fs;
use std::time::Instant;

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
    distance(&nodes.1[0], &nodes.2, &nodes.0).0.unwrap()
}

fn part_two(input: &String) -> i32 {
    let nodes = get_nodes(input, true);
    let mut smallest = i32::MAX;
    let mut dead_ends = HashSet::new();
    for start in &nodes.1 {
        if dead_ends.contains(start) {
            continue;
        }
        let distance = distance(start, &nodes.2, &nodes.0);
        if distance.0.is_some() {
            let value = distance.0.unwrap();
            smallest = smallest.min(value);            
        } else {
            for node in distance.1 {
                dead_ends.insert(node);
            }
        }
    }

    smallest
}

fn distance(
    start: &(usize, usize),
    end: &(usize, usize),
    nodes: &HashMap<(usize, usize), Vec<(usize, usize)>>
) -> (Option<i32>, HashSet<(usize, usize)>) {
    let mut queue = VecDeque::new();
    let mut visited = HashSet::new();

    queue.push_back((0, start));
    visited.insert((start.0, start.1));

    while let Some((distance, node)) = queue.pop_front() {     
        for connection in &nodes[node] {
            if connection == end {
                return (Some(distance + 1), visited);
            }

            if visited.insert((connection.0, connection.1)) {
                queue.push_back((distance + 1, connection))
            }
        }
    }    
    (None, visited)
}

fn get_nodes(
    input: &String,
    include_a: bool,
) -> (
    HashMap<(usize, usize), Vec<(usize, usize)>>,
    Vec<(usize, usize)>,
    (usize, usize),
) {
    let mut map = HashMap::new();
    let mut height_map = HashMap::new();
    let mut start = vec![];
    let mut end = (0usize, 0usize);

    let mut height = 0;
    let mut width = 0;

    for (j, row) in input.lines().enumerate() {
        if j > height {
            height = j;
        }
        for (i, cell) in row.bytes().enumerate() {
            if i > width {
                width = i;
            }
            map.insert((i, j), vec![]);
            if cell == b'S' || (include_a && cell == b'a') {
                start.push((i, j));
            }
            if cell == b'E' {
                end.0 = i;
                end.1 = j;
            }
            height_map.insert(
                (i, j),
                if cell == b'S' {
                    b'a'
                } else if cell == b'E' {
                    b'z'
                } else {
                    cell
                },
            );
        }
    }

    for j in 0..=height {
        for i in 0..=width {
            let value = height_map[&(i, j)];
            let connections = map.get_mut(&(i, j)).unwrap();
            if j > 0 && height_map[&(i, j - 1)] <= value + 1 {
                connections.push((i, j - 1));
            }
            if i > 0 && height_map[&(i - 1, j)] <= value + 1 {
                connections.push((i - 1, j));
            }
            if j < height && height_map[&(i, j + 1)] <= value + 1 {
                connections.push((i, j + 1));
            }
            if i < width && height_map[&(i + 1, j)] <= value + 1 {
                connections.push((i + 1, j));
            }
        }
    }

    (map, start, end)
}
