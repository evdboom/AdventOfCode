use std::collections::{HashMap, HashSet};
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
    let computers = get_computers(input);

    let mut three_link = HashSet::new();

    for computer in computers.keys() {
        let connections = computers.get(computer).unwrap();
        for connection in connections {
            let connection_connections: Vec<&Vec<u8>> = computers
                .get(connection)
                .unwrap()
                .iter()
                .filter(|c_c| connections.contains(c_c))
                .collect();
            for connection_connection in connection_connections {
                if computer[0] == b't' || connection[0] == b't' || connection_connection[0] == b't'
                {
                    let mut three = vec![computer.clone()];
                    three.push(connection.clone());
                    three.push(connection_connection.clone());
                    three.sort();
                    three_link.insert(three);
                }
            }
        }
    }

    three_link.len()
}

fn process_part_two(input: &str) -> String {
    let computers = get_computers(input);
    let mut cache = HashMap::new();

    for computer in computers.keys() {
        let mut clique = vec![computer.clone()];
        find_cliques(computer, &computers, &mut clique, &mut cache);
    }

    let mut largest_clique = cache
        .values()
        .max_by(|a, b| a.len().cmp(&b.len()))
        .unwrap()
        .clone();

    largest_clique.sort();

    largest_clique
        .into_iter()
        .map(|c| String::from_utf8(c).unwrap())
        .collect::<Vec<String>>()
        .join(",")
}

fn get_computers(input: &str) -> HashMap<Vec<u8>, Vec<Vec<u8>>> {
    let mut computers: HashMap<Vec<u8>, HashSet<Vec<u8>>> = HashMap::new();
    for line in input.lines() {
        let mut parts = line.split("-");
        let one = parts.next().unwrap().as_bytes().to_vec();
        let two = parts.next().unwrap().as_bytes().to_vec();

        let one_set = computers.entry(one.clone()).or_insert(HashSet::new());
        one_set.insert(two.clone());
        let two_set = computers.entry(two.clone()).or_insert(HashSet::new());
        two_set.insert(one.clone());
    }

    computers
        .into_iter()
        .map(|(key, value)| (key, value.into_iter().collect()))
        .collect()
}

fn find_cliques(
    computer: &Vec<u8>,
    computers: &HashMap<Vec<u8>, Vec<Vec<u8>>>,
    current_clique: &mut Vec<Vec<u8>>,
    cache: &mut HashMap<Vec<u8>, Vec<Vec<u8>>>,
) {
    let connections = computers.get(computer).unwrap();

    if let Some(entry) = cache.get(computer) {
        if entry.len() > connections.len() / 2 {
            return;
        }
    }

    for connection in connections {
        if current_clique
            .iter()
            .all(|c| computers.get(c).unwrap().contains(connection))
        {
            current_clique.push(connection.clone());
            find_cliques(connection, computers, current_clique, cache);
            current_clique.pop();
        }
    }

    let entry = cache.entry(computer.clone()).or_insert(vec![]);
    if current_clique.len() > entry.len() {
        *entry = current_clique.clone();
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part_one() {
        let input = String::from(
            "kh-tc
qp-kh
de-cg
ka-co
yn-aq
qp-ub
cg-tb
vc-aq
tb-ka
wh-tc
yn-cg
kh-ub
ta-co
de-co
tc-td
tb-wq
wh-td
ta-ka
td-qp
aq-cg
wq-ub
ub-vc
de-ta
wq-aq
wq-vc
wh-yn
ka-de
kh-ta
co-tc
wh-qp
tb-vc
td-yn",
        );

        assert_eq!(7, process_part_one(&input));
    }

    #[test]
    fn part_two() {
        let input = String::from(
            "kh-tc
qp-kh
de-cg
ka-co
yn-aq
qp-ub
cg-tb
vc-aq
tb-ka
wh-tc
yn-cg
kh-ub
ta-co
de-co
tc-td
tb-wq
wh-td
ta-ka
td-qp
aq-cg
wq-ub
ub-vc
de-ta
wq-aq
wq-vc
wh-yn
ka-de
kh-ta
co-tc
wh-qp
tb-vc
td-yn",
        );

        assert_eq!("co,de,ka,ta", process_part_two(&input));
    }
}
