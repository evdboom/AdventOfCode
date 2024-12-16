use std::collections::HashMap;
use std::fs;
use std::iter::repeat;
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
    input.lines().fold(0, |acc, line| {
        let mut parts = line.split_whitespace();
        let springs = parts.next().unwrap();
        let broken = parts
            .next()
            .unwrap()
            .split(',')
            .map(|s| s.parse::<usize>().unwrap())
            .collect::<Vec<_>>();

        acc + get_possible_arrangements(springs, broken)
    })
}

fn process_part_two(input: &str) -> usize {
    input.lines().fold(0, |acc, line| {
        let mut parts = line.split_whitespace();
        let springs = repeat(parts.next().unwrap())
            .take(5)
            .collect::<Vec<&str>>()
            .join("?");
        let broken = repeat(parts.next().unwrap())
            .take(5)
            .collect::<Vec<&str>>()
            .join(",")
            .split(',')
            .map(|s| s.parse::<usize>().unwrap())
            .collect::<Vec<_>>();

        acc + get_possible_arrangements(&springs, broken)
    })
}

fn get_possible_arrangements(springs: &str, broken_list: Vec<usize>) -> usize {
    let mut arrangements = vec![(0, -1isize, 1)];

    for &broken in &broken_list {
        let mut new_arrangements = HashMap::new();

        for &(start_index, size, count) in &arrangements {
            let valid_indexes =
                get_valid_indexes(springs, (start_index + size + 1) as usize, broken as usize);

            for index in valid_indexes {
                *new_arrangements.entry(index).or_insert(0) += count;
            }
        }

        arrangements = new_arrangements
            .into_iter()
            .map(|(key, count)| (key as isize, broken as isize, count))
            .collect();
    }

    arrangements
        .into_iter()
        .map(|(start_index, size, count)| (&springs[((start_index + size) as usize)..], count))
        .filter(|(remainder, _)| !remainder.contains('#'))
        .map(|(_, count)| count)
        .sum()
}

fn get_valid_indexes(springs: &str, start_index: usize, broken: usize) -> Vec<usize> {
    let mut valid_indexes = Vec::new();

    if start_index >= springs.len() {
        return valid_indexes;
    }

    let highest_start_index = springs[start_index..]
        .find('#')
        .map_or(springs.len(), |i| i + start_index);
    let mut index = start_index;

    while index + broken <= springs.len()
        && (highest_start_index == springs.len() || index <= highest_start_index)
    {
        if springs.chars().nth(index).unwrap() != '.' {
            if springs[index..index + broken].chars().all(|c| c != '.') {
                if index + broken == springs.len()
                    || springs.chars().nth(index + broken).unwrap() != '#'
                {
                    valid_indexes.push(index);
                }
            }
        }
        index += 1;
    }

    valid_indexes
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1",
        );

        assert_eq!(21, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1",
        );

        assert_eq!(525152, process_part_two(&input));
    }
}
