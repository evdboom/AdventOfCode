use std::fs;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let part_one = get_calories(&input, 1);
    println!("{part_one}");
    let part_two = get_calories(&input, 3);
    println!("{part_two}");
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
