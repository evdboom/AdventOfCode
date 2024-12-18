// src/grid.rs

use std::fmt::Display;
use std::hash::{self, Hash};
use std::ops::Add;

#[derive(Clone, Debug, Eq, PartialEq)]
pub enum Direction {
    Up(Point),
    Down(Point),
    Left(Point),
    Right(Point),
}

impl Hash for Direction {
    fn hash<H: hash::Hasher>(&self, state: &mut H) {
        match self {
            Direction::Up(point) => {
                "Up".hash(state);
                point.hash(state);
            }
            Direction::Down(point) => {
                "Down".hash(state);
                point.hash(state);
            }
            Direction::Left(point) => {
                "Left".hash(state);
                point.hash(state);
            }
            Direction::Right(point) => {
                "Right".hash(state);
                point.hash(state);
            }
        }
    }
}

impl Display for Direction {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        let direction = match self {
            Direction::Up(_) => "Up",
            Direction::Down(_) => "Down",
            Direction::Left(_) => "Left",
            Direction::Right(_) => "Right",
        };
        write!(f, "{}", direction)
    }
}

impl Direction {
    pub fn point(&self) -> Point {
        match self {
            Direction::Up(point) => *point,
            Direction::Down(point) => *point,
            Direction::Left(point) => *point,
            Direction::Right(point) => *point,
        }
    }

    pub fn from_direction(direction: &Direction, point: Point) -> Self {
        match direction {
            Direction::Up(_) => Direction::Up(point),
            Direction::Down(_) => Direction::Down(point),
            Direction::Left(_) => Direction::Left(point),
            Direction::Right(_) => Direction::Right(point),
        }
    }

    pub fn same_direction(&self, other: &Direction) -> bool {
        match (self, other) {
            (Direction::Up(_), Direction::Up(_)) => true,
            (Direction::Down(_), Direction::Down(_)) => true,
            (Direction::Left(_), Direction::Left(_)) => true,
            (Direction::Right(_), Direction::Right(_)) => true,
            _ => false,
        }
    }

    pub fn in_same_direction(&self, point: Point) -> Self {
        match self {
            Direction::Up(_) => Direction::Up(point),
            Direction::Down(_) => Direction::Down(point),
            Direction::Left(_) => Direction::Left(point),
            Direction::Right(_) => Direction::Right(point),
        }
    }

    pub fn up(point: Point) -> Self {
        Direction::Up(point)
    }

    pub fn down(point: Point) -> Self {
        Direction::Down(point)
    }

    pub fn left(point: Point) -> Self {
        Direction::Left(point)
    }

    pub fn right(point: Point) -> Self {
        Direction::Right(point)
    }
}

#[derive(Clone, Copy, Debug, Eq)]
pub struct Point {
    pub x: usize,
    pub y: usize,
}

impl Display for Point {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "({}, {})", self.x, self.y)
    }
}

impl Hash for Point {
    fn hash<H: hash::Hasher>(&self, state: &mut H) {
        self.x.hash(state);
        self.y.hash(state);
    }
}

impl Add for Point {
    type Output = Point;

    fn add(self, other: Point) -> Point {
        Point {
            x: self.x + other.x,
            y: self.y + other.y,
        }
    }
}

impl PartialEq for Point {
    fn eq(&self, other: &Self) -> bool {
        self.x == other.x && self.y == other.y
    }
}

impl Point {
    pub fn new(x: usize, y: usize) -> Self {
        Point { x, y }
    }

    pub fn manhattan_distance(&self, other: &Point) -> usize {
        self.x.abs_diff(other.x) + self.y.abs_diff(other.y)
    }

    pub fn is_adjacent(&self, other: &Point) -> bool {
        self.left() == Some(*other)
            || self.right() == Some(*other)
            || self.up() == Some(*other)
            || self.down() == Some(*other)
    }

    pub fn left(&self) -> Option<Point> {
        if self.x == 0 {
            None
        } else {
            Some(Point {
                x: self.x - 1,
                y: self.y,
            })
        }
    }

    pub fn right(&self) -> Option<Point> {
        Some(Point {
            x: self.x + 1,
            y: self.y,
        })
    }

    pub fn up(&self) -> Option<Point> {
        if self.y == 0 {
            None
        } else {
            Some(Point {
                x: self.x,
                y: self.y - 1,
            })
        }
    }

    pub fn down(&self) -> Option<Point> {
        Some(Point {
            x: self.x,
            y: self.y + 1,
        })
    }

    pub fn up_left(&self) -> Option<Point> {
        self.up().and_then(|up| up.left())
    }

    pub fn up_right(&self) -> Option<Point> {
        self.up().and_then(|up| up.right())
    }

    pub fn down_left(&self) -> Option<Point> {
        self.down().and_then(|down| down.left())
    }

    pub fn down_right(&self) -> Option<Point> {
        self.down().and_then(|down| down.right())
    }
}

#[derive(Clone, Debug)]
pub struct GridCell<T> {
    pub point: Point,
    pub value: T,
}

