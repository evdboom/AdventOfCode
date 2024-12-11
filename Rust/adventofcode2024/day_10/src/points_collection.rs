use std::collections::HashSet;
use std::hash::Hash;

pub enum PointsCollection<T> {
    Unique(HashSet<T>),
    All(Vec<T>),
}

impl<T: Eq + Hash> PointsCollection<T> {
    pub fn new(unique: bool) -> Self {
        if unique {
            PointsCollection::Unique(HashSet::new())
        } else {
            PointsCollection::All(Vec::new())
        }
    }

    pub fn insert(&mut self, item: T) -> bool {
        match self {
            PointsCollection::Unique(set) => set.insert(item),
            PointsCollection::All(vec) => {
                vec.push(item);
                true
            }
        }
    }

    pub fn len(&self) -> usize {
        match self {
            PointsCollection::Unique(set) => set.len(),
            PointsCollection::All(vec) => vec.len(),
        }
    }
}
