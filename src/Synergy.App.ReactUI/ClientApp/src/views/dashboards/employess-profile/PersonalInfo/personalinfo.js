import React from 'react'
import Grid from '@mui/material/Grid'
import { Typography, Box, Paper, Card, TextField } from '@mui/material'
import { styled } from '@mui/material/styles'
import AmountDescriptionn from "./AmountDescriptionn"
import Button from '@mui/material/Button'
import Icon from 'src/@core/components/icon'
import FileDownloadIcon from '@mui/icons-material/FileDownload'
import PrintIcon from '@mui/icons-material/Print'
import axios from 'axios'
import { Fragment, useState, useEffect } from 'react'

// import Logo from '../../../../../public/images/'
const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: 'center',
  color: theme.palette.text.secondary
}))

//API

function Info() {
  return (
    <div>
      <Grid container spacing={4}>
        <Grid item xs={12}>
          <Item>
            <Box sx={{ width: '50', margin: 20 }}>
              <Typography sx={{ fontSize: 23, ml: 2 }}>
                <p>Basic Info</p>
              </Typography>
              <hr />
              <Box sx={{ display: 'flex', justifyContent: 'rghite' }}>
                {/* <img
              src='public/images/logos/logo.jfif'
              width='20%'
            /> */}
              </Box>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', textAlign: 'left' }}>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "40%" }}>
                  <Typography>
                    <h4></h4>
                  </Typography>
                  <br />
                  <Typography>
                    Person No :
                  </Typography>
                  <br />
                  <Typography>
                    Personal Email :
                  </Typography>
                  <br />
                  <br />
                  <Typography >
                    <h4>Personal Details</h4>
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
                  <br />
                  <br />
                  <Typography >
                    <h4>Present Country Address</h4>
                  </Typography>
                  <br />
                  <Typography>
                    Unit Number :
                  </Typography>
                  <br />
                  <Typography>
                    Street Name :</Typography>
                  <br />
                  <Typography>
                    Postal Code :
                  </Typography><br />
                  <Typography>
                    Country Name :
                  </Typography>
                  <br />
                  <Typography>
                    Contact Country Name :
                  </Typography>
                  <br />
                  <br />
                  <Typography>

                    <h4> Emergency Contact Info 1</h4>
                  </Typography>
                  <br />
                  <Typography>
                    Emergency Contact Name 1 :

                  </Typography>
                  <br />
                  <br />
                  <Typography>
                    Emergency Contact No 1 :

                  </Typography>
                  <br />
                  <br />
                  <br />
                </Box>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "40%" }}>
                  <br />
                  <br />
                  <br />
                  <br />
                  <Typography>
                    Person Full Name :
                  </Typography>
                  <br />
                  <br />
                  <br />
                  <br />
                  <br />
                  <br />
                  <br />
                  
                  
                  <Typography>
                    Person Full Name :
                  </Typography>
                  <br />
                  <Typography>
                    Nationality Name :
                  </Typography>
                  <br /><Typography>
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
                  <br />
                  <br />
                  <br />
                  <br />
                  <br />
                  <br />
                  <br />
                  <br/>
                
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
                  <br />
                  <br />
                  <br />
                  <br />
                  <br />
                  <br />
                  <br />
                  <br />
                  <br />
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
                  <br />
                  <br />
                  <br />
                </Box>
              </Box>
              <Typography sx={{ fontSize: 20, ml: 200, minWidth: "center" }}>
                <Button variant='contained'>Edit</Button>
              </Typography>
            </Box>
          </Item>
        </Grid>
      </Grid>
    </div>
  )
}

export default Info