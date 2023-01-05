use std::fs;
use std::time::Instant;

struct MixPart {
    value: isize,
    index: usize,
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

fn part_one(input: &String) -> isize {
    let mut sequence = get_sequence(input, 1);
    decode_round(&mut sequence);
    get_result(sequence)
}

fn part_two(input: &String) -> isize {
    let mut sequence = get_sequence(input, 811589153);
    for _ in 0..10 {
        decode_round(&mut sequence);
    }

    get_result(sequence)
}

fn decode_round(sequence: &mut Vec<MixPart>) {
    for i in 0..sequence.len() {
        let index = sequence
            .iter()
            .position(|mix_part| mix_part.index == i)
            .unwrap();
        let reference = sequence.remove(index);
        let new_index = (index as isize + reference.value).rem_euclid(sequence.len() as isize);
        sequence.insert(new_index as usize, reference);
    }
}

fn get_result(sequence: Vec<MixPart>) -> isize {
    let index_zero = sequence
        .iter()
        .position(|mix_part| mix_part.value == 0)
        .unwrap();
    sequence[(index_zero + 1000) % sequence.len()].value
        + sequence[(index_zero + 2000) % sequence.len()].value
        + sequence[(index_zero + 3000) % sequence.len()].value
}

fn get_sequence(input: &String, factor: isize) -> Vec<MixPart> {
    input
        .lines()
        .enumerate()
        .map(|(index, value)| MixPart {
            value: value.parse::<isize>().unwrap() * factor,
            index: index,
        })
        .collect()
}
