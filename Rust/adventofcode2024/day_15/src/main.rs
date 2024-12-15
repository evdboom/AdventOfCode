use std::collections::HashMap;
use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = process_part_one(&input);
    let duration_one = start.elapsed();
    let part_two = process_part_two(&input);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn process_part_one(input: &str) -> usize {
    let (mut map, instructions) = get_map(input, false);

    let mut position = map.iter().find(|(_, &v)| v == '@').unwrap().0.clone();
    for c in instructions.chars() {
        if let Some(new_position) = move_object(&mut map, &position, &c, &'@') {
            position = new_position;
        }
    }

    map.iter()
        .filter(|(_, &c)| c == 'O')
        .fold(0, |acc, ((x, y), _)| acc + x + y * 100)
}

fn process_part_two(input: &str) -> usize {
    let (mut map, instructions) = get_map(input, true);

    let mut position = map.iter().find(|(_, &v)| v == '@').unwrap().0.clone();
    for c in instructions.chars() {
        if let Some(new_position) = move_object(&mut map, &position, &c, &'@') {
            position = new_position;
        }
    }

    // for j in 0..=map.iter().max_by_key(|((_, y), _)| y).unwrap().0 .1 {
    //     for i in 0..=map.iter().max_by_key(|((x, _), _)| x).unwrap().0 .0 {
    //         print!("{}", map.get(&(i, j)).unwrap());
    //     }
    //     println!();
    // }

    map.iter()
        .filter(|(_, &c)| c == '[')
        .fold(0, |acc, ((x, y), _)| acc + x + y * 100)
}

// added for part two, first check if the move can happen as multiple objects can be moved
fn can_move_object(
    map: &HashMap<(usize, usize), char>,
    position: &(usize, usize),
    direction: &char,
) -> bool {
    let new_position = match direction {
        '^' => (position.0, position.1 - 1),
        'v' => (position.0, position.1 + 1),
        '<' => (position.0 - 1, position.1),
        '>' => (position.0 + 1, position.1),
        _ => panic!("Invalid direction"),
    };

    let value = map.get(&new_position).unwrap();
    match direction {
        '<' | '>' => match value {
            '.' => true,
            'O' | '[' | ']' => can_move_object(map, &new_position, direction),
            _ => false,
        },
        '^' | 'v' => match value {
            '.' => true,
            'O' => can_move_object(map, &new_position, direction),
            '[' => {
                can_move_object(map, &new_position, direction)
                    && can_move_object(map, &(new_position.0 + 1, new_position.1), direction)
            }
            ']' => {
                can_move_object(map, &new_position, direction)
                    && can_move_object(map, &(new_position.0 - 1, new_position.1), direction)
            }

            _ => false,
        },
        _ => false,
    }
}

fn move_object(
    map: &mut HashMap<(usize, usize), char>,
    position: &(usize, usize),
    direction: &char,
    moved: &char,
) -> Option<(usize, usize)> {
    if !can_move_object(map, position, direction) {
        return None;
    }

    let current = map.get(position).unwrap();
    if current != moved {
        return None;
    }

    let new_position = match direction {
        '^' => (position.0, position.1 - 1),
        'v' => (position.0, position.1 + 1),
        '<' => (position.0 - 1, position.1),
        '>' => (position.0 + 1, position.1),
        _ => panic!("Invalid direction"),
    };

    let value = map.get(&new_position).unwrap();
    match value {
        '.' => {
            map.insert(new_position, moved.clone());
            map.insert(position.clone(), '.');
        }
        'O' => {
            let value_clone = value.clone();
            move_object(map, &new_position, direction, &value_clone);
            map.insert(new_position, moved.clone());
            map.insert(position.clone(), '.');
        }
        '[' => {
            let value_clone = value.clone();
            move_object(map, &new_position, direction, &value_clone);
            move_object(map, &(new_position.0 + 1, new_position.1), direction, &']');
            map.insert(new_position, moved.clone());
            map.insert(position.clone(), '.');
        }
        ']' => {
            let value_clone = value.clone();
            move_object(map, &new_position, direction, &value_clone);
            move_object(map, &(new_position.0 - 1, new_position.1), direction, &'[');
            map.insert(new_position, moved.clone());
            map.insert(position.clone(), '.');
        }
        _ => panic!("Invalid value"),
    }

    Some(new_position)
}

fn get_map(input: &str, enlarged: bool) -> (HashMap<(usize, usize), char>, String) {
    let mut map: HashMap<(usize, usize), char> = HashMap::new();
    let mut instructions = String::new();
    let mut instructions_started = false;
    for (j, line) in input.lines().enumerate() {
        if !instructions_started {
            if line.is_empty() {
                instructions_started = true;
                continue;
            }
            for (i, c) in line.chars().enumerate() {
                if !enlarged {
                    map.insert((i, j), c);
                } else {
                    match c {
                        '#' | '.' => {
                            map.insert((i * 2, j), c);
                            map.insert((i * 2 + 1, j), c);
                        }
                        '@' => {
                            map.insert((i * 2, j), c);
                            map.insert((i * 2 + 1, j), '.');
                        }
                        _ => {
                            map.insert((i * 2, j), '[');
                            map.insert((i * 2 + 1, j), ']');
                        }
                    }
                }
            }
        } else {
            instructions.push_str(line);
        }
    }

    (map, instructions)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one_small() {
        let input = String::from(
            "########
#..O.O.#
##@.O..#
#...O..#
#.#.O..#
#...O..#
#......#
########

<^^>>>vv<v>>v<<",
        );

        assert_eq!(2028, process_part_one(&input));
    }

    #[test]
    fn part_one_big() {
        let input = String::from(
            "##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^",
        );

        assert_eq!(10092, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^",
        );

        assert_eq!(9021, process_part_two(&input));
    }
}
