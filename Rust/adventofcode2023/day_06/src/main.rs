use std::fs;
use std::time::Instant;
use std::vec::Vec;

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
    let times_string = lines.next().unwrap().replace("Time:", "");
    let times = times_string
        .split(" ")
        .filter(|time| !time.is_empty())
        .map(|time| time.parse::<u64>().unwrap());
    let distances_string = lines.next().unwrap().replace("Distance:", "");
    let distances = distances_string
        .split(" ")
        .filter(|distance| !distance.is_empty())
        .map(|distance| distance.parse::<u64>().unwrap())
        .collect::<Vec<u64>>();

    let mut result: u64 = 1;
    let mut index: usize = 0;

    for time in times {
        result *= get_winning_options_count(time as f64, distances[index] as f64);
        index += 1;
    }

    result
}

fn process_part_two(input: &String) -> u64 {
    let mut lines = input.lines();
    let time = lines
        .next()
        .unwrap()
        .replace("Time:", "")
        .replace(" ", "")
        .parse::<u64>()
        .unwrap();
    let distance = lines
        .next()
        .unwrap()
        .replace("Distance:", "")
        .replace(" ", "")
        .parse::<u64>()
        .unwrap();

    get_winning_options_count(time as f64, distance as f64)
}

fn get_winning_options_count(time: f64, distance_to_beat: f64) -> u64 {
    let d = (time.powf(2.0) - 4.0 * distance_to_beat).sqrt();
    let lowest = (time - d) / 2.0;
    let highest = (time + d) / 2.0;

    let lowest_whole: u64 = (lowest + 1.0).floor() as u64;
    let highest_whole: u64 = (highest - 1.0).ceil() as u64;
    highest_whole - lowest_whole + 1
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "Time:      7  15   30
Distance:  9  40  200",
        );

        assert_eq!(288, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "Time:      7  15   30
Distance:  9  40  200",
        );

        assert_eq!(71503, process_part_two(&input));
    }
}
