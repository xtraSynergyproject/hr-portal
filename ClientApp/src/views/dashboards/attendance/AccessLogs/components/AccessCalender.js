// ** React Imports
import { useState } from 'react'
import Select from '@mui/material/Select'
import MenuItem from '@mui/material/MenuItem'
import InputLabel from '@mui/material/InputLabel'
import FormControl from '@mui/material/FormControl'

import * as React from 'react'
import Paper from '@mui/material/Paper'
import InputBase from '@mui/material/InputBase'

import IconButton from '@mui/material/IconButton'

import SearchIcon from '@mui/icons-material/Search'


// ** MUI Imports
import Box from '@mui/material/Box'

// ** Third Party Imports
import TextField from '@mui/material/TextField'
import { Button, Grid } from '@mui/material'

const AccessCalender = ({ popperPlacement }) => {
  // ** States
  const [minDate, setMinDate] = useState(new Date())
  const [maxDate, setMaxDate] = useState(new Date())

  return (
    <Paper sx={{ px: '20px', py: '15px' }}>
      <Grid container spacing={2} sx={{ display: 'flex', alignItems: 'center' }}>
        <Grid item xs={3}>
          <FormControl fullWidth>
            <InputLabel required id='demo-simple-select-outlined-label'>
              Employee
            </InputLabel>
            <Select
              label='Employee'
              defaultValue=''
              id='demo-simple-select-outlined'
              labelId='demo-simple-select-outlined-label'
            >
              <MenuItem value=''>
                <em>Select</em>
              </MenuItem>
              <MenuItem value='searchbar'>
                <InputBase fullWidth inputProps={{ 'aria-label': 'search ' }} />
                <IconButton type='button' aria-label='search'>
                  <SearchIcon />
                </IconButton>
              </MenuItem>
            </Select>
          </FormControl>
        </Grid>
        <Grid item xs={3}>
          <TextField
            required
            fullWidth
            sx={{ marginY: '5px' }}
            id='date'
            label='Start Date'
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
            label='End Date'
            type='date'
            defaultValue='YYYY-MM-DD'
            InputLabelProps={{
              shrink: true
            }}
          />
        </Grid>
        <Grid item xs={3}>
        <Button variant="contained">Search</Button>
        </Grid>
      </Grid>
    </Paper>
  )
}

export default AccessCalender
