mod logic_gate;

use std::collections::{BTreeSet, HashMap};
use std::fs;
use std::time::Instant;

use logic_gate::{GateType, LogicGate};

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
    let (mut wires, gates) = get_wires_and_gates(input);

    calculate_output(&mut wires, &gates);
    get_value(&wires, b'z').0
}

// iteration 9 :), with a lot if help (fdumontmd)
fn process_part_two(input: &str) -> String {
    let (wires, gates) = get_wires_and_gates(input);

    let count = wires.len() / 2;
    let start = gates
        .iter()
        .find(|gate| {
            gate.input_a == "x00".as_bytes().to_vec()
                && gate.input_b == "y00".as_bytes().to_vec()
                && gate.gate_type == GateType::XOR
        })
        .unwrap();

    let mut cache = BTreeSet::new();
    if start.output_wire != "z00".as_bytes().to_vec() {
        cache.insert(start.output_wire.clone());
    }

    let mut carry = gates
        .iter()
        .find_map(|gate| {
            if gate.input_a == "x00".as_bytes().to_vec()
                && gate.input_b == "y00".as_bytes().to_vec()
                && gate.gate_type == GateType::AND
            {
                Some(gate.output_wire.clone())
            } else {
                None
            }
        })
        .unwrap();

    for i in 1..count {
        let a = format!("x{i:02}").as_bytes().to_vec();
        let b = format!("y{i:02}").as_bytes().to_vec();
        let z = format!("z{i:02}").as_bytes().to_vec();

        let add = &gates
            .iter()
            .find(|gate| gate.is_input(&a) && gate.is_input(&b) && gate.gate_type == GateType::XOR)
            .unwrap()
            .output_wire;

        let next_add = &gates
            .iter()
            .find(|gate| {
                (gate.is_input(&add) || gate.is_input(&carry)) && gate.gate_type == GateType::XOR
            })
            .unwrap();

        if &next_add.output_wire != &z {
            cache.insert(next_add.output_wire.clone());
            cache.insert(z.clone());
        }

        if !next_add.is_input(add) {
            cache.insert(add.clone());
        }

        if !next_add.is_input(&carry) {
            cache.insert(carry.clone());
        }

        let basic_carry = &gates
            .iter()
            .find(|gate| gate.is_input(&a) && gate.is_input(&b) && gate.gate_type == GateType::AND)
            .unwrap()
            .output_wire;

        let cascade_carry = gates
            .iter()
            .find(|gate| {
                (gate.is_input(add) || gate.is_input(&carry)) && gate.gate_type == GateType::AND
            })
            .unwrap();

        if !cascade_carry.is_input(add) {
            cache.insert(add.clone());
        }

        if !cascade_carry.is_input(&carry) {
            cache.insert(carry.clone());
        }

        let carry_gate = gates
            .iter()
            .find(|gate| {
                (gate.is_input(&cascade_carry.output_wire) || gate.is_input(&basic_carry))
                    && gate.gate_type == GateType::OR
            })
            .unwrap();

        if !carry_gate.is_input(&cascade_carry.output_wire) {
            cache.insert(cascade_carry.output_wire.clone());
        }

        if !carry_gate.is_input(&basic_carry) {
            cache.insert(basic_carry.clone());
        }

        carry = carry_gate.output_wire.clone();
    }

    cache
        .into_iter()
        .map(|c| String::from_utf8(c).unwrap())
        .collect::<Vec<String>>()
        .join(",")
}

fn get_value(wires: &HashMap<Vec<u8>, bool>, to_get: u8) -> (usize, Vec<bool>) {
    let mut with_char = wires
        .iter()
        .filter(|(key, _)| key[0] == to_get)
        .collect::<Vec<_>>();

    with_char.sort_by(|a, b| a.0.cmp(b.0));

    let mut result = 0;
    let mut values = Vec::new();
    for i in 0..with_char.len() {
        let value = *with_char[i].1;
        if value {
            result += 1 << i;
        }
        values.push(value);
    }

    (result, values)
}

fn calculate_output(wires: &mut HashMap<Vec<u8>, bool>, gates: &Vec<LogicGate>) -> Option<bool> {
    let mut changed = true;
    let mut to_calculate = gates.len();
    while changed {
        changed = false;
        for gate in gates {
            if let Some(value) = gate.calculate_output(&wires) {
                if wires.insert(gate.output_wire.clone(), value).is_none() {
                    changed = true;
                    to_calculate -= 1;
                };
            }
        }
    }

    if to_calculate == 0 {
        Some(true)
    } else {
        None
    }
}

fn get_wires_and_gates(input: &str) -> (HashMap<Vec<u8>, bool>, Vec<LogicGate>) {
    let mut wires = HashMap::new();
    let mut gates = Vec::new();

    let mut gates_started = false;
    for line in input.lines() {
        if line.is_empty() {
            gates_started = true;
            continue;
        }

        if !gates_started {
            let mut parts = line.split(": ");
            wires.insert(
                parts.next().unwrap().as_bytes().to_vec(),
                parse_to_bool(parts.next().unwrap()),
            );
        } else {
            let mut parts = line.split(" ");
            let input_a = parts.next().unwrap().as_bytes().to_vec();
            let gate_type = parse_to_gate_type(parts.next().unwrap());
            let input_b = parts.next().unwrap().as_bytes().to_vec();
            let output = parts.last().unwrap().as_bytes().to_vec();

            gates.push(LogicGate::new(input_a, input_b, output, gate_type));
        }
    }

    (wires, gates)
}

fn parse_to_bool(value: &str) -> bool {
    match value {
        "0" => false,
        "1" => true,
        _ => panic!("Invalid value"),
    }
}

fn parse_to_gate_type(value: &str) -> logic_gate::GateType {
    match value {
        "AND" => logic_gate::GateType::AND,
        "OR" => logic_gate::GateType::OR,
        "XOR" => logic_gate::GateType::XOR,
        _ => panic!("Invalid value"),
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "x00: 1
x01: 0
x02: 1
x03: 1
x04: 0
y00: 1
y01: 1
y02: 1
y03: 1
y04: 1

ntg XOR fgs -> mjb
y02 OR x01 -> tnw
kwq OR kpj -> z05
x00 OR x03 -> fst
tgd XOR rvg -> z01
vdt OR tnw -> bfw
bfw AND frj -> z10
ffh OR nrd -> bqk
y00 AND y03 -> djm
y03 OR y00 -> psh
bqk OR frj -> z08
tnw OR fst -> frj
gnj AND tgd -> z11
bfw XOR mjb -> z00
x03 OR x00 -> vdt
gnj AND wpb -> z02
x04 AND y00 -> kjc
djm OR pbm -> qhw
nrd AND vdt -> hwm
kjc AND fst -> rvg
y04 OR y02 -> fgs
y01 AND x02 -> pbm
ntg OR kjc -> kwq
psh XOR fgs -> tgd
qhw XOR tgd -> z09
pbm OR djm -> kpj
x03 XOR y03 -> ffh
x00 XOR y04 -> ntg
bfw OR bqk -> z06
nrd XOR fgs -> wpb
frj XOR qhw -> z04
bqk OR frj -> z07
y03 OR x01 -> nrd
hwm AND bqk -> z03
tgd XOR rvg -> z12
tnw OR pbm -> gnj",
        );

        assert_eq!(2024, process_part_one(&input));
    }
}
