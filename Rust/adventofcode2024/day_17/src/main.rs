mod program;

use program::{OperationResult, Program};
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

fn process_part_one(input: &str) -> String {
    let mut program = get_program(input);
    let mut result = Vec::new();
    let cloned_program = program.clone();
    let mut iterator = cloned_program.iter();
    while let Some((opcode, operand)) = iterator.next() {
        let operation = perform_operation(opcode, operand, &mut program);
        if let OperationResult::Value(value) = operation {
            result.push(value.to_string());
        } else if let OperationResult::Jump(value) = operation {
            iterator.jump(value);
        }
    }

    result.join(",")
}

fn process_part_two(input: &str) -> usize {
    let source = get_program(input);
    let mut found = vec![0];

    // hardcoded from program: A always is divided by 8
    // (havent figured out a way to determine that runtime).
    //
    // move backwards through the program:
    //   for the last step: which values of A < 8 (as A must be 0 for the program to Halt)
    //   could have been used to get the last instruction.
    //
    //   multiply that by 8 and check which of the possible values of A could have been used
    //   to get the previous instruction.
    //   because of truncation we need to check all values (previous A + 0-7).
    //
    //   repeat until we have found the first values of A, and take the lowest (if multiple).
    //   because we are processing one loop at a time, we break at a jump instruction.

    for step in 0..source.get_instruction_count() {
        let mut new_found = Vec::new();
        for f in 0..found.len() {
            for i in 0..=7 {
                if i == 0 && step == 0 {
                    // last input starts at 1, otherwise program would have halted a step earlier
                    continue;
                }

                let mut program = source.clone();
                program.set_a(8 * found[f] + i);
                let cloned_program = program.clone();
                let mut iterator = cloned_program.iter();
                while let Some((opcode, operand)) = iterator.next() {
                    let operation = perform_operation(opcode, operand, &mut program);
                    if let OperationResult::Value(value) = operation {
                        if let Some(instruction) =
                            source.get_instruction(&(source.get_instruction_count() - step - 1))
                        {
                            if instruction == value {
                                new_found.push(8 * found[f] + i);
                                break;
                            } else {
                                break;
                            }
                        } else {
                            break;
                        }
                    } else if let OperationResult::Jump(_) = operation {
                        break;
                    }
                }
            }
        }
        found = new_found;
    }
    *found.iter().min().unwrap()
}

fn get_combo(operand: usize, program: &Program) -> usize {
    match operand {
        0 | 1 | 2 | 3 => operand,
        4 => program.get_a(),
        5 => program.get_b(),
        6 => program.get_c(),
        _ => panic!("Invalid operand"),
    }
}

fn perform_operation(opcode: usize, operand: usize, program: &mut Program) -> OperationResult {
    let mut result = OperationResult::None;
    match opcode {
        0 => program.set_a(program.get_a() / 2usize.pow(get_combo(operand, program) as u32)),
        1 => program.set_b(program.get_b() ^ operand),
        2 => program.set_b(get_combo(operand, program) % 8),
        3 => {
            if program.get_a() != 0 {
                result = OperationResult::Jump(operand)
            }
        }
        4 => program.set_b(program.get_b() ^ program.get_c()),
        5 => result = OperationResult::Value(get_combo(operand, program) % 8),
        6 => program.set_b(program.get_a() / 2usize.pow(get_combo(operand, program) as u32)),
        7 => program.set_c(program.get_a() / 2usize.pow(get_combo(operand, program) as u32)),
        _ => panic!("Invalid opcode"),
    }

    result
}

fn get_program(input: &str) -> Program {
    let mut result = Program::new();

    for line in input.lines() {
        if line.starts_with("Register A: ") {
            result.set_a(line.split_whitespace().last().unwrap().parse().unwrap());
        } else if line.starts_with("Register B: ") {
            result.set_b(line.split_whitespace().last().unwrap().parse().unwrap());
        } else if line.starts_with("Register C: ") {
            result.set_c(line.split_whitespace().last().unwrap().parse().unwrap());
        } else if line.starts_with("Program: ") {
            let program = line
                .split_whitespace()
                .skip(1)
                .next()
                .unwrap()
                .split(",")
                .collect::<Vec<&str>>();
            for i in (0..program.len()).step_by(2) {
                result
                    .add_to_program((program[i].parse().unwrap(), program[i + 1].parse().unwrap()));
            }
        }
    }

    result
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "Register A: 729
Register B: 0
Register C: 0

Program: 0,1,5,4,3,0",
        );

        assert_eq!("4,6,3,5,6,3,5,2,1,0", process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "Register A: 2024
Register B: 0
Register C: 0

Program: 0,3,5,4,3,0",
        );

        assert_eq!(117440, process_part_two(&input));
    }
}
