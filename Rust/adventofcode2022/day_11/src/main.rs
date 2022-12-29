use std::collections::HashMap;
use std::fs;
use std::time::Instant;

struct Monkey {
    items: Vec<i64>,
    modulo: i64,
    targets: (i32, i32),
    items_tested: i64,
    operation: String
}

impl Monkey {
    fn throw_items(&mut self, relief_function: fn(i64, i64) -> i64, relief_factor: i64) -> Vec<(i32, i64)> {
        let mut result = Vec::new();
        for item in &self.items {
            self.items_tested += 1;
            let new_item = relief_function(self.apply_operation(*item), relief_factor);
            let target = if new_item % self.modulo == 0 { self.targets.0 } else { self.targets.1};
            result.push((target, new_item));
        }
        self.items.clear();        
           
        result
    }

    fn apply_operation(&self, item: i64) -> i64 {
        if self.operation.contains("*") {
            let mut parts = self.operation.split(" * ");
            parts.next();
            let part = parts.next().unwrap().parse::<i64>();
            match part {
                Ok(ok) => item * ok,
                Err(_) => item * item
            }
        } else {
            let mut parts = self.operation.split(" + ");
            parts.next();
            let part = parts.next().unwrap().parse::<i64>();
            match part {
                Ok(ok) => item + ok,
                Err(_) => item + item
            }
        }
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
    
    let mut tested: Vec<i64> = monkeys.values().map(|monkey| monkey.items_tested).collect();
    tested.sort_by(|a,b| b.cmp(a));
    tested[0] * tested[1]
}

fn relief_one(item: i64, factor: i64) -> i64 {
    item / factor
}

fn part_two(input: &String) -> i64 {
    let start = Instant::now();
    let mut monkeys = get_monkeys(input);
    let d1 = start.elapsed();
    println!("Getting monkeys took: {:?}", d1);    
    let factor = monkeys.values().fold(1i64, |sum, monkey| sum * monkey.modulo);
    let d2 = start.elapsed() - d1;
    println!("Getting factor took: {:?}", d2);
    do_rounds(&mut monkeys, 10000, relief_two, factor);
    
    let mut tested: Vec<i64> = monkeys.values().map(|monkey| monkey.items_tested).collect();
    tested.sort_by(|a,b| b.cmp(a));
    tested[0] * tested[1]
}

fn relief_two(item: i64, factor: i64) -> i64 {
    item % factor
} 

fn get_monkeys(input: &String) -> HashMap<i32, Monkey> {
    let mut result = HashMap::new();

    let monkey_strings = input.split("\n\n");
    for monkey_string in monkey_strings {        
        let mut lines = monkey_string.lines();
        let number = lines.next().unwrap().replace("Monkey ", "").replace(":", "").parse::<i32>().unwrap();
        let monkey = Monkey {
            items: lines.next().unwrap().replace("  Starting items: ", "").split(", ").map(|i| i.parse::<i64>().unwrap()).collect(),
            operation: lines.next().unwrap().replace("  Operation: new = ", ""),
            modulo: lines.next().unwrap().replace("  Test: divisible by ", "").parse::<i64>().unwrap(),
            targets: (
                lines.next().unwrap().replace("    If true: throw to monkey ", "").parse::<i32>().unwrap(),
                lines.next().unwrap().replace("    If false: throw to monkey ", "").parse::<i32>().unwrap()
            ),            
            items_tested: 0            
        };
        result.insert(number, monkey);        
    }

    result
}

fn do_rounds(monkeys: &mut HashMap<i32, Monkey>, no_rounds: i32, relief_function: fn(i64, i64) -> i64, relief_factor: i64) {
    let mut counter = 0;
    let mut keys: Vec<i32> = monkeys.keys().map(|k| k.clone()).collect();
    keys.sort_unstable();
    while counter < no_rounds {
        for key in &keys {
            let thrown = monkeys.get_mut(key).unwrap().throw_items(relief_function, relief_factor);
            for item in thrown {
                monkeys.get_mut(&item.0).unwrap().items.push(item.1);
            }
        }

        counter += 1;
    }
}