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
      <Grid container spacing={4} >
        <Grid item xs={12}>
          <Item>
            <Box sx={{ width: 'auto', margin: 5 }}>
              <Box sx={{ display: 'flex', justifyContent: 'rghite' }}>
                {/* <img
              src='public/images/logos/logo.jfif'
              width='20%'
            /> */}
                <Typography sx={{ fontSize: 20, ml: 2 }}>
                  <h4>Contact</h4>
                </Typography>
                <Typography sx={{ fontSize: 20, ml: 90, minWidth :"center" }}>
                <Button variant='contained'>Manage Contact</Button>
                </Typography>
              </Box>
        

              <Box sx={{ display: 'flex', justifyContent: 'space-between', textAlign: 'left' }}>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth :"40%" }}>
                
                  <Typography>
                    ContractType:
                  </Typography>
                  <br />
                  <Typography>
                        Sponsor:
                  </Typography>
                  <br />
                  <Typography>
                  
                  </Typography>
                  <br />
                  <Typography>
            
                  </Typography>
                  <br />
                  <Typography>
               
                  </Typography>
                  <br />
                </Box>

                <Box sx={{ mt: 5, fontWeight: 5, minWidth :"40%"  }}>
                  <Typography>
                ContractRenewable:
                  </Typography>
                  <br />
                  <Typography>
                    AnnualLeaveEntitlement:
                  </Typography>
                  <br />
                  <Typography>
                   
                  </Typography>
                  <br />
                
                  <br />
               
                  <br />
                </Box>
              </Box>
            </Box>
          </Item>
        </Grid>

      
      </Grid>
    </div>
  )
}

export default PaySlip