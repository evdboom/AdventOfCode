use std::collections::HashSet;
use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = get_part_numbers(&input);
    let duration_one = start.elapsed();
    let part_two = get_gear_ratio(&input);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn get_part_numbers(input: &String) -> u32 {
    let values = get_numbers_and_symbols(input);
    values
        .0
        .into_iter()
        .filter(|number| has_any_symbol_match(number, &values.1))
        .map(|number| number.3)
        .sum()
}

fn get_gear_ratio(input: &String) -> u32 {
    let values = get_numbers_and_symbols(input);
    values
        .1
        .into_iter()
        .filter(|symbol| symbol.2 == '*')
        .map(|symbol| {
            values
                .0
                .clone()
                .into_iter()
                .filter(|number| has_symbol_match(number, &symbol))
                .collect::<HashSet<(i32, i32, i32, u32)>>()
        })
        .filter(|numbers| numbers.len() == 2)
        .map(|numbers| {
            let mut iter = numbers.into_iter();
            iter.next().unwrap().3 * iter.next().unwrap().3
        })
        .sum()
}

fn has_any_symbol_match(
    number: &(i32, i32, i32, u32),
    symbols: &HashSet<(i32, i32, char)>,
) -> bool {
    for symbol in symbols.into_iter() {
        if has_symbol_match(number, symbol) {
            return true;
        }
    }

    false
}

fn has_symbol_match(number: &(i32, i32, i32, u32), symbol: &(i32, i32, char)) -> bool {
    symbol.0 >= number.0 - 1
        && symbol.0 <= number.0 + 1
        && symbol.1 >= number.1 - 1
        && symbol.1 <= number.2 + 1
}

fn get_numbers_and_symbols(
    input: &String,
) -> (HashSet<(i32, i32, i32, u32)>, HashSet<(i32, i32, char)>) {
    let mut numbers = HashSet::<(i32, i32, i32, u32)>::new();
    let mut symbols = HashSet::<(i32, i32, char)>::new();
    let mut current_line = 0;
    for line in input.lines() {
        let mut current_value = String::from("");
        let mut current_character: i32 = 0;
        for ch in line.chars() {
            if ch.is_numeric() {
                current_value.push(ch);
            } else {
                if !current_value.is_empty() {
                    numbers.insert((
                        current_line,
                        current_character - (current_value.len() as i32),
                        current_character - 1,
                        current_value.parse::<u32>().unwrap(),
                    ));
                    current_value.clear();
                }

                if ch != '.' {
                    symbols.insert((current_line, current_character, ch));
                }
            }

            current_character += 1;
        }
        if !current_value.is_empty() {
            numbers.insert((
                current_line,
                current_character - (current_value.len() as i32),
                current_character - 1,
                current_value.parse::<u32>().unwrap(),
            ));
        }
        current_line += 1;
    }

    (numbers, symbols)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..",
        );

        assert_eq!(4361, get_part_numbers(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..",
        );

        assert_eq!(467835, get_gear_ratio(&input));
    }
}
