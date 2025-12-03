mod days;

use std::{env, fs, process};
use std::time::Instant;

fn main() {
    if let Err(err) = run() {
        eprintln!("{err}");
        process::exit(1);
    }
}

fn run() -> Result<(), String> {
    let mut args = env::args().skip(1);
    let day_arg = match args.next() {
        Some(value) => value,
        None => return Err(usage()),
    };

    let day: u8 = day_arg
        .parse()
        .map_err(|_| format!("Invalid day '{}'. Expected a number between 1 and 25.", day_arg))?;

    let input = read_input(day)?;

    let start_time = Instant::now();
    let result_one = days::run(day, input.as_str(), 1)?;
    let duration_one = start_time.elapsed();
    let result_two = days::run(day, input.as_str(), 2)?;
    let duration_two = start_time.elapsed() - duration_one;

    println!("Day {day:02}");
    println!("  Part 1: {} took {:?}", result_one, duration_one);
    println!("  Part 2: {} took {:?}", result_two, duration_two);

    Ok(())
}

fn usage() -> String {
    "Usage: cargo run -- <day> the input defaults to ./inputs/inputXX.txt."
        .to_string()
}

fn read_input(day: u8) -> Result<String, String> {
    let input = fs::read_to_string(format!("./inputs/input{day:02}.txt")).expect("Could not read file");
    Ok(input)
}