pub struct Grid<T> {
    grid: Vec<Vec<T>>,
    width: usize,
    height: usize,
}

pub struct GridIterator<'a, T> {
    grid: &'a Grid<T>,
    current_point: Point,
}

impl<'a, T> Iterator for GridIterator<'a, T>
where
    T: Clone,
{
    type Item = GridCell<T>;

    fn next(&mut self) -> Option<Self::Item> {
        if self.current_point.y >= self.grid.height() {
            return None;
        }

        let cell = self
            .grid
            .get(&self.current_point)
            .cloned()
            .map(|value| GridCell {
                point: self.current_point,
                value,
            });

        if self.current_point.x < self.grid.max_x() {
            self.current_point.x += 1;
        } else {
            self.current_point.x = 0;
            self.current_point.y += 1;
        }

        cell
    }
}

impl<T> Grid<T> {
    pub fn new(width: usize, height: usize, default_value: T) -> Self
    where
        T: Clone,
    {
        let grid = vec![vec![default_value; width]; height];
        Grid {
            grid,
            width,
            height,
        }
    }

    pub fn from_string(input: &str, parser: fn(char) -> T) -> Self
    where
        T: Clone,
    {
        let mut grid = Vec::new();
        let mut width = 0;

        for line in input.lines() {
            let row: Vec<T> = line.chars().map(|c| parser(c)).collect();

            if width == 0 {
                width = row.len();
            }

            grid.push(row);
        }
        let height = grid.len();
        Grid {
            grid,
            width,
            height,
        }
    }

    pub fn get(&self, point: &Point) -> Option<&T> {
        self.grid.get(point.y).and_then(|row| row.get(point.x))
    }

    pub fn set(&mut self, point: Point, value: T) {
        if let Some(row) = self.grid.get_mut(point.y) {
            if let Some(cell) = row.get_mut(point.x) {
                *cell = value;
            }
        }
    }

    pub fn get_row(&self, y: usize) -> Option<&Vec<T>> {
        self.grid.get(y)
    }

    pub fn get_column(&self, x: usize) -> Option<Vec<&T>> {
        if x >= self.width {
            return None;
        }

        let column: Vec<&T> = self.grid.iter().map(|row| &row[x]).collect();
        Some(column)
    }

    pub fn get_filtered_adjacent(&self, point: &Point, filter: fn(&T) -> bool) -> Vec<Point> {
        self.get_filtered_directions(point, filter)
            .iter()
            .map(|d| d.point())
            .collect()
    }

    pub fn get_filtered_directions(&self, point: &Point, filter: fn(&T) -> bool) -> Vec<Direction> {
        let mut adjacent = Vec::new();

        if let Some(left) = point.left() {
            if let Some(value) = self.get(&left) {
                if filter(value) {
                    adjacent.push(Direction::left(left));
                }
            }
        }

        if let Some(right) = point.right() {
            if let Some(value) = self.get(&right) {
                if filter(value) {
                    adjacent.push(Direction::right(right));
                }
            }
        }

        if let Some(up) = point.up() {
            if let Some(value) = self.get(&up) {
                if filter(value) {
                    adjacent.push(Direction::up(up));
                }
            }
        }

        if let Some(down) = point.down() {
            if let Some(value) = self.get(&down) {
                if filter(value) {
                    adjacent.push(Direction::down(down));
                }
            }
        }

        adjacent
    }

    pub fn get_adjacent_with_direction(&self, point: &Point) -> Vec<Direction> {
        let mut adjacent = Vec::new();

        if let Some(left) = point.left() {
            if self.get(&left).is_some() {
                adjacent.push(Direction::left(left));
            }
        }

        if let Some(right) = point.right() {
            if self.get(&right).is_some() {
                adjacent.push(Direction::right(right));
            }
        }

        if let Some(up) = point.up() {
            if self.get(&up).is_some() {
                adjacent.push(Direction::up(up));
            }
        }

        if let Some(down) = point.down() {
            if self.get(&down).is_some() {
                adjacent.push(Direction::down(down));
            }
        }

        adjacent
    }

    pub fn get_adjacent(&self, point: &Point) -> Vec<Point> {
        self.get_adjacent_with_direction(point)
            .iter()
            .map(|d| d.point())
            .collect()
    }

    pub fn width(&self) -> usize {
        self.width
    }

    pub fn height(&self) -> usize {
        self.height
    }

    pub fn size(&self) -> usize {
        self.width * self.height
    }

    pub fn max_x(&self) -> usize {
        self.width - 1
    }

    pub fn max_y(&self) -> usize {
        self.height - 1
    }

    pub fn iter(&self) -> GridIterator<T> {
        GridIterator {
            grid: self,
            current_point: Point { x: 0, y: 0 },
        }
    }

    pub fn print(&self, display: fn(&T) -> char) {
        for row in &self.grid {
            for cell in row {
                print!("{}", display(cell));
            }
            println!();
        }
    }
}
