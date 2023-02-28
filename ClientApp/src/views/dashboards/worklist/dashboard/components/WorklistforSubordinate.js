import React from 'react';
import { Box, Paper } from '@mui/material';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Autocomplete, { createFilterOptions } from '@mui/material/Autocomplete';

// ** Data
import { top100Films } from 'src/@fake-db/autocomplete'
// import CreateTaskModal from './modals/CreateTaskModal';

 const filterOptions = createFilterOptions({
  matchFrom: 'start',
  stringify: option => option.title
})
 

function WorklistforSubordinate() {
    return (
        <>
            <Box sx={{ justifyContent: 'space-between' }}>
                <Typography id="modal-modal-title" variant="h6" component="h2" sx={{ mt:5, mb:2 }}>
                    WorklistforSubordinate
                </Typography>
                <Box>
                    <Box

                        sx={{
                            width: '53rem',
                            height: '9vh',
                            backgroundColor: '#0179a8',
                            borderRadius: '5px',
                            boxShadow:"5px 5px 10px rgba(255,255,255,0.8)",

                        }}
                    >
                        <Box sx={{ display: 'flex' }}>
                            <Box sx={{ ml:5, mt:4, color: '#DCDCDC', fontSize:14 }}>
                                SelectUser
                            </Box>
                            <Box sx={{ width:'13rem',ml:4, mt:2}} >

                            <Autocomplete
                                sx={{
                                    height: '6.2vh', bgcolor: 'background.paper',
                                    borderRadius:1,
                                    boxShadow:"2px rgba(255,255,0,0.8)",

                                    color: (theme) =>
                                        theme.palette.getContrastText(theme.palette.background.paper),
                                }}
                                options={top100Films}
                                filterOptions={filterOptions}
                                size='small'
                                id='autocomplete-custom-filter'
                                getOptionLabel={option => option.title}
                                renderInput={params => <TextField {...params} label='..Select..' />}
                            />
                            </Box>
                            {/* <Box sx={{ colo:'#DCDCDC', ml:3,flex:2}}>
                                {/* <Paper elevation={1} sx={{size:'small'}}> 
                            <TextField
                                id="outlined-select-currency"
                                select
                                //   label="Select"
                                size='small'
                                defaultValue="EUR"
                            />
                             </Paper> 
                        </Box> */}
                    </Box>
                </Box>

            </Box>
        </Box>
        </>
    )
}



export default WorklistforSubordinate
