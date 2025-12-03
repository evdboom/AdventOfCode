pub fn solve(input: &str, part: u8) -> usize {
   match part {
       1 => part_one(input),
       2 => part_two(input),
       _ => panic!("Invalid part: {}", part),
   }
}

fn part_one(input: &str) -> usize {
    let ranges = parse_input(input);
    let parts: Vec<usize> = [2].to_vec();
    ranges.iter().map(|(start, end)| sum_equal_splits_in_range_with_parts(*start, *end, &parts)).sum()
}

fn part_two(input: &str) -> usize {
    let ranges = parse_input(input);
    ranges.iter().map(|(start, end)| sum_equal_splits_in_range(*start, *end)).sum()
}

fn sum_equal_splits_in_range(start: usize, end: usize) -> usize {
    (start..=end)
        .filter(|&id| {
            let s = id.to_string();
            sum_equal_splits_in_range_with_parts_inner(&s, &splittable_parts(&s))
        })
        .sum()
}

fn sum_equal_splits_in_range_with_parts(start: usize, end: usize, parts: &[usize]) -> usize {
    (start..=end)
        .filter(|&id| {
            let s = id.to_string();
            sum_equal_splits_in_range_with_parts_inner(&s, parts)
        })
        .sum()
}

fn sum_equal_splits_in_range_with_parts_inner(s: &str, parts: &[usize]) -> bool {
    let len = s.len();
    let result = parts.iter().any(|&p| {
        if len % p != 0 {
            return false;
        }
        let chunk = len / p;
        let first = &s[..chunk];

        let all_equal = (1..p).all(|i| {
            let start = i * chunk;
            let end = (i + 1) * chunk;
            let cur = &s[start..end];
            let equal = cur == first;
            equal
        });

        all_equal
    });

    result
}

fn parse_input(input: &str) -> Vec<(usize, usize)> {
    let mut pairs = Vec::new();
    for chunk in input.trim().split(',').filter(|s| !s.is_empty()) {
        let mut parts = chunk.split('-');
        if let (Some(a), Some(b)) = (parts.next(), parts.next()) {
            if let (Ok(x), Ok(y)) = (a.trim().parse::<usize>(), b.trim().parse::<usize>()) {
                pairs.push((x, y));
            }
        }
    }
    pairs
}

/// Returns every way the provided digit string can be split into equal-sized parts.
///
/// The output is the set of part counts greater than one that evenly divide the
/// length of the cleaned input. Each value represents "how many pieces" the
/// string can be cut into. The input itself is trimmed so surrounding whitespace
/// does not affect the calculation.
#[allow(dead_code)]
pub fn splittable_parts(number: &str) -> Vec<usize> {
    let length = number.trim().len();
    if length <= 1 {
        return Vec::new();
    }

    let mut parts = Vec::new();
    let mut candidate = 2;
    while candidate * candidate <= length {
        if length % candidate == 0 {
            parts.push(candidate);
            let paired = length / candidate;
            if paired != candidate {
                parts.push(paired);
            }
        }
        candidate += 1;
    }

    parts.push(length);
    parts.sort_unstable();
    parts
}

#[cfg(test)]
mod tests {
    use super::splittable_parts;

    #[test]
    fn tiny_numbers() {
        assert_eq!(splittable_parts("11"), vec![2]);
        assert_eq!(splittable_parts("8"), Vec::<usize>::new());
    }

    #[test]
    fn composite_lengths() {
        assert_eq!(splittable_parts("123123"), vec![2, 3, 6]);
        assert_eq!(
            splittable_parts("145567145567145567"),
            vec![2, 3, 6, 9, 18]
        );
    }

    #[test]
    fn primes_only_split_into_single_chars() {
        assert_eq!(splittable_parts("1111111"), vec![7]);
    }
}
