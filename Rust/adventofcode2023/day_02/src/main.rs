use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = get_valid_rounds(&input, 12, 13, 14);
    let duration_one = start.elapsed();
    let part_two = get_rounds_power(&input);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn get_valid_rounds(input: &String, max_red: i32, max_green: i32, max_blue: i32) -> u32 {
    input
        .lines()
        .map(|line| {
            let mut parts = line.split(": ");
            let game_string = parts.next().unwrap().replace("Game ", "");
            let game = game_string.parse::<u32>().unwrap();
            let rounds = parts.next().unwrap().split("; ");
            let mut valid = true;
            for round in rounds {
                let items = round.split(", ");

                for item in items {
                    let mut item_parts = item.split(" ");
                    let value_string = item_parts.next().unwrap();
                    let value = value_string.parse::<i32>().unwrap();
                    let color = item_parts.next().unwrap();

                    if color == "red" {
                        valid = value <= max_red;
                        break;
                    } else if color == "green" {
                        valid = value <= max_green;
                        break;
                    } else if color == "blue" {
                        valid = value <= max_blue;
                        break;
                    }
                    if !valid {
                        break;
                    }
                }
                if !valid {
                    break;
                }
            }

            if valid {
                game
            } else {
                0
            }
        })
        .sum()
}

fn get_rounds_power(input: &String) -> u32 {
    input
        .lines()
        .map(|line| {
            let mut parts = line.split(": ");
            parts.next();
            let rounds = parts.next().unwrap().split("; ");
            let mut max_red: u32 = 0;
            let mut max_green: u32 = 0;
            let mut max_blue: u32 = 0;
            for round in rounds {
                let items = round.split(", ");

                for item in items {
                    let mut item_parts = item.split(" ");
                    let value_string = item_parts.next().unwrap();
                    let value = value_string.parse::<u32>().unwrap();
                    let color = item_parts.next().unwrap();

                    if color == "red" {
                        if value > max_red {
                            max_red = value;
                        }
                    } else if color == "green" {
                        if value > max_green {
                            max_green = value;
                        }
                    } else if color == "blue" {
                        if value > max_blue {
                            max_blue = value;
                        }
                    }
                }
            }

            max_red * max_green * max_blue
        })
        .sum()
}
