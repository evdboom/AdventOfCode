use std::collections::HashSet;
use std::fs;
use std::hash::Hash;
use std::str::Lines;
use std::time::Instant;

struct MappingGroup {
    from: String,
    to: String,
    mappings: HashSet<Mapping>,
}

impl Hash for MappingGroup {
    fn hash<H: std::hash::Hasher>(&self, state: &mut H) {
        self.from.hash(state);
        self.to.hash(state);
    }
}

impl Eq for MappingGroup {}

impl PartialEq for MappingGroup {
    fn eq(&self, other: &Self) -> bool {
        self.from == other.from && self.to == other.to
    }
}

#[derive(Eq, Hash, PartialEq)]
struct Mapping {
    source_start: u64,
    destination_start: u64,
    length: u64,
}

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

fn process_part_one(input: &String) -> u64 {
    let mut lines = input.lines();
    let seeds = lines
        .next()
        .unwrap()
        .replace("seeds: ", "")
        .split(" ")
        .map(|seed| seed.parse::<u64>().unwrap())
        .collect::<Vec<u64>>();

    let mappings = get_mappings(lines);
    0
}

fn process_part_two(input: &String) -> u64 {
    0
}

fn get_mappings(input: Lines<'_>) -> HashSet<MappingGroup> {
    let mut result: HashSet<MappingGroup> = HashSet::new();
    let mut current_mapping_from = "";
    let mut current_mapping_to = "";
    for line in input {
        if line.is_empty() {
            continue;
        }
        if line.contains("map:") {
            let parts_string = line.replace(" map:", "");
            let mut parts = parts_string.split("-to");
            current_mapping_from = parts.next().unwrap();
            current_mapping_to = parts.next().unwrap();
            let mut current_group = MappingGroup {
                from: String::from(current_mapping_from),
                to: String::from(current_mapping_to),
                mappings: HashSet::new(),
            };
            result.insert(current_group);
        } else {
            let mut parts = line.split(" ").map(|part| part.parse::<u64>().unwrap());
            let mapping = Mapping {
                destination_start: parts.next().unwrap(),
                source_start: parts.next().unwrap(),
                length: parts.next().unwrap(),
            };
            current_group = result.get(value)
        }
    }

    result
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4",
        );

        assert_eq!(35, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4",
        );

        assert_eq!(46, process_part_two(&input));
    }
}
