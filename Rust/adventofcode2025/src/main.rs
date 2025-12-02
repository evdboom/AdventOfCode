mod days;

use std::{env, fs, process};

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

    let result = days::run(day, input.as_str())?;

    println!("Day {day:02}");
    println!("  Part 1: {}", result.part_one);
    println!("  Part 2: {}", result.part_two);

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