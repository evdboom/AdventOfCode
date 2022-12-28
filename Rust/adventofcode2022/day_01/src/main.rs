use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = get_calories(&input, 1);
    let duration_one = start.elapsed();    
    let part_two = get_calories(&input, 3);    
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);    
}

fn get_calories(input: &String, no_elves: usize) -> i32 {
    let mut elves: Vec<i32> = input
        .split("\n\n")
        .map(|x| {
            x.lines()
                .filter(|y| y != &"")
                .map(|y| y.parse::<i32>().expect("Could not parse"))
                .sum()
        })
        .collect();
    elves.sort_unstable_by(|a, b| b.cmp(a));
    elves.iter().take(no_elves).sum()
}
