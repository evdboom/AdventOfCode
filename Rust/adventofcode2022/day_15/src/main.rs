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

    let (sensor_1, sensor_2) = pairs[0];
    let (sensor_3, sensor_4) = pairs[1];

    let mut up = true;
    let mut start_1 = if sensor_1.1 > sensor_2.1 { sensor_1.1 - sensor_1.2 - 1 } else { sensor_1.1 + sensor_1.2 + 1 };
    if sensor_1.0 > sensor_2.0 {
        start_1 -= sensor_1.0;
    } else {
        start_1 += sensor_1.0;
        up = false;
    }
    let mut start_2 = if sensor_3.1 > sensor_4.1 { sensor_3.1 - sensor_3.2 - 1 } else { sensor_3.1 + sensor_3.2 + 1 };
    if sensor_3.0 > sensor_4.0 {
        start_2 -= sensor_3.0;
    } else {
        start_2 += sensor_3.0;
    }
    let x = if up { (start_2 - start_1) / 2 } else { (start_1 - start_2) / 2 };
    let y = if up { start_1 + x } else { start_1 - x };

    x * 4000000 + y
}

fn get_sensors(input: &String) -> Vec<Vec<isize>> {
    input
        .lines()
        .map(|line| line.replace("Sensor at x=", "").replace(": closest beacon is at x=", ",").replace(", y=", ",").split(",").map(|v| v.parse().unwrap()).collect()).collect()

}