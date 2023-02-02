import React from 'react'
import TimePermisonProfile from './Components/TimePermissonProfile'
import TimePermissionsTable from './Components/TimePermisonTable'
import TimeSelect from '../Project3/Components/TimeSelect'
import Model from '../../bussiness-trip/Project2/Components/Model'
import Grid from '@mui/material/Unstable_Grid2';
import { styled } from '@mui/material/styles';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';

const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: 'center',
  color: theme.palette.text.secondary,
}));



function  TimePermison() {
  return (
    
    <>


<Grid container spacing={2}>
  <Grid item xs={12} md={10}>
    <TimeSelect/>
  </Grid>
  <Grid item xs={12} md={2}>
   <Model/>
  </Grid>
  
  
  

    </Grid>
    
    <TimePermisonProfile/>
    <TimePermissionsTable/>
    
    </>
    

  )
}

export default TimePermison
