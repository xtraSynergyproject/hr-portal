import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import CardMedia from '@mui/material/CardMedia'
import RequestSidebarLeft from './RequestSidebarLeft'
import { Typography } from '@mui/material'
import RequestMainPage from './RequestMainPage'
import TotalRequest from './TotalRequest'

const RequestImg = ({ folder, label }) => {
  return (
    <Box sx={{ height: 820, width: 950, background: 'white', transform: 'translateZ(0px)', flexGrow: 1 }}>

      <Box>
        <Typography sx={{ fontFamily: "sans-serif", fontSize: "30px" }}>Services Home</Typography>
        <Card>
          <CardMedia
            component='img'
            alt='Coding'
            image='https://images.unsplash.com/photo-1542831371-29b0f74f9713?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=870&q=80'
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
        <RequestSidebarLeft />
        <Box sx={{ display: "flex", flexDirection: "column" }}>
          <RequestMainPage />
          < TotalRequest sx={{ mt: 3 }} />
        </Box>
      </Box>
    </Box>
  )
}

export default RequestImg
