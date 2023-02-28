import React from 'react';
import {Box, Grid,Button} from '@mui/material';
 // ** MUI Imports
 import TextField from '@mui/material/TextField'
 import Autocomplete, { createFilterOptions } from '@mui/material/Autocomplete';
 import CreateServiceModal from './modals/CreateServiceModal';

 // ** Data
 import { top100Films } from 'src/@fake-db/autocomplete'

 const filterOptions = createFilterOptions({
  matchFrom: 'start',
  stringify: option => option.title
})
 

function WorklistCreateService() {
  return (
    <>
    <Box >
    <Box
    sx={{
      width: '53rem',
      height: '9vh',
      backgroundColor: '#A9A9A9',
      borderRadius:'5px',
      mt:5,       
    }}>
      
    <Grid contained>
    <Box sx={{display:'flex', justifyContent: 'space-between'}}>    

    <Grid item xs={9}>

  <Box sx={{ width:'43rem', m:2
}}>
    <Autocomplete 
    sx={{height:'6.5vh',bgcolor: 'background.paper', borderRadius:1,
    color: (theme) =>
      theme.palette.getContrastText(theme.palette.background.paper),
 }}
      options={top100Films}
      filterOptions={filterOptions}
      size='small'
      id='autocomplete-custom-filter'
      getOptionLabel={option => option.title}
       renderInput={params => <TextField {...params} label='Select Service Templates...' />}
    />
  


  </Box>
        </Grid>
        <Grid item xs={3}>  
        {/* <Box sx={{mt:2,mr:'10px'}}> */}
          <CreateServiceModal/>
        {/* </Box>        */}
        </Grid>
        </Box>

    </Grid>
   
  </Box>
    

    
</Box>    
    </>
  )
}

export default WorklistCreateService
