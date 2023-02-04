import React from 'react'
import Box from '@mui/material/Box'
import Paper from '@mui/material/Paper'
import Divider from '@mui/material/Divider'
import Typography from '@mui/material/Typography'
import PayrollProfileDetails from 'src/views/dashboards/payroll/components/PayrollProfileDetails'
import MedicalReimbTable from './components/MedicalReimbTable'
import MedicalReimbModal from './components/MedicalReimbModal'


function MedicalReimbursement() {
  return (
    <div>
          <Paper elevation={4}>
        <Box sx={{ px: 4, py: 6, display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant='h4' component='h2'>
            Medical Reimbursement
          </Typography>
          <MedicalReimbModal/>
        </Box>
        <Divider sx={{mb:0}}/>
          <PayrollProfileDetails />
        </Paper>
        <MedicalReimbTable/>
    </div>
  )
}

export default MedicalReimbursement