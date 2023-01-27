import React from 'react'

//MUI Imports
import Box from '@mui/material/Box'
import Paper from '@mui/material/Paper'
import Typography from '@mui/material/Typography'

//Component Imports
import TerminationTable from './components/TerminationTable'
import TerminationRequestModal from './components/TerminationRequestModal'
import PayrollProfileDetails from '../../payroll/components/PayrollProfileDetails'
import { Divider } from '@mui/material'

function Termination() {
  return (
    <div>
      <Paper elevation={4}>
        <Box sx={{ px: 4, py: 6, display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant='h4' component='h2'>
            Termination
          </Typography>
          <TerminationRequestModal />
        </Box>
        <Divider sx={{mb:0}}/>
          <PayrollProfileDetails />
        </Paper>

      <TerminationTable />
    </div>
  )
}

export default Termination
