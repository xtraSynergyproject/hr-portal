import React from 'react'
import { Paper, Box, Typography, Divider } from '@mui/material/'

import PayrollTab from './components/PayrollTab'
import UserProfile from 'src/views/dashboards/payroll/components/UserProfile'
import PayrollEditFormModule from './components/Form/PayrollEditFormModule'
import PayrollAddEmpForm from './components/Form/PayrollAddEmpForm'
 

function PayrollMain() {
  return (
    <div>
      <Paper elevation={4}>
        <Box className='prl_main_box' sx={{ px: 4, py: 6, display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant='h5' component='h2'>
            Payroll
          </Typography>
          <Box className='prl_btn_box' sx={{ display: 'flex' }}>
            <PayrollAddEmpForm  />
            <PayrollEditFormModule  />
          </Box>
        </Box>
        <Divider sx={{ mb: 0 }} />
        <UserProfile />
      </Paper>
      <PayrollTab />
    </div>
  )
}

export default PayrollMain
