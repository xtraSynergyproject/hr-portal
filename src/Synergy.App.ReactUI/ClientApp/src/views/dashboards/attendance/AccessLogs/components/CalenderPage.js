// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import Box from '@mui/material/Box'

// ** Third Party Imports
import TextField from '@mui/material/TextField'

const CalenderPage = ({ popperPlacement }) => {
  // ** States
  const [minDate, setMinDate] = useState(new Date())
  const [maxDate, setMaxDate] = useState(new Date())

  return (
    <Box sx={{ display: "flex", alignItems: 'center', gap: '70px' }}>
      <div>
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
      </div>
      <div>
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
      </div>
    </Box>
  )
}

export default CalenderPage
