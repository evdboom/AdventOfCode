// src/grid.rs

use std::hash::{self, Hash};
use std::ops::Add;

#[derive(Clone, Copy, Debug, Eq, PartialEq)]
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

impl Point {
    pub fn new(x: usize, y: usize) -> Self {
        Point { x, y }
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
