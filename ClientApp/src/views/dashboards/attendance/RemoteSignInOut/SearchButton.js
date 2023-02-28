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
        <Grid item xs={8}>
          <FormControl fullWidth>
             <InputLabel required id='demo-simple-select-outlined-label'>
              Select
            </InputLabel> 
            <Select
              label='Employee'
              defaultValue=''
              id='demo-simple-select-outlined'
              labelId='demo-simple-select-outlined-label'
            >
              <Grid >
               <Box fullWidth>
          
               
                {/* <IconButton type='button' aria-label='search'> */}
                  
                  
                <TextField sx={{width:'850px'}}/>
               
                {/* </IconButton> */}
             
              </Box>
              </Grid>
           
               
            </Select>
          </FormControl>
        </Grid>
      
        
        <Grid item xs={3}>
        <Button variant="contained"><svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path fill="currentColor" d="M16 17v2H2v-2s0-4 7-4s7 4 7 4m-3.5-9.5A3.5 3.5 0 1 0 9 11a3.5 3.5 0 0 0 3.5-3.5m3.44 5.5A5.32 5.32 0 0 1 18 17v2h4v-2s0-3.63-6.06-4M15 4a3.39 3.39 0 0 0-1.93.59a5 5 0 0 1 0 5.82A3.39 3.39 0 0 0 15 11a3.5 3.5 0 0 0 0-7Z"/></svg></Button>
        </Grid>
      </Grid>
    </Paper>
  )
}

export default AccessCalender
