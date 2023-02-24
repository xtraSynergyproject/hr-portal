import Card from '@mui/material/Card'
import { Divider,Box } from '@mui/material'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import { Fragment, useState, useEffect } from 'react'
import axios from 'axios'

function DashBoard(OpenRequestCount) {
  return {
    OpenRequestCount
  }
}

const CardInfluencer = () => {
  const [data, setData] = useState([])
  useEffect(() => {
    axios.get('https://webapidev.aitalkx.com/tms/query/HelpdeskDashboard').then(response => {
      setData(response.data)
      console.log(response.data, 'OpenRequestCount data')
    })
  }, [])
  return (
    <Card sx={{ width: '100%', height: '20rem', border: '1px solid' }}>
      <CardContent
        sx={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          backgroundColor: 'lightgrey',
          fontWeight: 400,

          height: '40px',
          width: '100%'
        }}
      >
        <Typography sx={{ color: 'black' }}>Open Requests</Typography>
      </CardContent>
      <Typography variant='body2' sx={{ mb: 3.25 }}>
        <Divider />
        <Box sx={{ display: 'flex', justifyContent: 'center',mt: '30px', gap: '10px', color: 'black' }}>
        <Typography variant=''>
          Open
        </Typography>
        <Typography sx={{ mb: 2 }}>{data.OpenRequestCount}</Typography>
        </Box>
      </Typography>
    </Card>
  )
}

export default CardInfluencer
