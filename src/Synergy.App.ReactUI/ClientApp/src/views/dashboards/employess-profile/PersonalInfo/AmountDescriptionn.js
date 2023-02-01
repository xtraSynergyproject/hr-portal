import React from 'react';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';

function createData(description, amount) {
  return { description, amount};
}

const rows = [
  createData('Total Earning', "Rs.25000"),
  createData('Total Deduction', "Rs.2200"),
  createData('PF Deduction', "Rs.32546 "),
  createData('Taxes', "Rs.1000"),
  createData('Net Amount', "Rs.50000"),
];

export default function AmountDescription() {
  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 , mt : 10}} aria-label="simple table">
        <TableHead>
          <TableRow>
            <TableCell>Description</TableCell>
            <TableCell align="right">Amount</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {rows.map((row) => (
            <TableRow
              key={row.name}
              sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
            >
              <TableCell component="th" scope="row">
                {row.description}
              </TableCell>
              <TableCell align="right">{row.amount}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}