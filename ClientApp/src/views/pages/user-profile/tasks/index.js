// ** MUI Components
import Box from '@mui/material/Box'

// ** Custom Components Imports
import { Paper, Typography } from '@mui/material'
import Cards1 from './Cards1'
import Cards2 from './Cards2'
import Cards3 from './Cards3'

const Projects = ({ data }) => {
  return (
    <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
      <Box>
        <Typography variant='h6'>Draft</Typography>
        <Paper elevation={1} sx={{ height: '50vh', width: '22vw', overflow: 'scroll' }}>
          <Cards1 />
          <br />
          <Cards1 />
          <br />
          <Cards1 />
        </Paper>
      </Box>
      <Box>
      <Typography variant='h6'>In Progress</Typography>
        <Paper elevation={1} sx={{ height: '50vh', width: '22vw', overflow: 'scroll' }}>
          <Cards2 />
          <Cards2 />
          <Cards2 />
        </Paper>
      </Box>

      <Box>
       <Typography variant='h6'>Completed</Typography>
        <Paper elevation={1} sx={{ height: '50vh', width: '22vw', overflow: 'scroll' }}>
          <Cards3 />
          <Cards3 />
          <Cards3 />
        </Paper>
      </Box>
    </Box>
  )
}

export default Projects
