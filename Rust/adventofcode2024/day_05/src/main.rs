use std::collections::{HashMap, VecDeque};
use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");
    let start = Instant::now();
    let part_one = process_part_one(&input);
    let duration_one = start.elapsed();
    let part_two = process_part_two(&input);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn process_part_one(input: &str) -> u32 {
    let mut parts = input.split("\n\n");
    let pages = get_pages(parts.next().unwrap());
    let orders = get_orders(parts.next().unwrap());
    let validated = validate_orders(&pages, &orders, true);
    validated
        .iter()
        .fold(0, |acc, x| acc + x[x.len() / 2] as u32)
}

fn process_part_two(input: &str) -> u32 {
    let mut parts = input.split("\n\n");
    let pages = get_pages(parts.next().unwrap());
    let orders = get_orders(parts.next().unwrap());
    let validated = validate_orders(&pages, &orders, false);
    validated.iter().fold(0, |acc, x| {
        let len = x.len();
        acc + reorder(&x, &pages)[len / 2] as u32
    })
}

fn reorder(order: &Vec<i32>, pages: &HashMap<i32, Vec<i32>>) -> Vec<i32> {
    let mut result = Vec::new();
    let mut skipped = VecDeque::new();

    while result.len() != order.len() {
        for index in 0..order.len() {
            let mut can_add = true;
            if let Some(dependents) = pages.get(&order[index]) {
                can_add = !dependents
                    .iter()
                    .any(|dependent| order.contains(dependent) && !result.contains(dependent));
            }

            if can_add {
                result.push(order[index]);
            } else {
                skipped.push_back(order[index]);
            }
        }

        while let Some(skip) = skipped.pop_front() {
            let mut can_add = true;
            if let Some(dependents) = pages.get(&skip) {
                let required: Vec<&i32> = dependents
                    .iter()
                    .filter(|dependent| order.contains(dependent) && !result.contains(dependent))
                    .collect();

                can_add = required.is_empty();
            }

            if can_add {
                result.push(skip);
            } else {
                skipped.push_back(skip);
            }
        }
    }
    result
}

fn validate_orders(
    pages: &HashMap<i32, Vec<i32>>,
    orders: &Vec<Vec<i32>>,
    valid: bool,
) -> Vec<Vec<i32>> {
    let mut result = Vec::new();
    for order in orders {
        let mut seen = Vec::new();
        let mut valid_order = true;
        for i in 0..order.len() - 1 {
            let index = order.len() - i - 1;
            let page = order[index];
            let dependents = pages.get(&page).unwrap();
            for dependent in dependents {
                if seen.contains(dependent) {
                    valid_order = false;
                    break;
                }
            }
            if !valid_order {
                break;
            }
            seen.push(page);
        }

        if valid_order == valid {
            result.push(order.clone());
        }
    }
    result
}

fn get_pages(pages: &str) -> HashMap<i32, Vec<i32>> {
    let mut result = HashMap::new();
    for page in pages.lines() {
        let mut parts = page.split("|");
        let dependent = parts.next().unwrap().parse::<i32>().unwrap();
        let page = parts.next().unwrap().parse::<i32>().unwrap();

        let order_vec = result.entry(page).or_insert(Vec::new());
        order_vec.push(dependent);
    }
    result
}

fn get_orders(orders: &str) -> Vec<Vec<i32>> {
    orders
        .lines()
        .map(|x| x.split(',').map(|y| y.parse::<i32>().unwrap()).collect())
        .collect()
}
