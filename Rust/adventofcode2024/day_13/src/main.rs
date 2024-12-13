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
    let mut lines = input.lines();
    let mut result = 0;
    loop {
        let a: Vec<f64> = lines.next().unwrap()[9..]
            .split(", ")
            .map(|s| s[2..].parse().unwrap())
            .collect();
        let b: Vec<f64> = lines.next().unwrap()[9..]
            .split(", ")
            .map(|s| s[2..].parse().unwrap())
            .collect();
        let prize: Vec<f64> = lines.next().unwrap()[7..]
            .split(", ")
            .map(|s| s[2..].parse().unwrap())
            .collect();

        result += find_mix_to_price((a[0], a[1]), (b[0], b[1]), (prize[0], prize[1]), Some(100));

        if !lines.next().is_some() {
            break;
        }
    }
    result
}

fn process_part_two(input: &str) -> usize {
    let mut lines = input.lines();
    let mut result = 0;
    loop {
        let a: Vec<f64> = lines.next().unwrap()[9..]
            .split(", ")
            .map(|s| s[2..].parse().unwrap())
            .collect();
        let b: Vec<f64> = lines.next().unwrap()[9..]
            .split(", ")
            .map(|s| s[2..].parse().unwrap())
            .collect();
        let prize: Vec<f64> = lines.next().unwrap()[7..]
            .split(", ")
            .map(|s| s[2..].parse().unwrap())
            .collect();

        result += find_mix_to_price(
            (a[0], a[1]),
            (b[0], b[1]),
            (10000000000000f64 + prize[0], 10000000000000f64 + prize[1]),
            None,
        );

        if !lines.next().is_some() {
            break;
        }
    }
    result
}

fn find_mix_to_price(
    button_a: (f64, f64),
    button_b: (f64, f64),
    prize: (f64, f64),
    max_button_presses: Option<usize>,
) -> usize {
    let b = (prize.1 - (prize.0 * button_a.1) / button_a.0)
        / (button_b.1 - (button_b.0 * button_a.1) / button_a.0);
    let a = (prize.0 - button_b.0 * b) / button_a.0;

    let a_pressed = a.round() as usize;
    let b_pressed = b.round() as usize;

    if button_a.0 as usize * a_pressed + button_b.0 as usize * b_pressed != prize.0 as usize
        || button_a.1 as usize * a_pressed + button_b.1 as usize * b_pressed != prize.1 as usize
    {
        return 0;
    } else if let Some(max) = max_button_presses {
        if a_pressed <= max && b_pressed <= max {
            return a_pressed * 3 + b_pressed;
        } else {
            return 0;
        }
    } else {
        return a_pressed * 3 + b_pressed;
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279",
        );

        assert_eq!(480, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279",
        );

        assert_eq!(875318608908, process_part_two(&input));
    }
}
