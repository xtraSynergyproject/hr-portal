// ** React Imports
import { useState } from 'react'
import Select from '@mui/material/Select'
import MenuItem from '@mui/material/MenuItem'
import InputLabel from '@mui/material/InputLabel'
import FormControl from '@mui/material/FormControl'
//import SearchIcon from '@mui/icons-material/Search';
//import IconButton from '@mui/material/IconButton';
import * as React from 'react'
import Paper from '@mui/material/Paper'
import InputBase from '@mui/material/InputBase'
//import Divider from '@mui/material/Divider';
import IconButton from '@mui/material/IconButton'
//import MenuIcon from '@mui/icons-material/Menu';
import SearchIcon from '@mui/icons-material/Search'
//import DirectionsIcon from '@mui/icons-material/Directions';

// ** MUI Imports
import Box from '@mui/material/Box'

// ** Third Party Imports
import TextField from '@mui/material/TextField'
import { Button, Grid } from '@mui/material'

const AttendanceCalender = ({ popperPlacement }) => {
    
  // ** States
  const [minDate, setMinDate] = useState(new Date())
  const [maxDate, setMaxDate] = useState(new Date())

  return (
    <Paper >
      <Grid  container spacing={2} sx={{display:'flex', alignItems:"center"}}>
        <Grid item xs={3}>
        <TextField required
          
            fullWidth
            sx={{ margin: '5px' }}
            id='date'
            label='Month'
            type='date'
            defaultValue='YYYY-MM-DD'
            InputLabelProps={{
              shrink: true
            }}
          />
        
        </Grid>
        <Grid item xs={3}>
          <TextField
            required
            fullWidth
            sx={{ marginY: '5px' }}
            id='date'
            label='Period Form'
            type='date'
            defaultValue='YYYY-MM-DD'
            InputLabelProps={{
              shrink: true
            }}
          />
        </Grid>
        <Grid item xs={3}>
          <TextField
            required
            fullWidth
            sx={{ marginY: '5px' }}
            id='date'
            label='To'
            type='date'
            defaultValue='YYYY-MM-DD'
            InputLabelProps={{
              shrink: true
            }}
          />
        </Grid>
        <Grid item xs={3}>
            <Button variant='contained'>Search</Button>
         
        </Grid>
        
      </Grid>
    </Paper>
  )
}

export default AttendanceCalender
