use std::collections::HashSet;
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
    let sensors = get_sensors(input);
    let requested_row = 2000000;
    let mut no_beacon = vec![];
    let mut beacon_on_row = HashSet::new();
    for sensor in sensors {
        let distance = (sensor[0].abs_diff(sensor[2]) + sensor[1].abs_diff(sensor[3])) as isize;
        let distance_to_row = sensor[1].abs_diff(requested_row) as isize;
        if distance_to_row > distance {
            continue;
        }        
        no_beacon.push((sensor[0] - distance + distance_to_row, sensor[0] + distance - distance_to_row));
        if sensor[3] == requested_row {
            beacon_on_row.insert(sensor[2]);
        }
        
    }

    no_beacon.sort_unstable_by(|a,b| a.0.cmp(&b.0));
    
    let mut index = 0;
    while index < no_beacon.len() - 1 {        
        if no_beacon[index].1 >= no_beacon[index + 1].0 {
            no_beacon[index].1 = no_beacon[index + 1].1.max(no_beacon[index].1);
            no_beacon.remove(index + 1);
        } else {
            index += 1;
        }
    }

    no_beacon.into_iter().fold(0, |sum, range| 1 + sum + range.0.abs_diff(range.1)) - beacon_on_row.len() 
}

fn part_two(input: &String) -> isize {
    let sensors = get_sensors(input).iter().map(|sensor| (sensor[0], sensor[1],(sensor[0].abs_diff(sensor[2]) + sensor[1].abs_diff(sensor[3])) as isize)).collect::<Vec<(isize, isize, isize)>>();    
    let mut pairs = vec![];
    let mut checked= HashSet::new();
    for sensor in sensors.iter() {
        checked.insert((sensor.0, sensor.1));
        for other in sensors.iter() {
            if checked.contains(&(other.0, other.1)) {
                continue;
            }                        
            let distance = (sensor.0.abs_diff(other.0) + sensor.1.abs_diff(other.1)) as isize;
            let dif = distance - sensor.2 - other.2;
            if dif == 2 {                
                pairs.push((sensor, other));
            }
        }
    }    

    let work = pairs.iter().fold((0,0,isize::MAX) ,|min, pair| {
        if pair.0.2 < pair.1.2 && pair.0.2 < min.2 {
            (pair.0.0, pair.0.1, pair.0.2)
        } else if pair.1.2 < min.2 {
            (pair.1.0, pair.1.1, pair.1.2)
        } else {
            min
        }
    } );

    for i in 0..=(work.2 + 1) {
        let x1 = work.0 - i;
        let x2 = work.0 + i;
        let y1 = work.1 - (work.2 + 1) + i;
        let y2 = work.1 + (work.2 + 1) - i;

        for point in [(x1,y1), (x1,y2), (x2,y1), (x2,y2)] {
            if pairs[0].0.0.abs_diff(point.0) + pairs[0].0.1.abs_diff(point.1) == pairs[0].0.2 as usize + 1 
            && pairs[0].1.0.abs_diff(point.0) + pairs[0].1.1.abs_diff(point.1) == pairs[0].1.2 as usize + 1 
            && pairs[1].0.0.abs_diff(point.0) + pairs[1].0.1.abs_diff(point.1) == pairs[1].0.2 as usize + 1 
            && pairs[1].1.0.abs_diff(point.0) + pairs[1].1.1.abs_diff(point.1) == pairs[1].1.2 as usize + 1 {
                return point.0 * 4000000 + point.1;                        
            }
        }
    }
    panic!("no point found!");
}

fn get_sensors(input: &String) -> Vec<Vec<isize>> {
    input
        .lines()
        .map(|line| line.replace("Sensor at x=", "").replace(": closest beacon is at x=", ",").replace(", y=", ",").split(",").map(|v| v.parse().unwrap()).collect()).collect()

}