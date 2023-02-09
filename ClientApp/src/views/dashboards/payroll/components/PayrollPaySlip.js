import React from 'react'
import Grid from '@mui/material/Grid'
import { Typography, Box, Paper, Card, TextField } from '@mui/material'
import { styled } from '@mui/material/styles'
import Button from '@mui/material/Button'
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Icon from 'src/@core/components/icon'
import FileDownloadIcon from '@mui/icons-material/FileDownload'
import PrintIcon from '@mui/icons-material/Print'

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



const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: 'center',
  color: theme.palette.text.secondary
}))

function PayrollPaySlip() {
  return (
    <div>
      <Grid container spacing={4}>
        <Grid item xs={9}>
          <Paper elevation={4}>
            <Item>
              <Box sx={{ width: 'auto', margin: 5 }}>
                <Box sx={{ display: 'flex', justifyContent: 'center' }}>
                  {/* <img
              src='public/images/logos/logo.jfif'
              width='20%'
            /> */}
                  <Typography sx={{ fontSize: 20, ml: 5 }}>
                    <h4>Xtranet India, Bhopal, M.P.</h4>
                  </Typography>
                </Box>
                <Typography sx={{ mb: 7 }}>Date issued : {} </Typography>

                <Box sx={{ display: 'flex', justifyContent: 'space-between', textAlign: 'left' }}>
                  <Box sx={{ mt: 5, fontWeight: 5, minWidth: '40%' }}>
                    <Typography>
                      Salary Name: <b> {}</b>
                    </Typography>
                    <br />
                    <Typography>
                      Person Full Name:<b> {}</b>
                    </Typography>
                    <br />
                    <Typography>
                      Employee Number:<b> {}</b>
                    </Typography>
                    <br />
                    <Typography>
                      Iqamah No. :<b> {}</b>
                    </Typography>
                    <br />
                    <Typography>
                      Role:<b> {}</b>
                    </Typography>
                    <br />
                  </Box>

                  <Box sx={{ mt: 5, fontWeight: 5, minWidth: '40%' }}>
                    <Typography>
                      Bank Account No. :<b> {}</b>
                    </Typography>
                    <br />
                    <Typography>
                      UAN No. :<b> {}</b>
                    </Typography>
                    <br />
                    <Typography>
                      TAX No. :<b> {}</b>
                    </Typography>
                    <br />
                    <Typography>
                      Leaves:<b> {}</b>
                    </Typography>
                    <br />
                    <Typography>
                      LOP:<b> {}</b>
                    </Typography>
                    <br />
                  </Box>
                </Box>

                <Box>
                  <TableContainer component={Paper}>
                    <Table sx={{ minWidth: 650, mt: 10 }} aria-label='simple table'>
                      <TableHead>
                        <TableRow>
                          <TableCell>Description</TableCell>
                          <TableCell align='right'>Amount</TableCell>
                        </TableRow>
                      </TableHead>
                      <TableBody>
                        {rows.map(row => (
                          <TableRow key={row.name} sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                            <TableCell component='th' scope='row'>
                              {row.description}
                            </TableCell>
                            <TableCell align='right'>
                              <b>{row.amount}</b>
                            </TableCell>
                          </TableRow>
                        ))}
                      </TableBody>
                    </Table>
                  </TableContainer>
                </Box>
              </Box>
            </Item>
          </Paper>
        </Grid>

        <Grid item xs={3}>
          <Card sx={{ p: 3 }}>
            <TextField
              fullWidth
              id='date'
              label='Payroll Start Date'
              type='date'
              defaultValue='YYYY-MM-DD'
              InputLabelProps={{
                shrink: true
              }}
            />

            <TextField
              fullWidth
              sx={{ mt: 3 }}
              id='date'
              label='Payroll End Date'
              type='date'
              defaultValue='YYYY-MM-DD'
              InputLabelProps={{
                shrink: true
              }}
            />
          </Card>

          <Card sx={{ mt: 4 }}>
            <Button sx={{ m: 3, width: 0.915 }} variant='contained' endIcon={<Icon icon='mdi:send' />}>
              Send
            </Button>
            <Button sx={{ m: 3, width: 0.915 }} variant='contained' endIcon={<FileDownloadIcon />}>
              Download
            </Button>
            <Button sx={{ m: 3, width: 0.915 }} variant='contained' endIcon={<PrintIcon />}>
              Print
            </Button>
          </Card>
        </Grid>
      </Grid>
    </div>
  )
}

export default PayrollPaySlip


