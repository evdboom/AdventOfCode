use std::collections::HashSet;
use std::fs;
use std::time::Instant;

fn main() {
    let input = fs::read_to_string("./input.txt").expect("Could not read file");

    let start = Instant::now();
    let part_one = get_tail_movements(&input, 2);
    let duration_one = start.elapsed();
    let part_two = get_tail_movements(&input, 10);
    let duration_two = start.elapsed() - duration_one;
    println!("Part one: {}, took: {:?}", part_one, duration_one);
    println!("Part two: {}, took: {:?}", part_two, duration_two);
}

fn get_tail_movements(input: &String, knot_size: usize) -> i32 {
    let mut tail_points = HashSet::new();
    let mut locations = vec![(0,0); knot_size];
    tail_points.insert((0, 0));
    let mut points = 1;

    for line in input.lines() {
        let mut parts = line.split_whitespace();
        let direction = parts.next().unwrap();
        let amount = parts.next().unwrap().parse::<usize>().unwrap();
        let mut counter = 0;
        while counter < amount {
            match direction {
                "R" => locations[0].0 += 1,
                "L" => locations[0].0 -= 1,
                "U" => locations[0].1 -= 1,
                "D" => locations[0].1 += 1,
                &_ => panic!("Unknown direction")
            }

            for i in 1..locations.len() {
                let new_location = move_tail(locations[i-1], locations[i]);
                if new_location == locations[i] {
                    break;
                }
                if i == locations.len() - 1 && !tail_points.contains(&new_location) {
                    tail_points.insert(new_location);
                    points += 1;                    
                } 
                locations[i] = new_location;
            }

            counter += 1;            
        }

    }

    points
}

fn move_tail(head: (i32, i32), tail: (i32, i32)) -> (i32, i32) {
    let x = head.0 - tail.0;
    let y = head.1 - tail.1;

    let move_x = x / 2;
    let move_y = y / 2;
    if move_x != 0 {
        let mut add_y  = 0;       
        if y < 0 {            
            add_y = -1;
        } else if y > 0 {
            add_y = 1;
        }
                
        (tail.0 + move_x, tail.1 + add_y)
    } else if move_y != 0 {
        let mut add_x  = 0;
        if x < 0 {            
            add_x = -1;
        } else if x > 0 {
            add_x = 1;
        }
                
        (tail.0 + add_x, tail.1 + move_y)
    } else {
        tail
    }
}
        