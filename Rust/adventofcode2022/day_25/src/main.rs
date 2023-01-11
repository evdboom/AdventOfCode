use std::fs;
use std::ops::Add;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = part_one(&input);
    let duration_one = start.elapsed();    
    println!("Part one: {}, took: {:?}", part_one, duration_one);
}

fn part_one(input: &String) -> String {
    let decimal: isize = input.lines().map(|line| translate_to_decimal(line)).sum();
    translate_to_snafu(decimal)
}

fn translate_to_decimal(line: &str) -> isize {
    line.chars().rev().enumerate().fold(0, |sum, (index,char)| {
        let factor = 5isize.pow(index as u32) as isize;
        sum + match char {
            '2' => factor * 2,
            '1' => factor,
            '0' => 0,
            '-' => factor * -1,
            '=' => factor * -2,
            _ => panic!("Unknown SNAFU")
        }        
    })    
}

fn translate_to_snafu(decimal: isize) -> String {
    let mut result = String::new();
    let start_power = get_highest_power(decimal);
    let mut calc = decimal;
    for power in (0..=start_power).rev() {
        let factor = 5isize.pow(power);
        let mut base = calc / factor;
        let lower = get_maximum_for_lower_power(power);
        let remainder = calc - base * factor;
        if remainder.abs() > lower {
            base += if base > 0 || remainder > 0 { 1 } else { -1 };
        }
        result.push(match base {
            2 => '2',
            1 => '1',
            0 => '0',
            -1 => '-',
            -2 => '=',
            _ => panic!("Could not match to SNAFU")
        });
        calc -= base * factor;

    }

    result
}

fn get_highest_power(decimal: isize) -> u32 {
    let mut power = 0;    
    loop {
        let max = 5isize.pow(power);
        let max_lower = get_maximum_for_lower_power(power);        
        if (decimal - max).abs() < max_lower || (decimal - max * 2).abs() < max_lower {
            break;
        }
        power += 1;
    }
    power
}

fn get_maximum_for_lower_power(power: u32) -> isize {
    (0..power).fold(0, |sum, p| sum + 5isize.pow(p) * 2)
}

