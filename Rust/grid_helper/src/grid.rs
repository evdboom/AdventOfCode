// src/grid.rs

use std::hash::{self, Hash};
use std::ops::Add;

#[derive(Clone, Copy, Debug, Eq)]
pub struct Point {
    pub x: usize,
    pub y: usize,
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

    pub fn get_filtered_adjacent(&self, point: &Point, filter: fn(&T) -> bool) -> Vec<Point> {
        let mut adjacent = Vec::new();

        if let Some(left) = point.left() {
            if let Some(value) = self.get(&left) {
                if filter(value) {
                    adjacent.push(left);
                }
            }
        }

        if let Some(right) = point.right() {
            if let Some(value) = self.get(&right) {
                if filter(value) {
                    adjacent.push(right);
                }
            }
        }

        if let Some(up) = point.up() {
            if let Some(value) = self.get(&up) {
                if filter(value) {
                    adjacent.push(up);
                }
            }
        }

        if let Some(down) = point.down() {
            if let Some(value) = self.get(&down) {
                if filter(value) {
                    adjacent.push(down);
                }
            }
        }

        adjacent
    }

    pub fn get_adjacent(&self, point: &Point) -> Vec<Point> {
        let mut adjacent = Vec::new();

        if let Some(left) = point.left() {
            if self.get(&left).is_some() {
                adjacent.push(left);
            }
        }

        if let Some(right) = point.right() {
            if self.get(&right).is_some() {
                adjacent.push(right);
            }
        }

        if let Some(up) = point.up() {
            if self.get(&up).is_some() {
                adjacent.push(up);
            }
        }

        if let Some(down) = point.down() {
            if self.get(&down).is_some() {
                adjacent.push(down);
            }
        }

        adjacent
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
}