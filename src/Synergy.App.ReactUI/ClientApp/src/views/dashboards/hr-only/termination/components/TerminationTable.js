import React from 'react';
import { styled } from '@mui/material/styles';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell, { tableCellClasses } from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';

const StyledTableCell = styled(TableCell)(({ theme }) => ({
  [`&.${tableCellClasses.head}`]: {
    backgroundColor: theme.palette.common.black,
    color: theme.palette.common.white,
  },
  [`&.${tableCellClasses.body}`]: {
    fontSize: 14,
  },
}));

const StyledTableRow = styled(TableRow)(({ theme }) => ({
  '&:nth-of-type(odd)': {
    backgroundColor: theme.palette.action.hover,
  },
  // hide last border
  '&:last-child td, &:last-child th': {
    border: 0,
  },
}));



// Take Data as Per the API And change it later*********************************************************
function createData(name, calories, fat, carbs, protein, Api, DataLo) {
  return { name, calories, fat, carbs, protein, Api, DataLo };
}

const rows = [
  createData('Frozen yoghurt', 159, 6.0, 24, 4.0,0,0),
  createData('Ice cream sandwich', 237, 9.0, 37, 4.3,0,0),
  createData('Eclair', 262, 16.0, 24, 6.0,0,0),
  createData('Cupcake', 305, 3.7, 67, 4.3,0,0),
  createData('Gingerbread', 356, 16.0, 49, 3.9,0,0),
];

export default function TerminationTable() {
  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 700 }} aria-label="customized table">
        <TableHead>
          <TableRow>
            <StyledTableCell>Actions</StyledTableCell>
            <StyledTableCell>Service No.</StyledTableCell>
            <StyledTableCell>Subject</StyledTableCell>
            <StyledTableCell>Resignation Termination Date</StyledTableCell>
            <StyledTableCell> Last Working Date</StyledTableCell>
            <StyledTableCell> Service Status</StyledTableCell>
            <StyledTableCell> Clearance Form</StyledTableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {rows.map((row) => (
            <StyledTableRow key={row.name}>
              <StyledTableCell component="th" scope="row">
                {row.name}
              </StyledTableCell>
              <StyledTableCell >{row.calories}</StyledTableCell>
              <StyledTableCell >{row.fat}</StyledTableCell>
              <StyledTableCell >{row.carbs}</StyledTableCell>
              <StyledTableCell >{row.protein}</StyledTableCell>
              <StyledTableCell >{row.Api}</StyledTableCell>
              <StyledTableCell >{row.DataLo}</StyledTableCell>
            </StyledTableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}