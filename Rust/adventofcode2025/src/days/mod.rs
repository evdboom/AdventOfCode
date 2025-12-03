pub mod day_01;
pub mod day_02;


pub fn run(day: u8, input: &str, part: u8) -> Result<usize, String> {
    match day {
        1 => Ok(day_01::solve(input, part)),
        2 => Ok(day_02::solve(input, part)),
        _ => Err(format!("Day {day:02} is not implemented yet")),
    }
}
