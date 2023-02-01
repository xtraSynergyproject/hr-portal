import React from 'react'
import BussinessInput from './Components/BussinessInput'
import Model from '../Project2/Components/Model'
import BussinessTripProfile from '../Project2/Components/BussinessTripProfile'
import BussinessTable from './Components/BussinessTable'
import Grid from '@mui/material/Unstable_Grid2';



function Bussiness() {
  return (

    <>

<Grid container spacing={2}>
  <Grid item xs={12} md={10}>
    <BussinessInput/>
  </Grid>
  <Grid item xs={12} md={2}>
   <Model/>
  </Grid>
  

    </Grid>


      <BussinessTripProfile/>
      <BussinessTable />

    </>



  )
}

export default Bussiness
