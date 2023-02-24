import { Box, Card, Typography } from '@mui/material'
import { Divider } from '@mui/material'
import CardContent from '@mui/material/CardContent'
import { Fragment, useState, useEffect } from 'react'
import axios from 'axios'

function DashBoard(ServiceApproachingViolationInaMin, ServiceApproachingViolation) {
  return {
    ServiceApproachingViolationInaMin,
    ServiceApproachingViolation
  }
}

const CardInfluencer = () => {
  const [data, setData] = useState([])
  useEffect(() => {
    axios.get('https://webapidev.aitalkx.com/tms/query/HelpdeskDashboard').then(response => {
      setData(response.data)
      console.log(response.data, 'ServiceApproachingViolationInaMin data')
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
        <Typography sx={{ color: 'black' }}>RequestsApprochingSLAViolations</Typography>
      </CardContent>
      <Typography variant='body2' sx={{ mb: 3.25 }}>
        <Divider />
        <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: '30px',mt: '30px', color: 'black' }}>
          <Typography variant='' sx={{ mb: 2 }}>
            60Min
          </Typography>
          <Typography sx={{ mb: 2 , color: 'red'}}>{data.ServiceApproachingViolationInaMin}</Typography>
          <Typography variant='' sx={{ mb: 2 }}>
            Other
          </Typography>
          <Typography sx={{ mb: 2 }}>{data.ServiceApproachingViolation}</Typography>
        </Box>
      </Typography>
    </Card>
  )
}

export default CardInfluencer
