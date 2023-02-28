import * as React from 'react'
import Pagination from '@mui/material/Pagination'
import Stack from '@mui/material/Stack'
// import * as React from 'react';
import Box from '@mui/material/Box'
import { Typography } from '@mui/material';
import ToggleButton from '@mui/material/ToggleButton';
import SkipNextIcon from '@mui/icons-material/SkipNext';
import SkipPreviousIcon from '@mui/icons-material/SkipPrevious';
import FastRewindIcon from '@mui/icons-material/FastRewind';
import FastForwardIcon from '@mui/icons-material/FastForward';
import Paper from '@mui/material/Paper';




export default function PaginationButton() {
  return (
    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'left', marginTop: '-20px' }}>
      <Box component='span' sx={{ p: 6 }}>
        <Box display={'flex'} alignItems={'center'}>
        <Paper elevation={0}>

          <ToggleButton value="left" aria-label="left aligned" size='small' >
            <SkipPreviousIcon />

          </ToggleButton>
          <ToggleButton value="center" aria-label="centered" size='small' >

            <FastRewindIcon />
          </ToggleButton>
          <ToggleButton value="right" aria-label="right aligned" color='primary' size='small' variant='conatined' >
            0
          </ToggleButton>
          <ToggleButton value="justify" aria-label="justified" size='small' >
            <FastForwardIcon />
          </ToggleButton>

          <ToggleButton value="justify" aria-label="justified" size='small' >
            <SkipNextIcon />
          </ToggleButton>
         </Paper>
          <Typography sx={{ml:90}}>No items to display</Typography>
        </Box>
      </Box>
    </Box>
  )
}