import React from 'react'
import SelectData from './SelectData'
import WeekDisplayName from './WeekDisplayName'

function RosterSchedule() {
  return (
    <div><h1>Roster Schedule</h1>
    <Box sx={{display: 'flex'}}>
    <SelectData />
    <WeekDisplayName />
    </Box>
    </div>
  )
}
export default RosterSchedule
