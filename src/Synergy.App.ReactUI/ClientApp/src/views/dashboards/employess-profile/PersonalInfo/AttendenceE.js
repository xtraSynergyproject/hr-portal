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


      <Item>
        <Box sx={{ width: 'auto', margin: 10 }}>
          <Box sx={{ display: 'flex', justifyContent: 'rghite' }}>
            {/* <img
              src='public/images/logos/logo.jfif'
              width='20%'
            /> */}


          </Box>


          <Box sx={{ display: 'flex', justifyContent: 'space-between', textAlign: 'left' }}>
            <Box sx={{ mt: 5, fontWeight: 5, minWidth: "30%" }}>

              <Card sx={{ p: 7 }}>
                <TextField
                  fullWidth
                  id='date'
                  label='Month'
                  type='date'
                  defaultValue='YYYY-MM-DD'
                  InputLabelProps={{
                    shrink: true
                  }}
                />
              </Card>
              <br />
            </Box>

            <Box sx={{ mt: 5, fontWeight: 5, minWidth: "30%" }}>
              <Card sx={{ p: 7}}>
                <TextField
                  fullWidth
                  id='date'
                  label='
                  PeriodFrom'
                  type='date'
                  defaultValue='YYYY-MM-DD'
                  InputLabelProps={{
                    shrink: true
                  }}
                />
              </Card>
              <br />


            </Box>
            <Box sx={{ mt: 5, fontWeight: 5, minWidth: "30%" }}>
              <Card sx={{ p: 7 }}>
                <TextField
                  fullWidth
                  id='date'
                  label='To'
                  type='date'
                  defaultValue='YYYY-MM-DD'
                  InputLabelProps={{
                    shrink: true
                  }}
                />
              </Card>
              <br /></Box>


          </Box>

        </Box>


      </Item>




    </div>
  )
}

export default PaySlip