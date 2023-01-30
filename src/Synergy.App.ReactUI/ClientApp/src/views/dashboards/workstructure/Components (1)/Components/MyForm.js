import React from 'react'
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Box from  '@mui/material/Box';


function MyForm() {
  return (
    <Box>
      <div>
      <TextField required id='outlined-required' label='Location' sx={{width:"100%"}}/>
      </div>
      
  <div>
      <TextField required id='outlined-required' label='Location (in Arabic)'sx={{width:"100%",mt:2}}/>
     </div> 
      <div>
      <TextField required id='outlined-required' label='Description'sx={{width:"100%",mt:2}} />
     </div>
      <div>
      <Button variant="contained" sx={{width:"50%",mt:2,ml:5}}>Submit</Button>
      </div>
    </Box>
  )
}

export default MyForm
