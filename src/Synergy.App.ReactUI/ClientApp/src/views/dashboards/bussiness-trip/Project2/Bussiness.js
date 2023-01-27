import React from 'react'
import BuniSelect from '../Project2/Components/BusnisSelect'
import Model from '../Project2/Components/Model'
import TableShow from '../Project2/Components/TableShow'
import DataTable from '../Project2/Components/DataTable'
import Grid from '@mui/material/Unstable_Grid2';


function  Bussiness() {
  return (
    
    <>







<Grid container spacing={8}>
  <Grid xs={10}>
  <BuniSelect/>
    
  </Grid>
  <Grid xs={2}>
  <Model/>
  
  </Grid>
  
  
  

    </Grid>

    
    
    <TableShow/>
    <DataTable/>
      </>
    

  )
}

export default Bussiness
