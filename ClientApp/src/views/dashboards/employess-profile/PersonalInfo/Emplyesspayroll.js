import React from 'react'
import Grid from '@mui/material/Grid'
import { Typography, Box, Paper, Card, TextField } from '@mui/material'
import { styled } from '@mui/material/styles'
import AmountDescriptionn from "./AmountDescriptionn"
import Button from '@mui/material/Button'
import Icon from 'src/@core/components/icon'
import FileDownloadIcon from '@mui/icons-material/FileDownload'
import PrintIcon from '@mui/icons-material/Print'
// import Logo from '../../../../../public/images/'
const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: 'center',
  color: theme.palette.text.secondary
}))
function PaySlip() {
  return (
    <div>
      <Grid container spacing={4}>
        <Grid item xs={9}>
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
                <Box sx={{ mt: 5, fontWeight: 5, minWidth :"40%" }}>
                  <Typography>
                    <b>Salary Name:</b> {}
                  </Typography>
                  <br />
                  <Typography>
                    <b>Person Full Name:</b> {}
                  </Typography>
                  <br />
                  <Typography>
                    <b>Employee Number:</b> {}
                  </Typography>
                  <br />
                  <Typography>
                    <b>Iqamah No :</b> {}
                  </Typography>
                  <br />
                  <Typography>
                    <b>Role:</b> {}
                  </Typography>
                  <br />
                </Box>

                <Box sx={{ mt: 5, fontWeight: 5, minWidth :"40%"  }}>
                  <Typography>
                    <b>Bank Account No :</b> {}
                  </Typography>
                  <br />
                  <Typography>
                    <b>UAN No:</b>  {}
                  </Typography>
                  <br />
                  <Typography>
                    <b>TAX No :</b> {}
                  </Typography>
                  <br />
                  <Typography>
                    <b>Leaves:</b> {}
                  </Typography>
                  <br />
                  <Typography>
                    <b>LOP:</b> {}
                  </Typography>
                  <br />
                </Box>
              </Box>

              <Box>
                 <AmountDescriptionn />
              </Box>
            </Box>
          </Item>
        </Grid>

        <Grid item xs={3}>
          <Card sx={{p :3}}>
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
            sx={{mt:3}}
              fullWidth
              id='date'
              label='Payroll end Date'
              type='date'
              defaultValue='YYYY-MM-DD'
              InputLabelProps={{
                shrink: true  
              }}
            />
          </Card>

          <Card sx={{mt:4}}>
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

export default PaySlip