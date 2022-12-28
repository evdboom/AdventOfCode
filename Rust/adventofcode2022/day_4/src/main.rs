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
    input.lines().filter(|line| line != &"").fold(0i32, |sum, line| {
        let ranges = get_ranges(line);

        let dif_one = ranges.0 - ranges.2;
        let dif_two = ranges.3 - ranges.1;

        if dif_one >= 0 && dif_two >= 0 {
            sum + 1
        } else if dif_one <= 0 && dif_two <= 0 {
            sum + 1
        } else {
            sum
        }
    })
}

fn part_two(input: &String) -> i32 {
    input.lines().filter(|line| line != &"").fold(0i32, |sum, line| {
        let ranges = get_ranges(line);

        if ranges.0 <= ranges.3 && ranges.1 >= ranges.2 {
            sum + 1
        } else {
            sum
        }
    })
}


fn get_ranges(line: &str) -> (i32, i32, i32, i32) {
    let mut parts = line.split(",").map(|part| part.split("-"));
    let mut range_one = parts.next().unwrap();
    let mut range_two = parts.next().unwrap();

    let start_one = range_one.next().unwrap().parse::<i32>().unwrap();
    let end_one = range_one.next().unwrap().parse::<i32>().unwrap();
    let start_two = range_two.next().unwrap().parse::<i32>().unwrap();
    let end_two = range_two.next().unwrap().parse::<i32>().unwrap();
    
    (start_one, end_one, start_two, end_two)
}
