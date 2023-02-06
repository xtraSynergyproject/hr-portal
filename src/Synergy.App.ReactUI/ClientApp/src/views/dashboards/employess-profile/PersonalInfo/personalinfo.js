import React from 'react'
import Grid from '@mui/material/Grid'
import { Typography, Box, Paper, Card, TextField } from '@mui/material'
import { styled } from '@mui/material/styles'
import AmountDescriptionn from "./AmountDescriptionn"
import Button from '@mui/material/Button'
import Icon from 'src/@core/components/icon'
import FileDownloadIcon from '@mui/icons-material/FileDownload'
import PrintIcon from '@mui/icons-material/Print'
import Personal_edit_modal from './Personal_edit_modal'
// import Logo from '../../../../../public/images/'
const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: 'center',
  color: theme.palette.text.secondary
}))

const handleClickOpen = () => {
  setOpen(true)
}
function PaySlip() {
  return (
    <div>
      <br />
      <Grid container spacing={6}>
        <Grid item xs={11}>

          <Item>
            <Box sx={{ width: 'auto', margin: 5 }}>

              <Box sx={{ display: 'flex', justifyContent: 'space-between', textAlign: 'left' }}>

                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "30%" }}>

                  <Typography>
                    <h3>Basic Info</h3>
                  </Typography>
                  <br />
                  <Typography>
                    Person No :
                  </Typography>
                  <br />
                  <Typography>
                    Personal
                  </Typography>
                  <br />
                  <Typography>
                    Email :
                  </Typography>
                </Box>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "40%" }}>
                  <br />
                  <br />
                  <br /><br />
                  <Typography>
                    Person Full
                  </Typography>
                  <br />
                  <Typography>
                    Name :
                  </Typography>
                </Box>
              </Box>
            </Box>
          </Item>
        </Grid>
        <Grid item xs={11}>
          <Item>
            <Box sx={{ width: 'auto', margin: 5 }}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', textAlign: 'left' }}>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "30%" }}>
                  <Typography>
                    <h3>Personal Details</h3>
                  </Typography>
                  <br />
                  <Typography>
                    Title :
                  </Typography>
                  <br />
                  <Typography>
                    Gender :
                  </Typography>
                  <br />
                  <Typography>
                    Marital Status :
                  </Typography>
                  <br />
                  <Typography>
                    Date of Birth :
                  </Typography>
                  <br />
                  <Typography>
                    Contact Country Name :
                  </Typography>
                  <br />
                </Box>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "40%" }}>
                  <Typography variant='h6' component='span'>
                   <Personal_edit_modal/>
                  </Typography>
                  <br />
                  <br />
                  <br /><br />
                  <Typography>
                    Person Full Name :
                  </Typography>
                  <br />
                  <Typography>
                    Nationality Name :
                  </Typography>
                  <br />
                  <Typography>
                    Religion :
                  </Typography>
                  <br />
                  <Typography>
                    Personal Email :
                  </Typography>
                  <br />
                  <Typography>
                    Mobile :
                  </Typography>
                </Box>
              </Box>
            </Box>
          </Item>
        </Grid>
        <Grid item xs={11}>
          <Item>
            <Box sx={{ width: 'auto', margin: 5 }}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', textAlign: 'left' }}>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "30%" }}>
                  <Typography>
                    <h3>Present Country Address</h3>
                  </Typography>
                  <br />
                  <Typography>
                    Unit Number :
                  </Typography>
                  <br />
                  <Typography>
                    Street Name :
                  </Typography>
                  <br />
                  <Typography>
                    Postal Code :
                  </Typography>
                  <br />
                  <Typography>
                    Country Name :
                  </Typography>
                  <br />
                </Box>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "40%" }}>
                  <br />
                  <br />
                  <br /><br />
                  <Typography>
                    Building Number :
                  </Typography>
                  <br />
                  <Typography>
                    City :
                  </Typography>
                  <br />
                  <Typography>
                    Additional Number :
                  </Typography>
                </Box>
              </Box>
            </Box>
          </Item>
        </Grid>
        <Grid item xs={11}>
          <Item>
            <Box sx={{ width: 'auto', margin: 5 }}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', textAlign: 'left' }}>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "30%" }}>
                  <Typography>
                    <h3> Home Country Address</h3>
                  </Typography>
                  <br />
                  <Typography>
                    Unit Number :
                  </Typography>
                  <br />
                  <Typography>
                    Street Name :
                  </Typography>
                  <br />
                  <Typography>
                    Postal Code :
                  </Typography>
                  <br />
                  <Typography>
                    Country Name :
                  </Typography>
                  <br />
                </Box>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "40%" }}>
                  <br />
                  <br />
                  <br /><br />
                  <Typography>
                    Building Number :
                  </Typography>
                  <br />
                  <Typography>

                    City :
                  </Typography>
                  <br />
                  <Typography>
                    Additional Number :
                  </Typography>
                </Box>
              </Box>
            </Box>
          </Item>
        </Grid>
        <Grid item xs={11}>
          <Item>
            <Box sx={{ width: 'auto', margin: 5 }}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', textAlign: 'left' }}>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "30%" }}>
                  <Typography>
                    <h3>Emergency Contact Info 1</h3>
                  </Typography>
                  <br />
                  <Typography>
                    Emergency Contact Name 1 :
                  </Typography>
                  <br />
                  <Typography>
                    Emergency Contact No 1 :
                  </Typography>
                </Box>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "40%" }}>
                  <br />
                  <br />
                  <br /><br />
                  <Typography>
                    Emergency Contact Country Name 1 :
                  </Typography>
                  <br />
                  <Typography>
                    Relationship 1 :
                  </Typography>
                </Box>
              </Box>
            </Box>
          </Item>
        </Grid>
        <Grid item xs={11}>

          <Item>
            <Box sx={{ width: 'auto', margin: 5 }}>

              <Box sx={{ display: 'flex', justifyContent: 'space-between', textAlign: 'left' }}>

                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "30%" }}>

                  <Typography>
                    <h3>Emergency Contact Info 2</h3>
                  </Typography>
                  <br />
                  <Typography>
                    Emergency Contact Name 2 :

                  </Typography>
                  <br />
                  <Typography>
                    Emergency Contact No 2 :
                  </Typography>
                  <br />



                  <Typography>
                    Marital Status :
                  </Typography>
                  <br />



                </Box>

                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "40%" }}>
                  <br />
                  <br />
                  <br /><br />
                  <Typography>
                    Relationship 2 :
                  </Typography>
                  <br />
                  <Typography>
                    Emergency Contact Country Name 2 :
                  </Typography>
                  <br />
                  <Typography>
                    Relationship 2 :
                  </Typography>
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