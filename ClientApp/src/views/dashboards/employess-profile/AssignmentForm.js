import React from 'react'
import Grid from '@mui/material/Grid'
import { styled } from '@mui/material/styles'

import { Typography, Box, } from '@mui/material'
import Button from '@mui/material/Button'


import Paper from '@mui/material/Paper';
import Assingmentmodal from './PersonalInfo/Assingmentmodal'


const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(5),
  textAlign: 'center',
  color: theme.palette.text.secondary
}))
function TextFieldSizes() {
  return (

    <div>
      <Grid container spacing={4} sx={{ width: '50' }}>
        <Grid item xs={12}>
          <Item>
            <Box sx={{ width: '50', margin: 5 }}>
              <Box sx={{ display: 'flex', justifyContent: 'rghite' }}>
                {/* <img
          src='public/images/logos/logo.jfif'
          width='20%'
        /> */}
                <Typography sx={{ fontSize: 16, ml: 2, }}>
                  <h3>Assignment</h3>
                </Typography>
                <Typography sx={{ fontSize: 20,   ml: 180}}>
                <Assingmentmodal/>
              </Typography>
              </Box>
              <hr />
              <br />
            
              <Box sx={{ display: 'flex', justifyContent: 'space-between', textAlign: 'left' }}>
                <Box sx={{ mt: 5, fontWeight: 5, minWidth: "40%" }}>
                  <br />
                  <br />
                  <br />
                  <Typography>
                    Department :
                    </Typography>
                  <br />
                  <Typography>
                    Job :
                    </Typography>
                  <br />
                  <Typography>
                    Position :
                    </Typography>
                  <br />
                  <Typography>
                    Location :

                  </Typography>
                  <br />
                  <Typography>
                    Probation Period :
                    </Typography>
                  <br />
                  <br />
                  </Box>
                  <Box sx={{ mt: 5, fontWeight: 5, minWidth: "40%" }}>
                    <br />     <br />     <br /> <br />
                    <Typography>
                    Assignment Grade :
                    </Typography>
                  <br />
                  <Typography>
                    Assignment Type :
                  </Typography>
                  <br />
                  <Typography>
                    Assignment Status :


                  </Typography>
                  <br />

                  <Typography>
                    Date Of Join :

                  </Typography>
                  <br />
                  <Typography>
                    Notice Period :

                  </Typography>
                  <br />

                  <br />
                  </Box></Box>
           
            </Box>
          </Item>
        </Grid>


      </Grid>
    </div>

  );
}

export default TextFieldSizes

