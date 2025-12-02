pub mod day_01;

#[derive(Debug, Clone)]
pub struct DayResult {
	pub part_one: String,
	pub part_two: String,
}

pub fn run(day: u8, input: &str) -> Result<DayResult, String> {
	match day {
		1 => Ok(day_01::solve(input)),
		_ => Err(format!("Day {day:02} is not implemented yet")),
	}
}