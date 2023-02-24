import React from 'react'
import Box from '@mui/material/Box'
import Paper from '@mui/material/Paper'
import Divider from '@mui/material/Divider'
import Typography from '@mui/material/Typography'
import UserProfile from 'src/views/dashboards/payroll/components/UserProfile'
import TravelReimbTable from './components/TravelReimbTable'
import TravelReimbModal from './components/TravelReimbModal'


function TravelReimbursement() {
  return (
    <div>
          <Paper elevation={4}>
        <Box sx={{ px: 4, py: 6, display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant='h5' component='h2'>
            Travel Reimbursement
          </Typography>
          <TravelReimbModal/>
        </Box>
        <Divider sx={{mb:0}}/>
          <UserProfile />
        </Paper>
        <TravelReimbTable/>
    </div>
  )
}

export default TravelReimbursement