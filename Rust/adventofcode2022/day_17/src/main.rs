use std::collections::HashMap;
use std::fs;
use std::time::Instant;

#[derive(Eq, PartialEq, Hash, Clone)]
struct State {
    rock_type: usize,
    action: usize,
    columns: [u64; 7],
}

struct Rock {
    points: Vec<(usize, usize)>,
}

impl Rock {
    fn dimensions(&self) -> (usize, usize) {
        self.points.iter().fold((0, 0), |(max_x, max_y), (x, y)| {
            (max_x.max(x + 1), max_y.max(y + 1))
        })
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
    let rocks = get_rocks();
    let actions = get_actions(input);
    let mut height = 0;
    let mut state = State {
        columns: [u64::MAX; 7],
        rock_type: 0,
        action: 0,
    };

    for _ in 0..2022 {
        let (new_state, new_height) = drop_rock(state, height, &actions, &rocks);
        state = new_state;
        height = new_height;    
    }

    height
}

fn part_two(input: &String) -> usize {
    let rocks = get_rocks();
    let actions = get_actions(input);
    let mut height = 0;
    let mut to_add: usize = 1000000000000;
    let mut state = State {
        columns: [u64::MAX; 7],
        rock_type: 0,
        action: 0,
    };
    let mut states = HashMap::new();

    while to_add > 0 {
        let previous = states.insert(state.clone(), (to_add, height));
        if previous.is_some() {
            let (old_to_add, old_height) = previous.unwrap();             
            let dif_ticks = old_to_add - to_add;
            let dif_height = height - old_height;
            let added_height = dif_height * (to_add / dif_ticks);
            to_add %= dif_ticks;
            height += added_height;
        }
        let (new_state, new_height) = drop_rock(state, height, &actions, &rocks);
        state = new_state;
        height = new_height;  
        to_add -= 1;
    }

    height
}

fn drop_rock(
    state: State,
    old_height: usize,
    actions: &Vec<isize>,
    rocks: &Vec<Rock>,
) -> (State, usize) {
    let rock = &rocks[state.rock_type];
    let mut left = 2;
    let mut height = old_height + 3;
    let mut action = state.action;
    let (rock_width, rock_height) = rock.dimensions();
    loop {
        let new_left =
            ((state.columns.len() - rock_width) as isize).min(0.max(left + actions[action]));
        if !hit_rock(&state, rock, new_left, old_height, height) {
            left = new_left;
        }
        action = (action + 1) % actions.len();
        if height == 0 || hit_rock(&state, rock, left, old_height, height - 1) {
            let new_height = old_height.max(height + rock_height);
            let new_state = State {
                action,
                rock_type: (state.rock_type + 1) % rocks.len(),
                columns: set_columns(&state, old_height, new_height, height, &rock, left),
            };
            return (new_state, new_height);
        } else {
            height -= 1;
        }
    }
}

fn set_columns(
    state: &State,
    old_height: usize,
    new_height: usize,
    stop_height: usize,
    rock: &Rock,
    left: isize,
) -> [u64; 7] {
    let mut result = state.columns.clone();
    let diff = new_height - old_height;
    if diff > 0 {
        for i in 0..result.len() {
            result[i] = result[i] >> diff;
        }
    }

    for (x, y) in &rock.points {
        let row = 63 - (new_height - stop_height - y - 1);
        result[x + (left as usize)] = result[x + (left as usize)] | (1 << row);
    }

    result
}

fn hit_rock(state: &State, rock: &Rock, left: isize, old_height: usize, height: usize) -> bool {
    if height > old_height {
        return false;
    }
    for (x, y) in &rock.points {
        if height + y  + 1 > old_height {
            continue;
        }
        let row = 63 - (old_height - height - y - 1);
        if state.columns[x + (left as usize)] & (1 << row) != 0 {
            return true;
        }
    }

    false
}

fn get_actions(input: &String) -> Vec<isize> {
    input
        .trim()
        .chars()
        .map(|c| if c == '>' { 1 } else { -1 })
        .collect()
}

fn get_rocks() -> Vec<Rock> {
    vec![
        Rock {
            points: vec![(0, 0), (1, 0), (2, 0), (3, 0)],
        },
        Rock {
            points: vec![(1, 0), (0, 1), (1, 1), (2, 1), (1, 2)],
        },
        Rock {
            points: vec![(0, 0), (1, 0), (2, 0), (2, 1), (2, 2)],
        },
        Rock {
            points: vec![(0, 0), (0, 1), (0, 2), (0, 3)],
        },
        Rock {
            points: vec![(0, 0), (1, 0), (0, 1), (1, 1)],
        },
    ]
}
