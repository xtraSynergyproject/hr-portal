import React from 'react'
import { Typography } from '@mui/material';
import {Grid,Button} from '@mui/material'

import Card from '@mui/material/Card'
import MyPieChart from './components/MyPieChart'
// import CompletedTable from './components/mytable/CompletedTable'
import PendingTable from './components/mytable/PendingTable'

function MyTask() {
  return (
    <>
    <Typography variant="h5" component="h2">Dashboard</Typography>
     <MyPieChart/> 
    {/*<Button sx={{mt:30}}>Filter</Button> */}
     {/* <CompletedTable/> */}
     <PendingTable/>


    </>
  )
}

export default MyTask
