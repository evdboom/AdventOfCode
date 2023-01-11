use std::collections::{HashSet, HashMap};
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

fn part_one(input: &String) -> usize {
    let mut elves = get_elves(input);

    for round in 0..10 {
        let start = round % 4;     
        move_elves(&mut elves, start);   
    }
    
    let mut left = usize::MAX;
    let mut right = 0;
    let mut top = usize::MAX;
    let mut bottom = 0;
    let mut no_elves = 0;

    for j in 0..elves.len() {
        for i in 0..elves[0].len() {
            if !elves[j][i] {
                continue;
            }
            no_elves += 1;
            left = left.min(i);
            right = right.max(i);
            top = top.min(j);
            bottom = bottom.max(j);
        }
    }

    left.abs_diff(right + 1) * top.abs_diff(bottom + 1) - no_elves
}

fn part_two(input: &String) -> i32 {
    let mut elves = get_elves(input);

    let mut rounds = 0;
    while move_elves(&mut elves, rounds % 4) {
        rounds += 1;
    }
    rounds + 1
}

fn move_elves(elves: &mut Vec<Vec<bool>>, start: i32) -> bool {    
    let mut moves = HashMap::new();
    expand_grid(elves);

    for j in 1..(elves.len() - 1) {
        for i in 1..(elves[0].len() - 1) {
            if !elves[j][i] {
                continue;
            }
            let neighbours = get_neighbours(i, j, &elves);
            if neighbours.is_empty() || neighbours.len() > 5 {
                continue;
            }

            let mut operation = start;
            loop {
                match operation {
                    0 =>{
                        if neighbours.iter().find(|neighbour| neighbour.1 == j - 1).is_none() {
                            if moves.insert((i, j - 1), (i, j)).is_some() {
                                moves.remove(&(i, j - 1));
                            }
                            break;
                        }
                    },
                    1 =>{
                        if neighbours.iter().find(|neighbour| neighbour.1 == j + 1).is_none() {
                            if moves.insert((i, j + 1), (i, j)).is_some() {
                                moves.remove(&(i, j + 1));
                            }
                            break;
                        }
                    },
                    2 =>{
                        if neighbours.iter().find(|neighbour| neighbour.0 == i - 1).is_none() {
                            if moves.insert((i - 1, j), (i, j)).is_some() {
                                moves.remove(&(i - 1, j));
                            }
                            break;
                        }
                    },
                    3 =>{
                        if neighbours.iter().find(|neighbour| neighbour.0 == i + 1).is_none() {
                            if moves.insert((i + 1, j), (i, j)).is_some() {
                                moves.remove(&(i + 1, j));
                            }
                            break;
                        }
                    },
                    _ => ()
                    
                }

                operation = (operation + 1) % 4;
                if operation == start {
                    break;
                }
            }

        }
    }

    if moves.is_empty() {
        return false;
    }

    for (new, old) in moves {
        elves[new.1][new.0] = true;
        elves[old.1][old.0] = false;
    }

    true
}

fn expand_grid(elves: &mut Vec<Vec<bool>>) {
    let mut left = false;
    let mut right = false;
    for row in elves.iter() {
        left = left || row[0];
        right = right || *row.last().unwrap();

        if left && right {
            break;
        }
    }

    if left || right {
        for row in elves.iter_mut() {
            if left {
                row.insert(0, false);
            }
            if right {
                row.push(false);
            }
        }
    }

    if elves[0].iter().find(|elf| **elf).is_some() {
        elves.insert(0, vec![false; elves[0].len()]);
    }
    if elves.last().unwrap().iter().find(|elf| **elf).is_some() {
        elves.push(vec![false; elves[0].len()]);
    }

}

fn get_neighbours(elf_x: usize, elf_y: usize, elves: &Vec<Vec<bool>>) -> HashSet<(usize, usize)> {
    let mut result = HashSet::new();
    if elves[elf_y - 1][elf_x - 1] {
        result.insert((elf_x - 1, elf_y - 1));
    }
    if elves[elf_y - 1][elf_x] {
        result.insert((elf_x, elf_y - 1));
    }
    if elves[elf_y - 1][elf_x + 1] {
        result.insert((elf_x + 1, elf_y - 1));
    }
    if elves[elf_y + 1][elf_x - 1] {
        result.insert((elf_x - 1, elf_y + 1));
    }
    if elves[elf_y + 1][elf_x] {
        result.insert((elf_x, elf_y + 1));
    }
    if elves[elf_y + 1][elf_x + 1] {
        result.insert((elf_x + 1, elf_y + 1));
    }
    if elves[elf_y][elf_x - 1] {
        result.insert((elf_x - 1, elf_y));
    }
    if elves[elf_y][elf_x + 1] {
        result.insert((elf_x + 1, elf_y));
    }

    result
}

fn get_elves(input: &String) -> Vec<Vec<bool>> {
    input
        .lines()
        .map(|line| line
            .chars()
            .map(|char| char == '#')
            .collect())
        .collect()
}