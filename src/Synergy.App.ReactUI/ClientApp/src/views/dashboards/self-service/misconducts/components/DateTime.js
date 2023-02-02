import React from 'react'
import TextField from '@mui/material/TextField'
import Stack from '@mui/material/Stack'
import Select from '@mui/material/Select'
import MenuItem from '@mui/material/MenuItem'
import InputLabel from '@mui/material/InputLabel'
import FormControl from '@mui/material/FormControl'
import { Box } from '@mui/material'

export default function DateTime() {
  return (
    <Stack component='form' noValidate spacing={3}  >
      <Box sx={{px:2, py:3}}>
        <FormControl fullWidth sx={{mb:1.5}}>
          <InputLabel id='demo-simple-select-helper-label'>Priority</InputLabel>
          <Select
            label='Priority'
            defaultValue=''
            id='demo-simple-select-helper'
            labelId='demo-simple-select-helper-label'
          >
            <MenuItem value=''>
              <em>Select</em>
            </MenuItem>
            <MenuItem value='High'>High</MenuItem>
            <MenuItem value='Medium'>Medium</MenuItem>
            <MenuItem value='Low'>Low</MenuItem>
          </Select>
        </FormControl>
        <TextField sx={{my:1.5}}
          fullWidth
          id='datetime-local'
          label='Start Date'
          type='datetime-local'
          defaultValue='2017-05-24T10:30'
          InputLabelProps={{
            shrink: true
          }}
        />
        <TextField sx={{my:1.5}}
          fullWidth
          id='datetime-local'
          label='Due Date'
          type='datetime-local'
          defaultValue='2017-05-24T10:30'
          InputLabelProps={{
            shrink: true
          }}
        />
        <TextField sx={{my:1.5}}
          fullWidth
          id='datetime-local'
          label='Reminder Date'
          type='datetime-local'
          defaultValue='2017-05-24T10:30'
          InputLabelProps={{
            shrink: true
          }}
        />
        <TextField fullWidth type='number' id='outlined-basic' label='Day' variant='outlined'  sx={{my:1.5}}/>
        <TextField fullWidth type='number' id='outlined-basic' label='Hour' variant='outlined' sx={{my:1.5}} />
        <TextField fullWidth type='number' id='outlined-basic' label='Minute' variant='outlined'  sx={{mt:1.5}}/>
      </Box>
    </Stack>
  )
}
