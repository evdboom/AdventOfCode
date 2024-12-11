// tests/grid_tests.rs
use grid_helper::grid::{Grid, Point};

#[test]
fn test_grid_creation() {
    let grid = Grid::new(3, 3, 0);
    assert_eq!(grid.width(), 3);
    assert_eq!(grid.height(), 3);
    assert_eq!(grid.size(), 9);
}

#[test]
fn test_grid_iterator() {
    let mut grid = Grid::new(3, 3, 0);
    grid.set(Point { x: 1, y: 1 }, 5);
    let iter = grid.iter();
    let cells: Vec<_> = iter.collect();
    assert_eq!(cells.len(), 9);
    assert_eq!(cells[4].value, 5);
}
