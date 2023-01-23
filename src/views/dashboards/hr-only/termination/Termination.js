import React from 'react'

//MUI Imports
import Box from '@mui/material/Box'
import Paper from '@mui/material/Paper'
import Typography from '@mui/material/Typography'

//Component Imports
import TerminationTable from './components/TerminationTable'
import TerminationRequestModal from './components/TerminationRequestModal';


function Termination() {
  return (
    <div>
      <Paper elevation={4}>
        <Box>
            <Typography variant='h3' component='h2'>Termination</Typography>
<TerminationRequestModal/>
        </Box>
      </Paper>
      <TerminationTable />
    </div>
  )
}

export default Termination
