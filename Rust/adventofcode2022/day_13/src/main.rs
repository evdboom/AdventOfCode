use std::cmp::Ordering;
use std::fs;
use std::time::Instant;

#[derive(Eq)]
struct Packet {
    value: Option<u32>,
    inner_packets: Vec<Packet>,
}

impl Packet {
    fn add_child(&mut self, value: Option<u32>) -> &mut Self {
        self.inner_packets.push(Packet {
            value: value,
            inner_packets: vec![],
        });
        self.inner_packets.last_mut().unwrap()
    }

    fn last_at_depth(&mut self, depth: usize) -> &mut Self {
        if depth == 0 {
            self
        } else {
            self.inner_packets
                .last_mut()
                .unwrap()
                .last_at_depth(depth - 1)
        }
    }
}

impl PartialEq for Packet {
    fn eq(&self, other: &Self) -> bool {
        self.cmp(other) == Ordering::Equal
    }

    fn ne(&self, other: &Self) -> bool {
        !self.eq(other)
    }
}

impl PartialOrd for Packet {
    fn ge(&self, other: &Self) -> bool {
        let cmp = self.cmp(other);
        cmp == Ordering::Equal || cmp == Ordering::Greater
    }
    fn gt(&self, other: &Self) -> bool {
        self.cmp(other) == Ordering::Greater
    }
    fn le(&self, other: &Self) -> bool {
        let cmp = self.cmp(other);
        cmp == Ordering::Equal || cmp == Ordering::Less
    }
    fn lt(&self, other: &Self) -> bool {
        self.cmp(other) == Ordering::Less
    }
    fn partial_cmp(&self, other: &Self) -> Option<std::cmp::Ordering> {
        Some(self.cmp(other))
    }
}

impl Ord for Packet {
    fn cmp(&self, other: &Self) -> Ordering {
        if self.value.is_some() && other.value.is_some() {
            let diff = self.value.unwrap() as i32 - other.value.unwrap() as i32;
            if diff < 0 {
                return Ordering::Less;
            } else if diff == 0 {
                return Ordering::Equal;
            } else {
                return Ordering::Greater;
            }
        }

        let tmp_self = build_packet(self.value, 1);
        let tmp_other = build_packet(other.value, 1);
        let cmp_self = if self.value.is_some() {
            &tmp_self
        } else {
            self
        };
        let cmp_other = if other.value.is_some() {
            &tmp_other
        } else {
            other
        };

        let min = cmp_self
            .inner_packets
            .len()
            .min(cmp_other.inner_packets.len());
        for i in 0..min {
            let cmp = cmp_self.inner_packets[i].cmp(&cmp_other.inner_packets[i]);
            if cmp != Ordering::Equal {
                return cmp;
            }
        }
        let len_diff =
            cmp_self.inner_packets.len() as isize - cmp_other.inner_packets.len() as isize;
        if len_diff < 0 {
            Ordering::Less
        } else if len_diff == 0 {
            Ordering::Equal
        } else {
            Ordering::Greater
        }
    }
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

fn part_one(input: &String) -> usize {
    let mut valid = 0;
    let packets = parse_packets(input);
    for i in (0..packets.len()).step_by(2) {
        if packets[i] < packets[i + 1] {
            valid += i / 2 + 1;
        }
    }
    valid
}

fn part_two(input: &String) -> usize {
    let packets = parse_packets(input);
    let two = build_packet(Some(2), 2);
    let six = build_packet(Some(6), 2);

    let mut borrowed: Vec<&Packet> = packets.iter().collect();
    borrowed.push(&two);
    borrowed.push(&six);

    borrowed.sort();

    (borrowed
        .iter()
        .position(|packet| (*packet).eq(&two))
        .unwrap()
        + 1)
        * (borrowed
            .iter()
            .position(|packet| (*packet).eq(&six))
            .unwrap()
            + 1)
}

fn build_packet(value: Option<u32>, depth: usize) -> Packet {
    if depth == 0 {
        Packet {
            value: value,
            inner_packets: vec![],
        }
    } else {
        Packet {
            value: None,
            inner_packets: vec![build_packet(value, depth - 1)],
        }
    }
}

fn parse_packets(input: &String) -> Vec<Packet> {
    input
        .lines()
        .filter(|line| !line.is_empty())
        .map(|line| parse_packet(line))
        .collect()
}

fn parse_packet(line: &str) -> Packet {
    let mut value = vec![];
    let mut first = Packet {
        value: None,
        inner_packets: vec![],
    };
    let mut current = &mut first;
    let mut depth = 0;

    let mut chars = line.chars();
    while let Some(c) = chars.nth(0) {
        match c {
            '[' => {
                depth += 1;
                current = current.add_child(None);
            }
            ']' => {
                if !value.is_empty() {
                    current.add_child(Some(
                        value.iter().collect::<String>().parse::<u32>().unwrap(),
                    ));
                    value.clear();
                }
                depth -= 1;
                current = first.last_at_depth(depth);
            }
            ',' => {
                if !value.is_empty() {
                    current.add_child(Some(
                        value.iter().collect::<String>().parse::<u32>().unwrap(),
                    ));
                    value.clear();
                }
            }
            _ => {
                value.push(c);
            }
        }
    }

    first
}

// for (int i = 0; i < line.Length; i++)
//             {
//                 var c = line[i];
//                 switch (c)
//                 {
//                     case '[':
//                         var inner = new Packet { Parent = current };
//                         if (result is null)
//                         {
//                             result = inner;
//                         }
//                         if (current is not null)
//                         {
//                             current.InnerPackets.Add(inner);
//                         }
//                         current = inner;
//                         break;
//                     case ']':
//                         current = current!.Parent;
//                         break;
//                     case ',':
//                         continue;
//                     default:
//                         var next = line[i + 1];
//                         if (next != ',' && next != ']')
//                         {
//                             current!.InnerPackets.Add(new Packet { Parent = current, Value = int.Parse($"{c}{next}") });
//                             i++;

//                         }
//                         else
//                         {
//                             current!.InnerPackets.Add(new Packet { Parent = current, Value = int.Parse($"{c}") });
//                         }
//                         break;
//                 }
//             }
//             return result;
