import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Stack from '@mui/material/Stack';
import { deepOrange, green } from '@mui/material/colors';
import AssignmentIcon from '@mui/icons-material/Assignment';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Grid from '@mui/material/Grid';
import { styled } from '@mui/material/styles';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import WorklistSearch from '../Dashboard/WorklistSearch'
import WorklistModal from '../Dashboard/WorklistModal'










export default function VariantAvatars() {
  return (
    

<Grid  container
  direction="row"
  justifyContent="center"
  alignItems="center"
  margin="15px"
  
  
  >

<Grid item xs={3}>

<typography>Task </typography>

</Grid>


  <Grid item xs={3}>
  <typography>AssignedtoMe </typography>
    <Stack direction="row" spacing={2}>
    
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        69
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        60
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        110
      </Avatar>
     
      
    </Stack>
  </Grid>
  <Grid item xs={3}>

  <typography>RequestedbyMe </typography>
     <Stack direction="row" spacing={2}>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        69
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        60
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        110
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        110
      </Avatar>
     
      
    </Stack>
  </Grid>
  <Grid item xs={3}>

  <typography>SharedwithMe/Team </typography>
     <Stack direction="row" spacing={2}>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        69
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        60
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        110
      </Avatar>
     
      
    </Stack>
  </Grid>

  <Grid container rowSpacing={1} columnSpacing={{ xs: 1, sm: 2, md: 3 }}>
  <Grid item xs={6}>
  <WorklistSearch/>
  </Grid>
  <Grid item xs={6}>
  <WorklistModal/>
  </Grid>





</Grid>

<Grid  container
  direction="row"
  justifyContent="center"
  alignItems="center"
  margin="15px"
  
  
  >

<Grid item xs={12}>

<typography>Service  </typography>

</Grid>


  <Grid item xs={3}>
  <typography>
RequestedbyMe</typography>
    <Stack direction="row" spacing={2}>
    
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        69
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        60
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        110
      </Avatar>
     
      
    </Stack>
  </Grid>
  <Grid item xs={3}>

  <typography>SharedwithMe/Team </typography>
     <Stack direction="row" spacing={2}>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        69
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        60
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        110
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        110
      </Avatar>
     
      
    </Stack>
  </Grid>
 








</Grid>

<Grid container rowSpacing={1} columnSpacing={{ xs: 1, sm: 2, md: 3 }}>
  <Grid item xs={6}>
  <WorklistSearch/>
  </Grid>
  <Grid item xs={6}>
  <Button variant="contained" >+Create Service</Button>
  </Grid>





</Grid>


<Grid  container
  direction="row"
  justifyContent="center"
  alignItems="center"
  margin="15px"
  
  
  >

<Grid item xs={3}>

<typography>Task </typography>

</Grid>


  <Grid item xs={3}>
  <typography>AssignedtoMe </typography>
    <Stack direction="row" spacing={2}>
    
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        69
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        60
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        110
      </Avatar>
     
      
    </Stack>
  </Grid>
  <Grid item xs={3}>

  <typography>RequestedbyMe </typography>
     <Stack direction="row" spacing={2}>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        69
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        60
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        110
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        110
      </Avatar>
     
      
    </Stack>
  </Grid>
  <Grid item xs={3}>

  <typography>SharedwithMe/Team </typography>
     <Stack direction="row" spacing={2}>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        69
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        60
      </Avatar>
      <Avatar sx={{ color: deepOrange[500] }} variant="square">
        110
      </Avatar>
     
      
    </Stack>
  </Grid>
  </Grid>



</Grid>


  
  




  










 




   
  );
}





