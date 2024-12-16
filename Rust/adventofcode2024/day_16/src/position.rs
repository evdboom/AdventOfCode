use grid_helper::grid::{Direction, Point};
use std::{cmp::Ordering, collections::HashSet};

#[derive(Clone, Eq, PartialEq)]
pub struct Position {
    pub direction: Direction,
    pub cost: usize,
    pub visited: HashSet<Point>,
}

impl Ord for Position {
    fn cmp(&self, other: &Self) -> Ordering {
        if self.cost < other.cost {
            Ordering::Greater
        } else if self.cost > other.cost {
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
    pub fn new(direction: Direction, cost: usize, visited: &HashSet<Point>) -> Self {
        let mut new_visisted = visited.clone();
        new_visisted.insert(direction.point());
        Self {
            direction,
            cost,
            visited: new_visisted,
        }
    }

    pub fn point(&self) -> Point {
        self.direction.point()
    }
}
