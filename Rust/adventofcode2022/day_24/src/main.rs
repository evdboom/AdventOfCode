use std::cmp::Ordering;
use std::collections::{BinaryHeap, BTreeMap, HashSet};
use std::{fs, vec};
use std::time::Instant;

struct Blizzard {
    x: isize,
    y: isize,
    direction: isize
}

#[derive(PartialEq, Eq)]
struct State {
    distance: usize,
    step: usize,
    x: isize,
    y: isize,
}

impl PartialOrd for State {
    fn partial_cmp(&self, other: &Self) -> Option<std::cmp::Ordering> {
        Some(self.cmp(other))
    }
}

impl Ord for State {
    fn cmp(&self, other: &Self) -> std::cmp::Ordering {
        let dis_cmp = other.distance.cmp(&self.distance);
        if dis_cmp == Ordering::Equal {
            other.step.cmp(&self.step)
        } else {
            dis_cmp
        }
    }
}

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

fn part_one (input: &String) -> usize {
    let blizzards = get_blizzards(input);

    let start = (input.lines().next().unwrap().chars().position(|char| char == '.').unwrap() - 1) as isize;
    let end = (input.lines().last().unwrap().chars().position(|char| char == '.').unwrap() - 1) as isize;
    let width = (input.lines().next().unwrap().chars().count() - 2) as isize;
    let height = (input.lines().count() - 2) as isize;

    let mut blizzard_cache = BTreeMap::new();

    get_min_steps((start, -1), (end, height), &blizzards, &mut blizzard_cache, width, height, 0)
}

fn part_two (input: &String) -> usize {
    let blizzards = get_blizzards(input);

    let start = (input.lines().next().unwrap().chars().position(|char| char == '.').unwrap() - 1) as isize;
    let end = (input.lines().last().unwrap().chars().position(|char| char == '.').unwrap() - 1) as isize;
    let width = (input.lines().next().unwrap().chars().count() - 2) as isize;
    let height = (input.lines().count() - 2) as isize;

    let mut blizzard_cache = BTreeMap::new();

    let steps_to = get_min_steps((start, -1), (end, height), &blizzards, &mut blizzard_cache, width, height, 0);
    let steps_from = get_min_steps((end, height), (start, -1), &blizzards, &mut blizzard_cache, width, height, steps_to);
    get_min_steps((start, -1), (end, height), &blizzards, &mut blizzard_cache, width, height, steps_from)
}

fn get_min_steps(start: (isize, isize), finish: (isize, isize), blizzards: &Vec<Blizzard>, blizzard_cache: &mut BTreeMap<usize, HashSet<(isize, isize)>>, width: isize, height: isize, start_step: usize) -> usize {
    let mut queue = BinaryHeap::new();
    let mut cache = HashSet::new();

    cache.insert((start.0, start.1, start_step));
    queue.push(State { x: start.0, y: start.1, step: start_step, distance: start.0.abs_diff(finish.0) + start.1.abs_diff(finish.1) });

    let mut min_steps = usize::MAX;
    while let Some(state) = queue.pop() {
        if state.step + state.distance >= min_steps {
            continue;
        }

        let step = state.step + 1;

        if !blizzard_cache.contains_key(&step) {
            blizzard_cache.insert(step, blizzards_at_step(blizzards, step, width, height));
        }

        for (x, y) in get_moves(state.x, state.y, finish, &blizzard_cache[&step], width, height) {
            if (x, y) == finish {
                if step < min_steps {
                    min_steps = step;                 
                }
                continue;
            }
            if cache.insert((x, y, step)) {
                let distance = x.abs_diff(finish.0) + y.abs_diff(finish.1);
                queue.push(State { x, y, distance, step });
            }            
        }

    }

    min_steps
}

fn get_moves(x: isize, y: isize, finish: (isize, isize), blizzards: &HashSet<(isize, isize)>, width: isize, height: isize) -> Vec<(isize, isize)> {
    let mut result = vec![];

    let down = (x, y + 1);
    if down == finish {
        return vec![down];
    }
    if down.1 < height && !blizzards.contains(&down) {
        result.push(down);
    }
    let up = (x, y - 1);
    if up == finish {
        return vec![up];
    }
    if up.1 >= 0 && !blizzards.contains(&up) {
        result.push(up);
    }
    let stay = (x, y);
    if !blizzards.contains(&stay) {
        result.push(stay);
    }
    if y >= 0 && y < height {
        let right = (x + 1, y);
        if right.0 < width  && !blizzards.contains(&right) {
            result.push(right);
        }
        let left = (x - 1, y);
        if left.0 >= 0 && !blizzards.contains(&left) {
            result.push(left);
        }
    }

    result
}

fn blizzard_at_step(blizzard: &Blizzard, step: isize, width: isize, height: isize) -> (isize, isize) {
    let mut point = match blizzard.direction {
        0 => ((blizzard.x + step) % width, blizzard.y),
        1 => (blizzard.x, (blizzard.y + step) % height),
        2 => ((blizzard.x - step) % width, blizzard.y),
        3 => (blizzard.x, (blizzard.y - step) % height),
        _ => panic!("Not a valid direction")
    };

    if point.0 < 0 {
        point.0 += width;
    }
    if point.1 < 0 {
        point.1 += height;
    }

    point
}

fn blizzards_at_step(blizzards: &Vec<Blizzard>, step: usize, width: isize, height: isize) -> HashSet<(isize, isize)> {
    let mut result = HashSet::new();

    for blizzard in blizzards {
        result.insert(blizzard_at_step(blizzard, step as isize, width, height));
    }

    result
}

fn get_blizzards(input: &String) -> Vec<Blizzard> {
    let mut result = vec![];

    for (row, line) in input.lines().enumerate() {
        for (column, char) in line.chars().enumerate() {
            match  char {
                '>' => result.push(Blizzard { x: column as isize - 1, y: row as isize - 1, direction: 0 }),
                'v' => result.push(Blizzard { x: column as isize - 1, y: row as isize - 1, direction: 1 }),
                '<' => result.push(Blizzard { x: column as isize - 1, y: row as isize - 1, direction: 2 }),
                '^' => result.push(Blizzard { x: column as isize - 1, y: row as isize - 1, direction: 3 }),
                _ => ()
            }
        }
    }

    result
}