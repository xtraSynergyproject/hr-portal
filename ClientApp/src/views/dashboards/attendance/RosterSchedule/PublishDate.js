//import { Button} from '@mui/material'
import { Typography } from '@mui/material'
import { Box } from '@mui/system'
import React from 'react'
import { Css } from '@mui/icons-material' 


function PublishDate() {
  return (
    <>
      <Box  sx={{display:"flex",justifyContent :"space-between"}}>
        <Box>PublishDate:* Unpublished(draft)roster can not be used by syste and will not be visible to employees</Box>
        <Box>
          <input type='checkbox' name='select all day' />
          Select All Day
        </Box>
        <Typography sx={{backgroundColor:'darkseagreen', p:"2px", borderRadius:"5px"}}>Draft Roster</Typography>
        <Typography sx={{backgroundColor:'blueviolet', p:"2px", borderRadius:"5px"}}>Publish Roster</Typography>
        <Typography sx={{backgroundColor:'hotpink', p:"2px", borderRadius:"5px"}}>Day Off</Typography>
        <Typography sx={{backgroundColor:'brown', p:"2px", borderRadius:"5px"}}>Leave</Typography>
        <Typography sx={{backgroundColor:'burlywood', p:"2px", borderRadius:"5px"}}>Public Holiday</Typography>
      </Box>
    </>
  )
}

export default PublishDate
