import React from 'react'
import SelectData from './SelectData'
import WeekDisplayName from './WeekDisplayName'
import PublishDate from '../PublishDate'
import { Box } from '@mui/material'
import RouterTable from './RouterTable'
function RosterSchedule() {
  return (
    <div>
      <h1>Roster Schedule</h1>
      <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
        <SelectData />

        <Box>
          <WeekDisplayName />
        </Box>
      </Box>
      <PublishDate />
      <RouterTable/>
   
    </div>
  )
}
export default RosterSchedule
