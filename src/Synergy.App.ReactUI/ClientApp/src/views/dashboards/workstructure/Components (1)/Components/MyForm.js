import React from 'react'
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';


function MyForm() {
  return (
    <div>
      
      <TextField required id='outlined-required' label='Location' />
      

      <TextField required id='outlined-required' label='Location (in Arabic)' />
      
      
      <TextField required id='outlined-required' label='Description' />
     
      
      <Button variant="contained">Submit</Button>
      
    </div>
  )
}

export default MyForm
