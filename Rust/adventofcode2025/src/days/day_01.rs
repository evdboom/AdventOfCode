use crate::days::DayResult;

pub fn solve(input: &str) -> DayResult {
    let sanitized = input.trim();

    DayResult {
        part_one: format!("Stubbed part 1 (input length: {})", sanitized.len()),
        part_two: "Stubbed part 2".to_string(),
    }
}
