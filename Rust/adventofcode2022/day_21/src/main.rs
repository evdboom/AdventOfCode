use std::collections::HashMap;
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

fn part_one(input: &String) -> isize {
    let monkeys = get_monkeys(input);
    get_result("root", &monkeys)
}

fn part_two(input: &String) -> isize {
    let monkeys = get_monkeys(input);
    let formula = get_formula("root", &monkeys);

    let (part_1, part_2) = formula.split_once(" = ").unwrap();
    let value_1 = part_1.parse::<isize>();
    let value_2 = part_2.parse::<isize>();
    if value_1.is_ok() {
        decode_formula(part_2, value_1.unwrap())
    } else {
        decode_formula(part_1, value_2.unwrap())
    }
}

fn decode_formula(mut formula: &str, value: isize) -> isize {
    let removed = formula
        .strip_prefix("(")
        .and_then(|stripped| stripped.strip_suffix(")"));

    if removed.is_some() {
        formula = removed.unwrap();
    }

    let option = formula.find("(");
    if option.is_some() {
        let index = option.unwrap();
        let inner = if index == 0 {
            &formula[..formula.rfind(")").unwrap() + 1]
        } else {
            &formula[index..]
        };
        let other = formula.replace(inner, "");
        let new_value = get_value(other.trim(), value);

        decode_formula(inner, new_value)
    } else {
        let other = formula.replace("humn", "");
        get_value(other.trim(), value)
    }
}

fn get_value(other: &str, value: isize) -> isize {
    if other.starts_with("+") || other.ends_with("+") {
        value - other.replace("+", "").trim().parse::<isize>().unwrap()
    } else if other.starts_with("*") || other.ends_with("*") {
        value / other.replace("*", "").trim().parse::<isize>().unwrap()
    } else if other.starts_with("-") {
        value + other.replace("-", "").trim().parse::<isize>().unwrap()
    } else if other.ends_with("-") {
        (value - other.replace("-", "").trim().parse::<isize>().unwrap()) * -1
    } else if other.starts_with("/") {
        value * other.replace("/", "").trim().parse::<isize>().unwrap()
    } else if other.ends_with("/") {
        other.replace("/", "").trim().parse::<isize>().unwrap() / value
    } else {
        panic!("No known operator!")
    }
}

fn get_formula(monkey: &str, monkeys: &HashMap<&str, &str>) -> String {
    if monkey == "humn" {
        return String::from(monkey);
    }
    let action = monkeys[monkey];
    if action.parse::<isize>().is_ok() {
        return String::from(action);
    }

    let mut parts = action.split_whitespace();
    let monkey_1 = get_formula(parts.next().unwrap(), monkeys);
    let operator = parts.next().unwrap();
    let monkey_2 = get_formula(parts.next().unwrap(), monkeys);

    if monkey == "root" {
        return format!("{monkey_1} = {monkey_2}");
    }
    let number_1 = monkey_1.parse::<isize>();
    let number_2 = monkey_2.parse::<isize>();

    if number_1.is_ok() && number_2.is_ok() {
        match operator {
            "+" => format!("{}", number_1.unwrap() + number_2.unwrap()),
            "*" => format!("{}", number_1.unwrap() * number_2.unwrap()),
            "-" => format!("{}", number_1.unwrap() - number_2.unwrap()),
            "/" => format!("{}", number_1.unwrap() / number_2.unwrap()),
            _ => panic!("unkown operator"),
        }
    } else {
        format!("({monkey_1} {operator} {monkey_2})")
    }
}

fn get_result(monkey: &str, monkeys: &HashMap<&str, &str>) -> isize {
    let action = monkeys[monkey];
    let value = action.parse::<isize>();
    if value.is_ok() {
        return value.unwrap();
    }

    let mut parts = action.split_whitespace();
    let monkey_1 = get_result(parts.next().unwrap(), monkeys);
    let operator = parts.next().unwrap();
    let monkey_2 = get_result(parts.next().unwrap(), monkeys);

    match operator {
        "+" => monkey_1 + monkey_2,
        "*" => monkey_1 * monkey_2,
        "-" => monkey_1 - monkey_2,
        "/" => monkey_1 / monkey_2,
        _ => panic!("unkown operator"),
    }
}

fn get_monkeys<'m>(input: &'m String) -> HashMap<&'m str, &'m str> {
    let mut result = HashMap::new();

    for (monkey, action) in input.lines().map(|line| line.split_once(": ").unwrap()) {
        result.insert(monkey, action);
    }

    result
}
