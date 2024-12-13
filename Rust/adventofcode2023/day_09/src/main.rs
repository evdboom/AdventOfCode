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

fn process_part_one(input: &str) -> i64 {
    input
        .lines()
        .map(|line| get_next_in_sequence(line, false))
        .sum()
}

fn process_part_two(input: &str) -> i64 {
    input
        .lines()
        .map(|line| get_next_in_sequence(line, true))
        .sum()
}

fn get_next_in_sequence(line: &str, backwards: bool) -> i64 {
    let mut values = line
        .split_whitespace()
        .map(|num| num.parse::<i64>().unwrap())
        .collect::<Vec<_>>();

    let mut result = if backwards {
        values[0].clone()
    } else {
        values[values.len() - 1].clone()
    };

    let mut even = true;
    while !values.iter().all(|v| v == &0) {
        let mut new_values = Vec::new();
        for i in 0..values.len() - 1 {
            new_values.push(values[i + 1] - values[i]);
        }

        if backwards && even {
            result -= new_values[0];
        } else if backwards {
            result += new_values[0];
        } else {
            result += new_values[new_values.len() - 1];
        }
        even = !even;
        values = new_values;
    }

    result
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45",
        );

        assert_eq!(114, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45",
        );

        assert_eq!(2, process_part_two(&input));
    }
}
