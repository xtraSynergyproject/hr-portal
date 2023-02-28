import React from 'react'
import { TextField, Box } from '@mui/material'

function WeekDisplayName() {
  return (
    <Box sx={{width:" 410px",display:'flex',justifyContent:'space-between'}}>
      <TextField
        required
        sx={{ marginY: '5px', width: '200px' }}
        id='date'
        label='Period Form'
        type='date'
        defaultValue='YYYY-MM-DD'
        InputLabelProps={{
          shrink: true
        }}
      />

      <TextField
        required
        sx={{ marginY: '5px', width: '200px' }}
        id='date'
        label='To'
        type='date'
        defaultValue='YYYY-MM-DD'
        InputLabelProps={{
          shrink: true
        }}
      />
    </Box>
  )
}

export default WeekDisplayName
