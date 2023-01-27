import React from 'react'
import Quick from '../Project3/Components/Quick'
import PermissionsTable from '../Project3/Components/PermisonTable'
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
    
    <Quick/>
    <PermissionsTable/>
    
    </>
    

  )
}

export default TimePermison
