use std::collections::HashMap;
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

fn process_part_one(input: &str) -> usize {
    let (instructions, nodes) = get_instructions_and_nodes(input);

    steps_to_target("AAA", "ZZZ", &instructions, &nodes)
}

fn process_part_two(input: &str) -> usize {
    let (instructions, nodes) = get_instructions_and_nodes(input);

    let values: Vec<_> = nodes
        .keys()
        .filter(|node| node.ends_with("A"))
        .map(|code| steps_to_target(code, "Z", &instructions, &nodes))
        .collect();

    values
        .iter()
        .cloned()
        .reduce(|a, b| least_common_multiple(a, b))
        .unwrap()
}

fn greatest_common_divisor(a: usize, b: usize) -> usize {
    if b == 0 {
        a
    } else {
        greatest_common_divisor(b, a % b)
    }
}

fn least_common_multiple(a: usize, b: usize) -> usize {
    a * b / greatest_common_divisor(a, b)
}

fn steps_to_target(
    start: &str,
    target: &str,
    instructions: &str,
    nodes: &HashMap<String, (String, String)>,
) -> usize {
    let mut steps = 0;
    let mut current = start.to_string();
    while !current.ends_with(target) {
        let step = steps % instructions.len();
        let instruction = instructions.chars().nth(step).unwrap();
        current = if instruction == 'L' {
            nodes.get(&current).unwrap().0.to_string()
        } else {
            nodes.get(&current).unwrap().1.to_string()
        };
        steps += 1;
    }
    steps
}

fn get_instructions_and_nodes(input: &str) -> (String, HashMap<String, (String, String)>) {
    let mut lines = input.lines();
    let instructions = lines.next().unwrap().to_string();
    lines.next();
    let mut nodes = HashMap::new();
    for line in lines {
        let mut parts = line.split(" = ");
        let node = parts.next().unwrap();
        let mut targets = parts.next().unwrap().split(", ");
        let left = targets.next().unwrap();
        let right = targets.next().unwrap();
        let left = left.trim_matches(|c| c == '(');
        let right = right.trim_matches(|c| c == ')');
        nodes.insert(node.to_string(), (left.to_string(), right.to_string()));
    }
    (instructions, nodes)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)",
        );

        assert_eq!(6, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)",
        );

        assert_eq!(6, process_part_two(&input));
    }
}
