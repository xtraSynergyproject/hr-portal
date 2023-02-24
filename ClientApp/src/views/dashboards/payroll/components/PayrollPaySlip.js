import React from 'react'
import Grid from '@mui/material/Grid'
import { Typography, Box, Paper, Card, TextField } from '@mui/material'
import { styled } from '@mui/material/styles'
import Button from '@mui/material/Button'
import Table from '@mui/material/Table'
import TableBody from '@mui/material/TableBody'
import TableCell from '@mui/material/TableCell'
import TableHead from '@mui/material/TableHead'
import TableRow from '@mui/material/TableRow'
import Icon from 'src/@core/components/icon'
import FileDownloadIcon from '@mui/icons-material/FileDownload'
import PrintIcon from '@mui/icons-material/Print'

function createData(description, amount) {
  return { description, amount }
}

const rows = [
  createData('Total Earning', 'Rs.25000'),
  createData('Total Deduction', 'Rs.2200'),
  createData('PF Deduction', 'Rs.32546 '),
  createData('Taxes', 'Rs.1000'),
  createData('Net Amount', 'Rs.50000')
]

const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: 'center',
  color: theme.palette.text.secondary
}))

function PayrollPaySlip() {
  // const [value, setValue] = React.useState(dayjs('2022-04-07'));

  return (
    <div>
      <Box className='psp_main_box'  sx={{display:"flex" ,justifyContent:"space-between"}}>
        <Paper className='psp_paper_two' elevation={4} sx={{width:"55vw"}}  >
          <Box sx={{  margin: 5 }} className="psp_box_three">
            <Box sx={{ display: 'flex',alignItems:"center", flexDirection: 'column' }}>
              <Typography sx={{ fontSize: 20, ml: 5 }}>
                <h4>Xtranet India, Bhopal, M.P.</h4>
              </Typography>
              <Typography sx={{ mb: 7 }}>Date issued : {} </Typography>
            </Box>

            <Box
              className='psp_info_box'
              sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', textAlign: 'left' }}
            >
              <Box className='psp_info_content psp_cb_one' sx={{ fontWeight: 5, minWidth: '40%' }}>
                <Typography className='psp_info_typo'>
                  Salary Name: <b> {}</b>
                </Typography>
                <br />
                <Typography className='psp_info_typo'>
                  Person Full Name:<b> {}</b>
                </Typography>
                <br />
                <Typography className='psp_info_typo'>
                  Employee Number:<b> {}</b>
                </Typography>
                <br />
                <Typography className='psp_info_typo'>
                  Iqamah No. :<b> {}</b>
                </Typography>
                <br />
                <Typography className='psp_info_typo'>
                  Role:<b> {}</b>
                </Typography>
                <br />
              </Box>

              <Box className='psp_info_content psp_cb_two' sx={{ fontWeight: 5, minWidth: '40%' }}>
                <Typography className='psp_info_typo'>
                  Bank Account No. :<b> {}</b>
                </Typography>
                <br />
                <Typography className='psp_info_typo'>
                  UAN No. :<b> {}</b>
                </Typography>
                <br />
                <Typography className='psp_info_typo'>
                  TAX No. :<b> {}</b>
                </Typography>
                <br />
                <Typography className='psp_info_typo'>
                  Leaves:<b> {}</b>
                </Typography>
                <br />
                <Typography className='psp_info_typo'>
                  LOP:<b> {}</b>
                </Typography>
                <br />
              </Box>
            </Box>

            <Box>
              <Paper className='container'>
                <Table>
                  <TableHead>
                    <TableRow>
                      <TableCell>Description</TableCell>
                      <TableCell numeric>Amount</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {rows.map(({ id, description, amount }) => (
                      <TableRow key={id}>
                        <TableCell component='th' scope='row'>
                          {description}
                        </TableCell>
                        <TableCell>Rs. {amount}</TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </Paper>

              {/* <TableContainer >
                    <Table sx={{ minWidth: 650, mt: 10 }} aria-label='simple table'>
                      <TableHead>
                        <TableRow>
                          <TableCell>Description</TableCell>
                          <TableCell align='right'>Amount</TableCell>
                        </TableRow>
                      </TableHead>
                      <TableBody >
                        {rows.map(row => (
                          <TableRow key={row.name} sx={{ '&:last-child td, &:last-child th': { border: 0 }, display:"flex", justifyContent:"space-between" }}>
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
                  </TableContainer> */}
            </Box>
          </Box>
        </Paper>

        
          <Card className='psp_btn_calendar' sx={{ p: 3, display:"flex", flexDirection:"column",alignItems:"center", height:"290px" ,width:"20vw"}}>
            <TextField
             fullWidth
             sx={{m:3}}
              id='date'
              label='Payroll Date'
              type='date'
              defaultValue='YYYY-MM-DD'
              InputLabelProps={{
                shrink: true
              }}
            />

            <Button sx={{ m: 3, }} fullWidth variant='contained' endIcon={<Icon icon='mdi:send' />}>
              Send
            </Button>
            <Button sx={{ m: 3, }} fullWidth variant='contained' endIcon={<FileDownloadIcon />}>
              Download
            </Button>
            <Button sx={{ m: 3, }} fullWidth variant='contained' endIcon={<PrintIcon />}>
              Print
            </Button>
          </Card>
        </Box>
      
    </div>
  )
}

export default PayrollPaySlip
