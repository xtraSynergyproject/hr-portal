import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import CardMedia from '@mui/material/CardMedia'
// import RequestSidebarLeft from './RequestSidebarLeft'
import { Typography } from '@mui/material'
// import RequestMainPage from './RequestMainPage'
// import Pagination from './Pagination'

const RequestImg = ({ folder, label }) => {
  return (
    <Box sx={{ marginTop: '-60px' }}>
      <Box>
        {/* <Typography sx={{ fontFamily: 'sans-serif', fontSize: '30px', display: 'flex', marginLeft: '20px' }}>
          <h5>Note Home</h5>
        </Typography> */}
        <Card>
          <CardMedia
            component='img'
            alt='Coding'
            image='https://images.unsplash.com/photo-1542831371-29b0f74f9713?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=870&q=80'
            sx={{
              height: '6rem',
              mt:8,
            }}
          />
            {/* <Typography sx={{color:'white'}}>Task Home</Typography> */}

           {/* </CardMedia>  */}
        </Card>
      </Box>

      <Box
        sx={{
          width: '100%',
          display: 'flex',
          borderRadius: 1,
          overflow: 'hidden',
          position: 'relative'
        }}
      >
        {/* <RequestSidebarLeft /> */}
        <Box sx={{ display: 'flex', flexDirection: 'column' }}>
          {/* <RequestMainPage /> */}
          {/* <Pagination /> */}
        </Box>
      </Box>
    </Box>
  )
}

export default RequestImg