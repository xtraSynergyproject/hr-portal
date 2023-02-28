import { Typography } from '@mui/material';
import React from 'react';
import {Grid} from '@mui/material'
// import MyChart from  './components/MyChart'
// import MyChart1 from './components/MyChart1';
 import ServiceChart from './components/ServiceChart'
// import StaticCard from './components/StaticCard'

function MyService() {
  return (
    <div>
      <Typography variant="h5" component="h2">Dashboard</Typography>
       <ServiceChart/> 
      {/* <StaticCard/> */}
    </div>
  )
}

export default MyService
