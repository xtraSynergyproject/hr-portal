import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import CardMedia from '@mui/material/CardMedia'
import MyTaskSidebar from './MyTaskSidebar'
import { Typography } from '@mui/material'
import TaskCallPage from './TaskCallPage'
import TaskTotalProfitcard from './TaskTotalProfitcard'


const TaskHomePage = ({ folder, label }) => {

  return (
    <Box sx={{ height: 820, width: 950, background: 'white', transform: 'translateZ(0px)', flexGrow: 1 }}>

      <Box>
        <Typography sx={{ fontFamily: "sans-serif", fontSize: "30px", display: 'flex', marginLeft: '20px' }}><h5>Task Home</h5></Typography>
        <Card>
          <CardMedia
            component='img'
            alt='Coding'
            image='https://contentstatic.timesjobs.com/img/64858386/Master.jpg'
            sx={{
              height: "150px"
            }}
          />
        </Card>


      </Box>

      <Box
        sx={{
          width: '100%',
          display: 'flex',
          borderRadius: 1,
          overflow: 'hidden',
          position: 'relative',

        }}
      >
        <MyTaskSidebar />
        <div>
          <TaskCallPage />
          <TaskTotalProfitcard />
        </div>
      </Box>
    </Box>
  )
}

export default TaskHomePage
