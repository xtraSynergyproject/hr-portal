
import React from 'react';
import {Box, Grid,Button} from '@mui/material';
import Icon from 'src/@core/components/icon';
import Fab from '@mui/material/Fab';
 // ** MUI Imports
 import TextField from '@mui/material/TextField'
 import Autocomplete, { createFilterOptions } from '@mui/material/Autocomplete';
 import CreateNoteModal from './modals/CreateNoteModal';
 
 // ** Data
 import { top100Films } from 'src/@fake-db/autocomplete';
import MainViewModal from './modals/note/viewnotemodal/MainViewModal';
 const filterOptions = createFilterOptions({
  matchFrom: 'start',
  stringify: option => option.title
})
 



export default function WorklistCreateNotes() {
  return (
    <>
      <Box >
    <Box
    sx={{
      width:'53rem',
      height: '13vh',
      backgroundColor: '#A9A9A9',
      borderRadius:'5px',
      mt:8,       
    }}>
      
    <Grid contained>
    <Box sx={{display:'flex', justifyContent: 'space-between'}}>    

    <Grid item xs={9}>

  <Box sx={{ width:'43rem',  margin: '6px'
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
       renderInput={params => <TextField {...params} label='Select Note Template...' />}
    />
  


  </Box>
        </Grid>
        <Grid item xs={3}>  
        <Box>
            <MainViewModal/>
            <CreateNoteModal />
        </Box>       
        </Grid>
        </Box>

    </Grid>
   
  </Box>
    

    
</Box>
    </>
  )
}

