import Box from '@mui/material/Box'
import RequestsByCategory from './RequestsByCategory'
import RequestsSummary from './RequestsSummary'
import TaskSummary from './TaskSummary'
import OpenRequestsbyCategory from './OpenRequestsbyCategory'
import SLAViolationByCategory from './SLAViolationByCategory'
import SLAVcard from './SLAVcard'
import OpenRequests from './OpenRequests'
import SLAViolated from './SLAViolated'
import Received from './Received'
import Closed from './Closed'

const CardAppleWatch = () => {
  return (
    <div>
      <Box>
        <Box sx={{display: 'flex', gap: '10px'}}>
        <RequestsByCategory />
        <RequestsSummary />
        </Box>
        <Box sx={{display: 'flex', gap: '10px', mt: '5px'}}>
        <TaskSummary />
        <OpenRequestsbyCategory />
        </Box>
        <Box sx={{display: 'flex', gap: '10px', mt: '5px'}}>
        <SLAViolationByCategory />
        <SLAVcard />
        </Box>
        <Box sx={{display: 'flex', gap: '10px', mt: '5px'}}>
        <OpenRequests />
        <SLAViolated />
        </Box>
        <Box sx={{display: 'flex', gap: '10px', mt: '5px'}}>
        <Received />
        <Closed />
        </Box>
        
      </Box>
    </div>
  )
}

export default CardAppleWatch
