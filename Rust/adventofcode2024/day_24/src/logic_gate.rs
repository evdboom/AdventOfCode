use std::collections::HashMap;

#[derive(Clone, Copy, PartialEq, Eq)]
pub enum GateType {
    AND,
    OR,
    XOR,
}

#[derive(Clone)]
pub struct LogicGate {
    pub input_a: Vec<u8>,
    pub input_b: Vec<u8>,
    pub output_wire: Vec<u8>,
    pub gate_type: GateType,
}

impl LogicGate {
    pub fn new(
        input_a: Vec<u8>,
        input_b: Vec<u8>,
        output_wire: Vec<u8>,
        gate_type: GateType,
    ) -> LogicGate {
        LogicGate {
            input_a,
            input_b,
            output_wire,
            gate_type,
        }
    }

    pub fn calculate_output(&self, wires: &HashMap<Vec<u8>, bool>) -> Option<bool> {
        if let Some(wire_a) = wires.get(&self.input_a) {
            if let Some(wire_b) = wires.get(&self.input_b) {
                let result = match self.gate_type {
                    GateType::AND => wire_a & wire_b,
                    GateType::OR => wire_a | wire_b,
                    GateType::XOR => wire_a ^ wire_b,
                };
                return Some(result);
            }
        }

        None
    }

    pub fn is_input(&self, input: &Vec<u8>) -> bool {
        &self.input_a == input || &self.input_b == input
    }
}
