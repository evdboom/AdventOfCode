use std::{cmp::Ordering, collections::HashMap};

const CARD_STRENGTH: [char; 14] = [
    'X', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A',
];

pub struct Hand {
    pub cards: Vec<char>,
    pub bid: usize,
    pub joker_game: bool,
}

impl PartialOrd for Hand {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

impl Ord for Hand {
    fn cmp(&self, other: &Self) -> Ordering {
        if self.get_hand_type() < other.get_hand_type() {
            return Ordering::Less;
        } else if self.get_hand_type() > other.get_hand_type() {
            return Ordering::Greater;
        } else {
            for (self_card, other_card) in self.cards.iter().zip(other.cards.iter()) {
                if get_card_strength(self_card, self.joker_game)
                    < get_card_strength(other_card, other.joker_game)
                {
                    return Ordering::Less;
                } else if get_card_strength(self_card, self.joker_game)
                    > get_card_strength(other_card, other.joker_game)
                {
                    return Ordering::Greater;
                }
            }
        }
        Ordering::Equal
    }
}

impl Eq for Hand {}

impl PartialEq for Hand {
    fn eq(&self, other: &Self) -> bool {
        self.get_hand_type() == other.get_hand_type() && self.bid == other.bid
    }
}

impl Hand {
    pub fn new(cards: Vec<char>, bid: usize, joker_game: bool) -> Self {
        Hand {
            cards,
            bid,
            joker_game,
        }
    }

    pub fn get_hand_type(&self) -> usize {
        let mut grouped = self.cards.iter().fold(HashMap::new(), |mut acc, card| {
            let count = acc.entry(card).or_insert(0);
            *count += 1;
            acc
        });

        if self.joker_game {
            if let Some(joker) = grouped.get(&'J') {
                let joker_count = *joker;
                if joker_count != 5 {
                    grouped.remove(&'J');
                    let max_card = grouped.iter_mut().max_by_key(|entry| *entry.1).unwrap();
                    *max_card.1 += joker_count;
                }
            }
        }

        match grouped.len() {
            1 => 6,
            2 => {
                if grouped.values().max().unwrap().clone() == 4 {
                    5
                } else {
                    4
                }
            }
            3 => {
                if grouped.values().max().unwrap().clone() == 3 {
                    3
                } else {
                    2
                }
            }
            4 => 1,
            _ => 0,
        }
    }
}

fn get_card_strength(card: &char, joker_game: bool) -> usize {
    if joker_game && card == &'J' {
        0
    } else {
        CARD_STRENGTH.iter().position(|&c| &c == card).unwrap()
    }
}
