use std::cmp::Ordering;
use std::collections::{BinaryHeap, HashSet};
use std::fs;
use std::time::Instant;

struct Blueprint {
    number: usize,
    ore_robot: usize,
    clay_robot: usize,
    obsidian_robot: (usize, usize),
    geode_robot: (usize, usize),
}

#[derive(PartialEq, Eq, Hash, Clone)]
struct State {
    time_remaining: usize,
    ore: usize,
    ore_robots: usize,
    clay: usize,
    clay_robots: usize,
    obsidian: usize,
    obsidian_robots: usize,
    geodes: usize,
    geode_robots: usize,
    potential: usize
}

impl Ord for State {
    fn cmp(&self, other: &Self) -> Ordering {
        let pot = self.potential.cmp(&other.potential);
        if pot == Ordering::Equal {
            self.time_remaining.cmp(&other.time_remaining)
        } else {
            pot
        }
    }
}

impl PartialOrd for State {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        Some(self.cmp(other))
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

fn part_one(input: &String) -> usize {
    let blueprints = get_blueprints(input);
    blueprints.into_iter().fold(0, |sum, blueprint| {
        println!("{}", sum);
        sum + blueprint.number * get_geodes(blueprint, 24)
    })
}

fn part_two(input: &String) -> usize {
    0
}

fn get_geodes(blueprint: Blueprint, start_time: usize) -> usize {
    let initial_state = State {
        ore: 0,
        ore_robots: 1,
        clay: 0,
        clay_robots: 0,
        obsidian: 0,
        obsidian_robots: 0,
        geodes: 0,
        geode_robots: 0,
        time_remaining: start_time,
        potential: 0
    };
    let mut cache = HashSet::new();
    let mut queue = BinaryHeap::new();
    queue.push(initial_state);
    let mut max = 0;

    while let Some(state) = queue.pop() {
        if state.time_remaining == 0 {
            max = state.geodes;
            break;
        }
        let potential = get_potential(&state, &blueprint);
        if potential == 0 {
            continue;
        }

        if state.ore >= blueprint.geode_robot.0 && state.obsidian >= blueprint.geode_robot.1 {
            let new_state = State {
                ore: state.ore + state.ore_robots - blueprint.geode_robot.0,
                ore_robots: state.ore_robots,
                clay: state.clay + state.clay_robots,
                clay_robots: state.clay_robots,
                obsidian: state.obsidian + state.obsidian_robots - blueprint.geode_robot.1,
                obsidian_robots: state.obsidian_robots,
                geodes: state.geodes + state.geode_robots,
                geode_robots: state.geode_robots + 1,
                time_remaining: state.time_remaining - 1,
                potential
            };
            if cache.insert(new_state.clone()) {
                queue.push(new_state);
            }
        } else {
            if state.ore >= blueprint.obsidian_robot.0 && state.clay >= blueprint.obsidian_robot.1 {
                let new_state = State {
                    ore: state.ore + state.ore_robots - blueprint.obsidian_robot.0,
                    ore_robots: state.ore_robots,
                    clay: state.clay + state.clay_robots - blueprint.obsidian_robot.1,
                    clay_robots: state.clay_robots,
                    obsidian: state.obsidian + state.obsidian_robots,
                    obsidian_robots: state.obsidian_robots + 1,
                    geodes: state.geodes + state.geode_robots,
                    geode_robots: state.geode_robots,
                    time_remaining: state.time_remaining - 1,
                    potential
                };
                if cache.insert(new_state.clone()) {
                    queue.push(new_state);
                }
            }
            if state.ore >= blueprint.clay_robot {
                let new_state = State {
                    ore: state.ore + state.ore_robots - blueprint.clay_robot,
                    ore_robots: state.ore_robots,
                    clay: state.clay + state.clay_robots,
                    clay_robots: state.clay_robots + 1,
                    obsidian: state.obsidian + state.obsidian_robots,
                    obsidian_robots: state.obsidian_robots,
                    geodes: state.geodes + state.geode_robots,
                    geode_robots: state.geode_robots,
                    time_remaining: state.time_remaining - 1,
                    potential
                };
                if cache.insert(new_state.clone()) {
                    queue.push(new_state);
                }
            }
            if state.ore >= blueprint.ore_robot {
                let new_state = State {
                    ore: state.ore + state.ore_robots - blueprint.ore_robot,
                    ore_robots: state.ore_robots + 1,
                    clay: state.clay + state.clay_robots,
                    clay_robots: state.clay_robots,
                    obsidian: state.obsidian + state.obsidian_robots,
                    obsidian_robots: state.obsidian_robots,
                    geodes: state.geodes + state.geode_robots,
                    geode_robots: state.geode_robots,
                    time_remaining: state.time_remaining - 1,
                    potential
                };
                if cache.insert(new_state.clone()) {
                    queue.push(new_state);
                }
            }
            let new_state = State {
                ore: state.ore + state.ore_robots,
                ore_robots: state.ore_robots,
                clay: state.clay + state.clay_robots,
                clay_robots: state.clay_robots,
                obsidian: state.obsidian + state.obsidian_robots,
                obsidian_robots: state.obsidian_robots,
                geodes: state.geodes + state.geode_robots,
                geode_robots: state.geode_robots,
                time_remaining: state.time_remaining - 1,
                potential
            };
            if cache.insert(new_state.clone()) {
                queue.push(new_state);
            }
        }
    }

    max
}

fn get_potential(state: &State, blueprint: &Blueprint) -> usize {
    let mut potential_state = state.clone();
    for _ in 0..state.time_remaining {
        let added_ore = potential_state.ore_robots;
        let added_clay = potential_state.clay_robots;
        let added_obsidian = potential_state.obsidian_robots;
        let added_geodes = potential_state.geode_robots;

        potential_state.ore_robots += 1;
        if potential_state.ore >= blueprint.clay_robot {
            potential_state.ore -= blueprint.clay_robot;
            potential_state.clay_robots += 1;
        }
        if potential_state.clay >= blueprint.obsidian_robot.1 {
            potential_state.clay -= blueprint.obsidian_robot.1;
            potential_state.obsidian_robots += 1;
        }
        if potential_state.obsidian >= blueprint.geode_robot.1 {
            potential_state.obsidian -= blueprint.geode_robot.1;
            potential_state.geode_robots += 1;
        }
        potential_state.ore += added_ore;
        potential_state.clay += added_clay;
        potential_state.obsidian += added_obsidian;
        potential_state.geodes += added_geodes;
    }
    potential_state.geodes
}

fn get_blueprints(input: &String) -> Vec<Blueprint> {
    input
        .lines()
        .map(|line| {
            let (number, bots) = line.split_once(": ").unwrap();
            let number = number.strip_prefix("Blueprint ").unwrap().parse().unwrap();
            let mut bots = bots.split(". Each ");
            let ore_robot = bots
                .next()
                .unwrap()
                .strip_prefix("Each ore robot costs ")
                .and_then(|b| b.strip_suffix(" ore"))
                .unwrap()
                .parse()
                .unwrap();
            let clay_robot = bots
                .next()
                .unwrap()
                .strip_prefix("clay robot costs ")
                .and_then(|b| b.strip_suffix(" ore"))
                .unwrap()
                .parse()
                .unwrap();
            let obsidian_robot = bots
                .next()
                .unwrap()
                .strip_prefix("obsidian robot costs ")
                .and_then(|b| b.strip_suffix(" clay"))
                .unwrap()
                .split(" ore and ")
                .fold((0, 0), |(ore, clay), part| {
                    if ore == 0 {
                        (part.parse().unwrap(), clay)
                    } else {
                        (ore, part.parse().unwrap())
                    }
                });
            let geode_robot = bots
                .next()
                .unwrap()
                .strip_prefix("geode robot costs ")
                .and_then(|b| b.strip_suffix(" obsidian."))
                .unwrap()
                .split(" ore and ")
                .fold((0, 0), |(ore, obsidian), part| {
                    if ore == 0 {
                        (part.parse().unwrap(), obsidian)
                    } else {
                        (ore, part.parse().unwrap())
                    }
                });
            Blueprint {
                number,
                ore_robot,
                clay_robot,
                obsidian_robot,
                geode_robot,
            }
        })
        .collect()
}
