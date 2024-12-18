use grid_helper::grid::Point;
use std::{cmp::Ordering, collections::HashSet};

#[derive(Clone, Eq, PartialEq)]
pub struct Position {
    pub point: Point,
    pub steps: usize,
    pub visited: HashSet<Point>,
}

impl Ord for Position {
    fn cmp(&self, other: &Self) -> Ordering {
        if self.steps < other.steps {
            Ordering::Greater
        } else if self.steps > other.steps {
            Ordering::Less
        } else {
            Ordering::Equal
        }
    }
}

impl PartialOrd for Position {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

impl Position {
    pub fn new(point: Point, steps: usize, visited: &HashSet<Point>) -> Self {
        let mut new_visisted = visited.clone();
        new_visisted.insert(point);
        Self {
            point,
            steps,
            visited: new_visisted,
        }
    }

    pub fn try_new(point: Point, steps: usize, visited: &HashSet<Point>) -> Option<Self> {
        let mut new_visisted = visited.clone();
        if !new_visisted.insert(point) {
            return None;
        }
        Some(Self {
            point,
            steps,
            visited: new_visisted,
        })
    }
}
