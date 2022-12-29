use std::fs;
use std::time::Instant;

struct Monkey {
    items: Vec<i64>,
    modulo: i64,
    targets: (usize, usize),
    items_tested: i64,
    operation: Box<dyn Fn(i64) -> i64>,
}

impl Monkey {
    fn throw_items(
        &mut self,
        relief_function: fn(i64, i64) -> i64,
        relief_factor: i64,
    ) -> Vec<(usize, i64)> {
        let mut result = vec![];
        while let Some(mut item) = self.items.pop() {
            item = relief_function((self.operation)(item), relief_factor);
            let target = if item % self.modulo == 0 {
                self.targets.0
            } else {
                self.targets.1
            };
            result.push((target, item));
            self.items_tested += 1;
        }

        result
    }
}

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

fn part_one(input: &String) -> i64 {
    let mut monkeys = get_monkeys(input);

    do_rounds(&mut monkeys, 20, relief_one, 3);

    monkeys.sort_unstable_by(|a, b| b.items_tested.cmp(&a.items_tested));
    monkeys[0].items_tested * monkeys[1].items_tested
}

fn relief_one(item: i64, factor: i64) -> i64 {
    item / factor
}

fn part_two(input: &String) -> i64 {
    let mut monkeys = get_monkeys(input);
    let factor = monkeys.iter().map(|monkey| monkey.modulo).product();
    do_rounds(&mut monkeys, 10000, relief_two, factor);

    monkeys.sort_unstable_by(|a, b| b.items_tested.cmp(&a.items_tested));
    monkeys[0].items_tested * monkeys[1].items_tested
}

fn relief_two(item: i64, factor: i64) -> i64 {
    item % factor
}

fn get_monkeys(input: &String) -> Vec<Monkey> {
    let monkey_strings = input.split("\n\n");
    monkey_strings
        .map(|monkey_string| {
            let mut lines = monkey_string.lines();
            lines.next();
            Monkey {
                items: lines
                    .next()
                    .unwrap()
                    .replace("  Starting items: ", "")
                    .split(", ")
                    .map(|i| i.parse::<i64>().unwrap())
                    .collect(),
                operation: parse_operation(
                    lines.next().unwrap().replace("  Operation: new = old ", ""),
                ),
                modulo: lines
                    .next()
                    .unwrap()
                    .replace("  Test: divisible by ", "")
                    .parse::<i64>()
                    .unwrap(),
                targets: (
                    lines
                        .next()
                        .unwrap()
                        .replace("    If true: throw to monkey ", "")
                        .parse::<usize>()
                        .unwrap(),
                    lines
                        .next()
                        .unwrap()
                        .replace("    If false: throw to monkey ", "")
                        .parse::<usize>()
                        .unwrap(),
                ),
                items_tested: 0,
            }
        })
        .collect()
}

fn parse_operation(line: String) -> Box<dyn Fn(i64) -> i64> {
    if line.starts_with("*") {
        let part = line.replace("* ", "").parse::<i64>();
        match part {
            Ok(ok) => Box::new(move |item| item * ok),
            Err(_) => Box::new(move |item| item * item),
        }
    } else {
        let part = line.replace("+ ", "").parse::<i64>();
        match part {
            Ok(ok) => Box::new(move |item| item + ok),
            Err(_) => Box::new(move |item| item + item),
        }
    }
}

fn do_rounds(
    monkeys: &mut Vec<Monkey>,
    no_rounds: i32,
    relief_function: fn(i64, i64) -> i64,
    relief_factor: i64,
) {
    for _ in 0..no_rounds {
        for monkey in 0..monkeys.len() {
            let thrown = monkeys[monkey].throw_items(relief_function, relief_factor);
            for item in thrown {
                monkeys[item.0].items.insert(0, item.1);
            }
        }
    }
}
