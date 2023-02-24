import React from 'react'

//MUI Imports
import Box from '@mui/material/Box'
import Paper from '@mui/material/Paper'
import Typography from '@mui/material/Typography'

//Component Imports
import UserProfile from 'src/views/dashboards/payroll/components/UserProfile'
import { Divider } from '@mui/material'
import MisconductsModal from './MisconductsModal'
import MisconductsTable from './MisconductsTable'

function Misconducts() {
  return (
    <div>
      <Paper elevation={4}>
        <Box sx={{ px: 4, py: 6, display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant='h5' component='h2'>
            Misconducts
          </Typography>
          <MisconductsModal/>
        </Box>
        <Divider sx={{mb:0}}/>
          <UserProfile />
        </Paper>

      <MisconductsTable/>
    </div>
  )
}

export default Misconducts













