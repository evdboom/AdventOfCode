#[derive(Clone)]
pub struct Program {
    register_a: usize,
    register_b: usize,
    register_c: usize,
    program: Vec<(usize, usize)>,
}

impl Program {
    pub fn new() -> Self {
        Self {
            register_a: 0,
            register_b: 0,
            register_c: 0,
            program: Vec::new(),
        }
    }

    pub fn set_a(&mut self, value: usize) {
        self.register_a = value;
    }

    pub fn set_b(&mut self, value: usize) {
        self.register_b = value;
    }

    pub fn set_c(&mut self, value: usize) {
        self.register_c = value;
    }

    pub fn get_a(&self) -> usize {
        self.register_a
    }

    pub fn get_b(&self) -> usize {
        self.register_b
    }

    pub fn get_c(&self) -> usize {
        self.register_c
    }

    pub fn add_to_program(&mut self, value: (usize, usize)) {
        self.program.push(value);
    }

    pub fn iter(&self) -> ProgramIterator {
        ProgramIterator {
            program: self,
            index: 0,
        }
    }

    pub fn get_instruction_count(&self) -> usize {
        self.program.len() * 2
    }

    pub fn get_instruction(&self, index: &usize) -> Option<usize> {
        if index >= &(self.program.len() * 2) {
            return None;
        }

        let record = self.program[index / 2];
        if index % 2 == 0 {
            Some(record.0)
        } else {
            Some(record.1)
        }
    }
}

pub struct ProgramIterator<'a> {
    program: &'a Program,
    index: usize,
}

impl<'a> Iterator for ProgramIterator<'a> {
    type Item = (usize, usize);

    fn next(&mut self) -> Option<Self::Item> {
        if self.index >= self.program.program.len() {
            return None;
        }

        let value = self.program.program[self.index];
        self.index += 1;

        Some(value)
    }
}

impl ProgramIterator<'_> {
    pub fn jump(&mut self, value: usize) {
        self.index = value;
    }
}

pub enum OperationResult {
    None,
    Value(usize),
    Jump(usize),
}
