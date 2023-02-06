// ** Next Imports
import Link from 'next/link'

// ** MUI Components
import Box from '@mui/material/Box'

// ** Utils Import
import { getInitials } from 'src/@core/utils/get-initials'

// ** Custom Components Imports
import CustomAvatar from 'src/@core/components/mui/avatar'
import { Paper } from '@mui/material'


const Projects = ({ data }) => {
  return (
   <Box  sx={{display:"flex", justifyContent:"space-evenly"}}>
<Box>
  <Paper  elevation={1} sx={{height:"40vh",width:"20vw",overflow:"scroll"}}>
Box1
  </Paper>
</Box>
<Box>
  <Paper  elevation={1} sx={{width:"20vw", height:"40vh", overflow:"scroll"}}>
Box2
  </Paper>
</Box>
  
<Box>
  <Paper  elevation={1} sx={{width:"20vw", height:"40vh", overflow:"scroll"}}>
Box3
  </Paper>
</Box>

   </Box>
  )
}

export default Projects
