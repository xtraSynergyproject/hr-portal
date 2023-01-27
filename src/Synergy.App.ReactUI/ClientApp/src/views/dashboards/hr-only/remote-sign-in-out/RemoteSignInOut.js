import React from 'react'
//MUI Imports
import Box from '@mui/material/Box'
import Paper from '@mui/material/Paper'
import Typography from '@mui/material/Typography'
import PayrollProfileDetails from '../../payroll/components/PayrollProfileDetails'
import { Divider, FormControl, InputLabel, Select, MenuItem, Button } from '@mui/material'

function RemoteSignInOut() {
  const [value, setValue] = React.useState('')
  const handleChanges = event => {
    setValue(event.target.value)
  }

  return (
    <div>
      <Paper elevation={4}>
        <Box sx={{ px: 4, py: 6, display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant='h4' component='h2'>
            Remote Sign In/Out
          </Typography>
        </Box>
        <Divider sx={{ mb: 0 }} />
        <PayrollProfileDetails />

        
      </Paper>

      <Box>
        <Paper elevation={1} sx={{ mt: 3 , height: "160px"}}>
          <Box>
            <Box sx={{ px: 6, py: 5, display: 'flex', justifyContent: 'space-between' }}>
              <FormControl sx={{ width: '580px' }}>
                <InputLabel required id='demo-simple-select-label'>
                  Location
                </InputLabel>
                <Select
                  labelId='demo-simple-select-label'
                  id='demo-simple-select'
                  value={value}
                  label='Location'
                  onChange={handleChanges}
                >
                  <MenuItem value='Abu Dhabi'>Abu Dhabi</MenuItem>
                  <MenuItem value='Sharjah'>Sharjah</MenuItem>
                  <MenuItem value='Al Mankhool'>Al Mankhool</MenuItem>
                  <MenuItem value='Satwa'>Satwa</MenuItem>
                  <MenuItem value='Kuwait'>Kuwait</MenuItem>
                  <MenuItem value='Saudi Arabia'>Saudi Arabia</MenuItem>
                  <MenuItem value='Lebanon'>Lebanon</MenuItem>
                  <MenuItem value='Jordan'>Jordan</MenuItem>
                  <MenuItem value='Egypt'>Egypt</MenuItem>
                  <MenuItem value='Bahrain'>Bahrain</MenuItem>
                  <MenuItem value='UAE'>UAE</MenuItem>
                  <MenuItem value='Tunisia'>Tunisia</MenuItem>
                  <MenuItem value='Morocco'>Morocco</MenuItem>
                  <MenuItem value='Bhopal'>Bhopal</MenuItem>
                </Select>
              </FormControl>

              <FormControl sx={{ width: '580px' }}>
                <InputLabel required id='demo-simple-select-label'>
                  Signing Type
                </InputLabel>
                <Select
                  labelId='demo-simple-select-label'
                  id='demo-simple-select'
                  value={value}
                  label='Signing Type'
                  onChange={handleChanges}
                >
                  <MenuItem value='Sign In'>Sign In</MenuItem>
                  <MenuItem value='Sign Out'>Sign Out</MenuItem>
                </Select>
              </FormControl>
            </Box>
            <Box>
              {/* <Divider sx={{ my: 4 }} /> */}
              <Button variant='contained' sx={{ float: 'right', mx: 6 , width: "150px"}}>
                Save
              </Button>
            </Box>
          </Box>
        </Paper>
      </Box>
    </div>
  )
}

export default RemoteSignInOut
