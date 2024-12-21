use std::collections::HashMap;
use std::fs;
use std::time::Instant;

static NUMERIC_KEYPAD: [(char, (isize, isize)); 11] = [
    ('7', (0, 0)),
    ('8', (1, 0)),
    ('9', (2, 0)),
    ('4', (0, 1)),
    ('5', (1, 1)),
    ('6', (2, 1)),
    ('1', (0, 2)),
    ('2', (1, 2)),
    ('3', (2, 2)),
    ('0', (1, 3)),
    ('A', (2, 3)),
];

static DIRECTIONAL_KEYPAD: [(char, (isize, isize)); 5] = [
    ('^', (1, 0)),
    ('A', (2, 0)),
    ('<', (0, 1)),
    ('v', (1, 1)),
    ('>', (2, 1)),
];

fn main() {
    let input = fs::read_to_string("./input_test.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = process_part_one(&input);
    let duration_one = start.elapsed();
    let part_two = process_part_two(&input);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn process_part_one(input: &str) -> usize {
    perform_keypad_presses(input, 2)
}

fn process_part_two(input: &str) -> usize {
    // need to figure this one out.. currently creates a very very very long path, and takes forever
    perform_keypad_presses(input, 25)
}

fn perform_keypad_presses(input: &str, chain_length: usize) -> usize {
    let numeric_keypad: HashMap<char, (isize, isize)> = NUMERIC_KEYPAD
        .iter()
        .map(|&(key, coords)| (key, coords))
        .collect();
    let directional_keypad = DIRECTIONAL_KEYPAD
        .iter()
        .map(|&(key, coords)| (key, coords))
        .collect();

    let numeric_start: (isize, isize) = (2, 3);
    let directional_start: (isize, isize) = (2, 0);

    let mut result = 0;
    for numeric_code in input.lines() {
        let code_value = numeric_code[..numeric_code.len() - 1]
            .parse::<usize>()
            .unwrap();
        let mut code = get_path(&numeric_code, &numeric_keypad, &numeric_start, 3);
        for i in 0..chain_length {
            println!("Step: {}", i);
            code = get_path(&code, &directional_keypad, &directional_start, 0);
        }

        result += code.len() * code_value;
    }

    result
}

fn get_path(
    code: &str,
    keypad: &HashMap<char, (isize, isize)>,
    start: &(isize, isize),
    gap_row: isize,
) -> String {
    let mut path = String::new();
    let mut position = start.clone();
    for c in code.chars() {
        let (x, y) = keypad.get(&c).unwrap().clone();

        // move left if not on gap row, or if on gap row and not at the edge ('<' per step)
        if (x != 0 || position.1 != gap_row) && x < position.0 {
            for _ in 0..(position.0 - x).abs() {
                path.push('<');
            }
            position.0 = x;
        }
        // move up / down ('^' or 'v' per step) if not on gap column (0), or if on gap column and not at the edge
        if position.0 != 0 || y != gap_row {
            for _ in 0..(position.1 - y).abs() {
                path.push(if y < position.1 { '^' } else { 'v' });
            }
            position.1 = y;
        }
        // move left / right if still needed ('<' or '>' per step)
        for _ in 0..(position.0 - x).abs() {
            path.push(if x < position.0 { '<' } else { '>' });
        }
        // move up / down if still needed ('^' or 'v' per step)
        for _ in 0..(position.1 - y).abs() {
            path.push(if y < position.1 { '^' } else { 'v' });
        }

        path.push('A');

        position = (x, y);
    }

    path
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "029A
980A
179A
456A
379A",
        );

        assert_eq!(126384, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "029A
980A
179A
456A
379A",
        );

        assert_eq!(117440, process_part_one(&input));
    }
}
