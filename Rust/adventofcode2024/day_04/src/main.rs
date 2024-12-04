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

fn process_part_one(input: &str) -> u32 {
    let grid = get_grid(input);
    let width = grid[0].len();
    let height = grid.len();
    let mut count: u32 = 0;
    for j in 0..=(height - 1) {
        for i in 0..=(width - 1) {
            let c = grid[j][i];
            if c == 'X' {
                count += find_words(&grid, i, j);
            }
        }
    }

    count
}

fn process_part_two(input: &str) -> u32 {
    let grid = get_grid(input);
    let width = grid[0].len();
    let height = grid.len();
    let mut count: u32 = 0;
    for j in 0..=(height - 1) {
        for i in 0..=(width - 1) {
            let c = grid[j][i];
            if c == 'A' && is_cross(&grid, i, j) {
                count += 1;
            }
        }
    }

    count
}

fn get_grid(input: &str) -> Vec<Vec<char>> {
    let mut grid = Vec::new();
    for line in input.lines() {
        let mut row = Vec::new();
        for c in line.chars() {
            row.push(c);
        }
        grid.push(row);
    }
    grid
}

fn is_cross(grid: &Vec<Vec<char>>, i: usize, j: usize) -> bool {
    let width = grid[0].len();
    let height = grid.len();
    let mut backslash = false;
    if let Some((ulx, ulu)) = up_left(i, j, width, height) {
        if let Some((drx, dry)) = down_right(i, j, width, height) {
            if (grid[ulu][ulx] == 'M' && grid[dry][drx] == 'S')
                || (grid[ulu][ulx] == 'S' && grid[dry][drx] == 'M')
            {
                backslash = true;
            }
        }
    }
    let mut slash = false;
    if let Some((urx, ury)) = up_right(i, j, width, height) {
        if let Some((dlx, dly)) = down_left(i, j, width, height) {
            if (grid[ury][urx] == 'M' && grid[dly][dlx] == 'S')
                || (grid[ury][urx] == 'S' && grid[dly][dlx] == 'M')
            {
                slash = true;
            }
        }
    }

    backslash && slash
}

fn find_words(grid: &Vec<Vec<char>>, i: usize, j: usize) -> u32 {
    let mut count: u32 = 0;

    if check_direction(grid, i, j, left) {
        count += 1;
    }
    if check_direction(grid, i, j, right) {
        count += 1;
    }
    if check_direction(grid, i, j, up) {
        count += 1;
    }
    if check_direction(grid, i, j, down) {
        count += 1;
    }
    if check_direction(grid, i, j, up_left) {
        count += 1;
    }
    if check_direction(grid, i, j, up_right) {
        count += 1;
    }
    if check_direction(grid, i, j, down_left) {
        count += 1;
    }
    if check_direction(grid, i, j, down_right) {
        count += 1;
    }

    count
}

fn check_direction(
    grid: &Vec<Vec<char>>,
    i: usize,
    j: usize,
    direction: fn(usize, usize, usize, usize) -> Option<(usize, usize)>,
) -> bool {
    if let Some((li, lj)) = direction(i, j, grid[0].len(), grid.len()) {
        if grid[lj][li] != 'M' {
            return false;
        }
        if let Some((li2, lj2)) = direction(li, lj, grid[0].len(), grid.len()) {
            if grid[lj2][li2] != 'A' {
                return false;
            }
            if let Some((li3, lj3)) = direction(li2, lj2, grid[0].len(), grid.len()) {
                return grid[lj3][li3] == 'S';
            }
        }
    }
    false
}

fn left(i: usize, j: usize, _width: usize, _height: usize) -> Option<(usize, usize)> {
    if i > 0 {
        Some((i - 1, j))
    } else {
        None
    }
}

fn right(i: usize, j: usize, width: usize, _height: usize) -> Option<(usize, usize)> {
    if i + 1 < width {
        Some((i + 1, j))
    } else {
        None
    }
}

fn up(i: usize, j: usize, _width: usize, _height: usize) -> Option<(usize, usize)> {
    if j > 0 {
        Some((i, j - 1))
    } else {
        None
    }
}

fn down(i: usize, j: usize, _width: usize, height: usize) -> Option<(usize, usize)> {
    if j + 1 < height {
        Some((i, j + 1))
    } else {
        None
    }
}

fn up_left(i: usize, j: usize, _width: usize, _height: usize) -> Option<(usize, usize)> {
    if i > 0 && j > 0 {
        Some((i - 1, j - 1))
    } else {
        None
    }
}

fn up_right(i: usize, j: usize, width: usize, _height: usize) -> Option<(usize, usize)> {
    if i + 1 < width && j > 0 {
        Some((i + 1, j - 1))
    } else {
        None
    }
}

fn down_left(i: usize, j: usize, _width: usize, height: usize) -> Option<(usize, usize)> {
    if i > 0 && j + 1 < height {
        Some((i - 1, j + 1))
    } else {
        None
    }
}

fn down_right(i: usize, j: usize, width: usize, height: usize) -> Option<(usize, usize)> {
    if i + 1 < width && j + 1 < height {
        Some((i + 1, j + 1))
    } else {
        None
    }
}
