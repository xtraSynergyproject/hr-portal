import React from 'react'
import Box from '@mui/material/Box'
import Paper from '@mui/material/Paper'
import Typography from '@mui/material/Typography'

//Component Imports
import PayrollProfileDetails from '../../payroll/components/PayrollProfileDetails'
import { Divider } from '@mui/material'
import TransferReqTable from './components/TransferReqTable'
import AddTransferReqModal from './components/AddTransferReqModal'

function TransferRequest() {
  return (
    <div>
      <Paper elevation={4}>
        <Box sx={{ px: 4, py: 6, display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant='h4' component='h2'>
            Misconducts
          </Typography>
          <AddTransferReqModal />{' '}
        </Box>
        <Divider sx={{ mb: 0 }} />
        <PayrollProfileDetails />
      </Paper>

      <TransferReqTable />
    </div>
  )
}

export default TransferRequest
