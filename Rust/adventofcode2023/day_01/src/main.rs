use std::collections::HashMap;
use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = get_numeric_values(&input);
    let duration_one = start.elapsed();
    let part_two = get_numeric_and_written_value(&input);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn get_numeric_values(input: &String) -> u32 {
    input
        .lines()
        .map(|line| {
            let mut first = '0';
            let mut first_set = false;
            let mut last = '0';
            for c in line.chars() {
                if c.is_numeric() {
                    if !first_set {
                        first = c;
                        first_set = true;
                    }
                    last = c;
                }
            }

            format!("{}{}", first, last)
                .parse::<u32>()
                .expect("could not parse")
        })
        .sum()
}

fn get_numeric_and_written_value(input: &String) -> u32 {
    let replacements = HashMap::from([
        ("one", '1'),
        ("two", '2'),
        ("three", '3'),
        ("four", '4'),
        ("five", '5'),
        ("six", '6'),
        ("seven", '7'),
        ("eight", '8'),
        ("nine", '9'),
    ]);

    input
        .lines()
        .map(|line| {
            let mut first = '0';
            let mut first_set = false;
            let mut last = '0';
            let mut mapped = String::from(line);
            while mapped.len() > 0 {
                let ch = mapped.chars().next().unwrap();
                if ch.is_numeric() {
                    if !first_set {
                        first = ch;
                        first_set = true;
                    }
                    last = ch;
                } else {
                    for (key, value) in &replacements {
                        if mapped.starts_with(key) {
                            if !first_set {
                                first = *value;
                                first_set = true;
                            }
                            last = *value;
                        }
                    }
                }

                mapped.remove(0);
            }

            format!("{}{}", first, last)
                .parse::<u32>()
                .expect("could not parse")
        })
        .sum()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet",
        );
        assert_eq!(142, get_numeric_values(&input))
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen",
        );

        assert_eq!(281, get_numeric_and_written_value(&input));
    }
}
