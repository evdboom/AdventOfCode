use std::collections::HashMap;
use std::fs;
use std::time::Instant;

const NUMERIC_KEYPAD: [(u8, (isize, isize)); 11] = [
    (b'7', (0, 0)),
    (b'8', (1, 0)),
    (b'9', (2, 0)),
    (b'4', (0, 1)),
    (b'5', (1, 1)),
    (b'6', (2, 1)),
    (b'1', (0, 2)),
    (b'2', (1, 2)),
    (b'3', (2, 2)),
    (b'0', (1, 3)),
    (b'A', (2, 3)),
];

const DIRECTIONAL_KEYPAD: [(u8, (isize, isize)); 5] = [
    (b'^', (1, 0)),
    (b'A', (2, 0)),
    (b'<', (0, 1)),
    (b'v', (1, 1)),
    (b'>', (2, 1)),
];

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
    perform_keypad_presses(input, 2)
}

fn process_part_two(input: &str) -> usize {
    perform_keypad_presses(input, 25)
}

fn perform_keypad_presses(input: &str, chain_length: usize) -> usize {
    let numeric_keypad = NUMERIC_KEYPAD
        .iter()
        .map(|&(key, coords)| (key, coords))
        .collect();
    let directional_keypad = DIRECTIONAL_KEYPAD
        .iter()
        .map(|&(key, coords)| (key, coords))
        .collect();

    let numeric_start: (isize, isize) = (2, 3);
    let directional_start: (isize, isize) = (2, 0);

    let mut cache: HashMap<(Vec<u8>, usize), usize> = HashMap::new();
    let mut result = 0;
    for numeric_code in input.lines() {
        let mut length = 0;
        let code_value = numeric_code[..numeric_code.len() - 1]
            .parse::<usize>()
            .unwrap();
        let codes = get_paths(
            &numeric_code.as_bytes().to_vec(),
            &numeric_keypad,
            &numeric_start,
            3,
        );

        for code in codes.into_iter() {
            length += get_code_length(
                code,
                &directional_keypad,
                &directional_start,
                0,
                chain_length,
                &mut cache,
            );
        }

        result += length * code_value;
    }

    result
}

fn get_code_length(
    code: Vec<u8>,
    keypad: &HashMap<u8, (isize, isize)>,
    start: &(isize, isize),
    gap_row: isize,
    chain_length: usize,
    cache: &mut HashMap<(Vec<u8>, usize), usize>,
) -> usize {
    if chain_length == 0 {
        return code.len();
    } else if let Some(cached) = cache.get(&(code.to_vec(), chain_length)) {
        return *cached;
    }

    let mut length = 0;
    let paths = get_paths(&code, keypad, start, gap_row);
    for path in paths.into_iter() {
        length += get_code_length(path, keypad, start, gap_row, chain_length - 1, cache);
    }
    cache.insert((code, chain_length), length);
    length
}

fn get_paths(
    code: &Vec<u8>,
    keypad: &HashMap<u8, (isize, isize)>,
    start: &(isize, isize),
    gap_row: isize,
) -> Vec<Vec<u8>> {
    let mut paths = Vec::new();
    let mut position = start.clone();
    for c in code {
        let mut path = vec![];
        let (x, y) = keypad.get(&c).unwrap().clone();

        // move left if not on gap row, or if on gap row and not at the edge ('<' per step)
        if (x != 0 || position.1 != gap_row) && x < position.0 {
            for _ in 0..(position.0 - x).abs() {
                path.push(b'<');
            }
            position.0 = x;
        }
        // move up / down ('^' or 'v' per step) if not on gap column (0), or if on gap column and not at the edge
        if position.0 != 0 || y != gap_row {
            for _ in 0..(position.1 - y).abs() {
                path.push(if y < position.1 { b'^' } else { b'v' });
            }
            position.1 = y;
        }
        // move left / right if still needed ('<' or '>' per step)
        for _ in 0..(position.0 - x).abs() {
            path.push(if x < position.0 { b'<' } else { b'>' });
        }
        // move up / down if still needed ('^' or 'v' per step)
        for _ in 0..(position.1 - y).abs() {
            path.push(if y < position.1 { b'^' } else { b'v' });
        }

        path.push(b'A');
        paths.push(path);
        position = (x, y);
    }

    paths
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

        assert_eq!(154115708116294, process_part_two(&input));
    }
}
