

import * as React from 'react';
import { styled } from '@mui/material/styles';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Grid from '@mui/material/Grid';
import Button from '@mui/material/Button';


const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: 'center',
  color: theme.palette.text.secondary,
}));

export default function AutoGrid() {
  return (
    <Box sx={{ height: 600, width: 999, background: 'white', transform: 'translateZ(0px)', flexGrow: 1 }}>
    
      <Grid container spacing={5}>
     
        <Grid item xs={3}>
        <Box
  sx={{
    width: 200,
    height: 150,
    backgroundColor: 'primary.dark',
    
    '&:hover': {
      backgroundColor: 'primary.main',
      opacity: [0.9, 0.8, 0.7],
    
    },
  }}
/>
<Button  size="medium">
          Time Permisson Busisness
        </Button>



        </Grid>
        <Grid item xs={4}>
        <Box
  sx={{
    width: 200,
    height: 150,
    backgroundColor: 'primary.dark',
    '&:hover': {
      backgroundColor: 'primary.main',
      opacity: [0.9, 0.8, 0.7],
    },
  }}
/>
<Button  size="medium">
   Time Permisson Personal
          
        </Button>


        </Grid>
      </Grid>
    </Box>
    
  );
}





















  
