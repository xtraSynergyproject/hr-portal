import React from 'react'
import Box from '@mui/material/Box'
import Paper from '@mui/material/Paper'
import Divider from '@mui/material/Divider'
import Typography from '@mui/material/Typography'
import PayrollProfileDetails from 'src/views/dashboards/payroll/components/PayrollProfileDetails'
import EducationalReimbTable from './components/EducationalReimbTable'
import EducationalReimbModal from './components/EducationalReimbModal'


function EducationalReimbursement() {
  return (
    <div>
          <Paper elevation={4}>
        <Box sx={{ px: 4, py: 6, display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant='h4' component='h2'>
            Educational Reimbursement
          </Typography>
          <EducationalReimbModal/>
        </Box>
        <Divider sx={{mb:0}}/>
          <PayrollProfileDetails />
        </Paper>
        <EducationalReimbTable/>
    </div>
  )
}

export default EducationalReimbursement







