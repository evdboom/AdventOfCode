use itertools::Itertools;
use std::cmp::Ordering;
use std::collections::{BinaryHeap, HashMap, HashSet};
use std::fs;
use std::time::Instant;

#[derive(Eq)]
struct State {
    time_remaining: usize,
    flow: usize,
    closed_valves: Vec<u16>,
    actors: Vec<Actor>,
    potential: usize,
}

impl Ord for State {
    fn cmp(&self, other: &Self) -> Ordering {
        let pot = self.potential.cmp(&other.potential);
        if pot == Ordering::Equal {
            self.flow.cmp(&other.flow)
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

impl PartialEq for State {
    fn eq(&self, other: &Self) -> bool {
        self.potential == other.potential
    }
}

#[derive(PartialEq, Eq)]
struct Actor {
    location: u16,
    eta: usize,
}

#[derive(PartialEq, Eq)]
struct Valve {
    flow: usize,
    connections: Vec<u16>,
}

impl Ord for Valve {
    fn cmp(&self, other: &Self) -> Ordering {
        self.flow.cmp(&other.flow)
    }
}

impl PartialOrd for Valve {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

#[derive(PartialEq, Eq)]
struct Node {
    distance: usize,
    id: u16,
}

// reverse order, lower distance is higher priority
impl Ord for Node {
    fn cmp(&self, other: &Self) -> Ordering {
        other.distance.cmp(&self.distance)
    }
}

impl PartialOrd for Node {
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
    let valves = get_valves(input);
    let start = to_id("AA");
    let (distances, closed_valves) = get_distances(&valves, &start);

    let initial_state = State {
        flow: 0,
        time_remaining: 30,
        potential: 0,
        closed_valves,
        actors: vec![Actor {
            eta: 30,
            location: start,
        }],
    };

    get_maximum_flow(initial_state, &distances, &valves)
}

fn part_two(input: &String) -> usize {
    let valves = get_valves(input);
    let start = to_id("AA");
    let (distances, closed_valves) = get_distances(&valves, &start);

    let initial_state = State {
        flow: 0,
        time_remaining: 26,
        potential: 0,
        closed_valves,
        actors: vec![
            Actor {
                eta: 26,
                location: start,
            },
            Actor {
                eta: 26,
                location: start,
            },
        ],
    };

    get_maximum_flow(initial_state, &distances, &valves)
}

fn get_maximum_flow(
    initial_state: State,
    distances: &HashMap<(u16, u16), usize>,
    valves: &HashMap<u16, Valve>,
) -> usize {
    let mut queue = BinaryHeap::new();

    queue.push(initial_state);
    let mut max = 0;
    while let Some(state) = queue.pop() {
        if state.time_remaining == 0 || state.closed_valves.is_empty() {
            max = state.flow;
            break;
        }

        let finished: Vec<&Actor> = state
            .actors
            .iter()
            .filter(|actor| actor.eta == state.time_remaining)
            .collect();
        if finished.len() == 1 {
            for (index, closed) in state.closed_valves.iter().enumerate() {
                let distance = distances[&(finished[0].location, *closed)];
                let eta = if state.time_remaining > distance + 1 {
                    state.time_remaining - distance - 1
                } else {
                    0
                };
                let time_remaining = state.actors.iter().fold(eta, |new, actor| {
                    if actor.eta != state.time_remaining && actor.eta > new {
                        actor.eta
                    } else {
                        new
                    }
                });
                let flow = state.flow + eta * valves[closed].flow;
                let mut closed_valves = state.closed_valves.clone();
                closed_valves.remove(index);
                let potential = get_potential(
                    flow,
                    time_remaining,
                    &closed_valves,
                    valves,
                    state.actors.len(),
                );
                let actors = state
                    .actors
                    .iter()
                    .map(|actor| {
                        if actor.eta == state.time_remaining {
                            Actor {
                                eta: eta,
                                location: *closed,
                            }
                        } else {
                            Actor { ..*actor }
                        }
                    })
                    .collect();

                let new_state = State {
                    time_remaining,
                    potential,
                    flow,
                    closed_valves,
                    actors,
                };

                queue.push(new_state);
            }
        } else if state.closed_valves.len() == 1 {
            let closed = state.closed_valves[0];
            let distance = state.actors.iter().fold(usize::MAX, |c, actor| {
                distances[&(actor.location, closed)].min(c)
            });
            let eta = if state.time_remaining > distance + 1 {
                state.time_remaining - distance - 1
            } else {
                0
            };
            let time_remaining = state.actors.iter().fold(eta, |new, actor| {
                if actor.eta != state.time_remaining && actor.eta > new {
                    actor.eta
                } else {
                    new
                }
            });
            let flow = state.flow + eta * valves[&closed].flow;
            let potential = flow;
            let actors = state
                .actors
                .iter()
                .map(|actor| {
                    if actor.eta == state.time_remaining {
                        Actor {
                            eta: eta,
                            location: closed,
                        }
                    } else {
                        Actor {
                            eta: actor.eta,
                            location: actor.location,
                        }
                    }
                })
                .collect();

            let new_state = State {
                time_remaining,
                potential,
                flow,
                closed_valves: vec![],
                actors,
            };
            queue.push(new_state);
        } else {
            for (closed_1, closed_2) in state
                .closed_valves
                .iter()
                .tuple_combinations::<(&u16, &u16)>()
            {
                let distance_1 = distances[&(finished[0].location, *closed_1)];
                let distance_2 = distances[&(finished[1].location, *closed_2)];

                let eta_1 = if state.time_remaining > distance_1 + 1 {
                    state.time_remaining - distance_1 - 1
                } else {
                    0
                };
                let eta_2 = if state.time_remaining > distance_2 + 1 {
                    state.time_remaining - distance_2 - 1
                } else {
                    0
                };

                let time_remaining = eta_1.max(eta_2);
                let flow =
                    state.flow + eta_1 * valves[closed_1].flow + eta_2 * valves[closed_2].flow;
                let mut closed_valves = state.closed_valves.clone();
                let index_1 = closed_valves.iter().position(|v| v == closed_1).unwrap();
                let index_2 = closed_valves.iter().position(|v| v == closed_2).unwrap();
                closed_valves.remove(index_1.max(index_2));
                closed_valves.remove(index_1.min(index_1));
                let potential = get_potential(
                    flow,
                    time_remaining,
                    &closed_valves,
                    valves,
                    state.actors.len(),
                );
                let actors = vec![
                    Actor {
                        eta: eta_1,
                        location: *closed_1,
                    },
                    Actor {
                        eta: eta_2,
                        location: *closed_2,
                    },
                ];

                let new_state = State {
                    time_remaining,
                    potential,
                    flow,
                    closed_valves,
                    actors,
                };
                queue.push(new_state);
            }
        }
    }
    max
}

fn get_potential(
    flow: usize,
    time_remaining: usize,
    closed_valves: &Vec<u16>,
    valves: &HashMap<u16, Valve>,
    no_actors: usize,
) -> usize {
    if time_remaining <= 2 {
        return flow;
    }

    let mut closed: Vec<&Valve> = closed_valves.iter().map(|v| &valves[v]).collect();
    closed.sort_unstable();

    let mut potential = flow;
    let mut time = time_remaining;
    while !closed.is_empty() && time > 2 {
        time -= 2;
        for _ in 0..no_actors {
            let location = closed.pop();
            if location.is_some() {
                potential += location.unwrap().flow * time;
            } else {
                break;
            }
        }
    }
    potential
}

fn get_distances(
    valves: &HashMap<u16, Valve>,
    start: &u16,
) -> (HashMap<(u16, u16), usize>, Vec<u16>) {
    let mut result = HashMap::new();
    let mut to_open = vec![];
    let with_flow: Vec<_> = valves
        .iter()
        .filter(|(id, valve)| id == &start || valve.flow > 0)
        .collect();
    let to_add = with_flow.len() - 1;
    for (id, _) in with_flow {
        if id != start {
            to_open.push(*id);
        }

        let mut queue = BinaryHeap::new();
        let mut visited = HashSet::new();

        queue.push(Node {
            id: *id,
            distance: 0,
        });
        visited.insert(*id);
        let mut added = 0;
        while let Some(node) = queue.pop() {
            let valve = &valves[&node.id];

            for connection in valve.connections.iter() {
                if visited.insert(*connection) {
                    queue.push(Node {
                        id: *connection,
                        distance: node.distance + 1,
                    });
                    let connection_valve = &valves[connection];
                    if connection_valve.flow > 0 {
                        result.insert((*id, *connection), node.distance + 1);
                        added += 1;
                    }
                }
            }

            if (id != start && added == to_add) || added == to_add + 1 {
                break;
            }
        }
    }

    (result, to_open)
}

fn get_valves(input: &String) -> HashMap<u16, Valve> {
    let mut result = HashMap::new();
    for line in input.lines() {
        let (valve, connections) = line.split_once("; ").unwrap();
        let (name, flow) = valve.split_once(" has flow rate=").unwrap();
        let id = to_id(name.strip_prefix("Valve ").unwrap());
        let flow = flow.parse().unwrap();
        let connections = connections
            .strip_prefix("tunnels lead to valves ")
            .or_else(|| connections.strip_prefix("tunnel leads to valve "))
            .unwrap()
            .split(", ")
            .map(|c| to_id(c))
            .collect();
        result.insert(id, Valve { flow, connections });
    }

    result
}

fn to_id(value: &str) -> u16 {
    let bytes = value.as_bytes();
    ((bytes[0] as u16) << 8) | bytes[1] as u16
}
