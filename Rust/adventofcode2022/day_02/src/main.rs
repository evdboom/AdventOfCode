use std::fs;
use std::time::Instant;
fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    
    let start = Instant::now();
    let part_one = part_one(&input);
    let duration_one = start.elapsed();    
    let part_two = part_two(&input);    
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn part_one(input: &String) -> i32 {
    input.lines().fold(0i32, |sum, value| {
        let mut bytes = value.bytes();
        let them: i32 = (bytes.next().expect("Could not find them") - b'A' + 1).into();
        bytes.next();
        let you: i32 = (bytes.next().expect("Could not find you") - b'X' + 1).into();

        let raw: i32 = you - them;
        let result = if raw < 0 {
            ((raw - 4) % 3) + 2
        } else {
            (raw + 4) % 3
        } * 3;

        sum + you + result
    })
}

fn part_two(input: &String) -> i32 {
    input.lines().fold(0i32, |sum, value| {
        let mut bytes = value.bytes();
        let them: i32 = (bytes.next().expect("Could not find them") - b'A').into();
        bytes.next();
        let you: i32 = (bytes.next().expect("Could not find you") - b'X').into();

        let result = 1 + (them + you + 2) % 3;

        sum + result + you * 3
    })
}
