import React from 'react'
import UserProfile from 'src/views/dashboards/payroll/components/UserProfile'
import AccessCalender from './components/AccessCalender'
import AccessTable from './components/AccessTable'
import Box from '@mui/material/Box'

function AccessLogs() {
  return (
    <div>
       
      <UserProfile/>
      <Box sx={{ paddingleft: '80px' }}>

      <h4 >AccessLog</h4></Box>
      <AccessCalender/> 
      <AccessTable/>
    </div>
  )
}

export default AccessLogs
