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
    let trees = get_trees(input);
    let mut visible = trees.len() * 2 + trees[0].len() * 2 - 4;

    for j in 1..(trees.len() - 1) {
        for i in 1..(trees[0].len() - 1) {            
            let height = trees[j][i];
            if visible_north(i, j, &height, &trees).1 {
                visible += 1;
            } else if visible_east(i, j, &height, &trees).1 {
                visible += 1;
            } else if visible_south(i, j, &height, &trees).1 {
                visible += 1;
            } else if visible_west(i, j, &height, &trees).1 {
                visible += 1;
            }
        }
    }

    visible
}

fn part_two(input: &String) -> usize {
    let trees = get_trees(input);
    let mut max = 0;

    for j in 0..trees.len() {
        for i in 0..trees[0].len() {
            let height = trees[j][i];
            let north = j - visible_north(i, j, &height,&trees).0;
            let east = visible_east(i, j, &height, &trees).0 - i;
            let south = visible_south(i, j, &height, &trees).0 - j;
            let west = i - visible_west(i, j, &height, &trees).0;

            let scenic_score = north * east * south * west;
            if scenic_score > max {
                max = scenic_score;
            }
        }
    }

    max
}

fn visible_north(i: usize, j: usize, height: &u8, trees: &Vec<Vec<u8>>) -> (usize, bool) {    
    let mut result = 0;
    let mut edge = true;
    for y in (0..j).rev() {
        if &trees[y][i] >= height {            
            result = y;
            edge = false;
            break;
        }
    }

    (result, edge)
}

fn visible_east(i: usize, j: usize, height: &u8, trees: &Vec<Vec<u8>>) -> (usize, bool) {    
    let mut result = trees[0].len() - 1;
    let mut edge = true;
    for x in (i + 1)..trees[0].len() {
        if &trees[j][x] >= height {
            result = x;
            edge = false;
            break;
        }
    }

    (result, edge)
}

fn visible_south(i: usize, j: usize, height: &u8, trees: &Vec<Vec<u8>>) -> (usize, bool) {    
    let mut result = trees.len() - 1;
    let mut edge = true;
    for y in (j + 1)..trees.len() {
        if &trees[y][i] >= height {
            result = y;
            edge = false;
            break;
        }
    }

    (result, edge)
}

fn visible_west(i: usize, j: usize, height: &u8, trees: &Vec<Vec<u8>>) -> (usize, bool) {        
    let mut result = 0;
    let mut edge = true;
    for x in (0..i).rev() {
        if &trees[j][x] >= height {
            result = x;
            edge = false;
            break;
        }
    }

    (result, edge)
}

fn get_trees(input: &String) -> Vec<Vec<u8>> {
    let mut result = Vec::new();

    for line in input.lines() {
        let row: Vec<u8> = line
            .chars()
            .map(|c| format!("{}", c).parse::<u8>().unwrap())
            .collect();
        result.push(row);
    }

    result
}
