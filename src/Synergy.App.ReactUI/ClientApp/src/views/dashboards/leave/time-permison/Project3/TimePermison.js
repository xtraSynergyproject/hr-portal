import React from 'react'
import TimePermisonProfile from './Components/TimePermissonProfile'
import TimePermissionsTable from './Components/TimePermisonTable'
import TimeSelect from '../Project3/Components/TimeSelect'
import Model from '../../bussiness-trip/Project2/Components/Model'
import Grid from '@mui/material/Unstable_Grid2';



function  TimePermison() {
  return (
    
    <>


<Grid container spacing={8}>
  <Grid xs={10}>
  <TimeSelect/>
  
    
  </Grid>
  <Grid xs={2}>
  <Model/>
  
  </Grid>
  
  
  

    </Grid>
    
    <TimePermisonProfile/>
    <TimePermissionsTable/>
    
    </>
    

  )
}

export default TimePermison
