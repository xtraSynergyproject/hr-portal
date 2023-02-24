import { Divider, Grid } from '@mui/material'
import React from 'react'
import Typography from '@mui/material/Typography'
import Avatar from '@mui/material/Avatar'
import Stack from '@mui/material/Stack'
import Box from '@mui/material/Box'

function UserProfileHeader() {
  return (
    <div>
      <Grid item xs={12} p='10px' sx={{ backgroundColor: 'white', height: '200px' }}>
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', alignContent: 'center' }}>
          <Box>
            <Stack direction='row' spacing={2}>
              <Avatar
                alt='Remy Sharp'
                src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
              />
            </Stack>
          </Box>
          <Divider />
          <Typography variant='h6' >
            <b>Administrator</b>
          </Typography>
          <Box  fontSize='12px'>
            <p>admin@synergy.com</p>
          </Box>
          <Box  fontSize='12px'><p>System Administrator</p></Box>
        </Box>
      </Grid>
    </div>
  )
}

export default UserProfileHeader
