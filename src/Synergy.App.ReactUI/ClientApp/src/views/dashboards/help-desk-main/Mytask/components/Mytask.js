import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import CardMedia from '@mui/material/CardMedia'
import HelpDiskSidebar from './HelpDiskSidebar'
import { Typography } from '@mui/material'
import MyTaskMain from './MyTaskMain'
import TotalProfit from './TotalProfit'


const Mytask = ({ folder, label }) => {

  return (
    <Box sx={{ height: 820, width: 950, background: 'white', transform: 'translateZ(0px)', flexGrow: 1 }}>

      <Box>
        <Typography sx={{ fontFamily: "sans-serif", fontSize: "50px" }}>Task Home</Typography>
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
        <HelpDiskSidebar />
        <div>
          <MyTaskMain />
          <TotalProfit />
        </div>
      </Box>
    </Box>
  )
}

export default Mytask
