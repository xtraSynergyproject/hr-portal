
import * as React from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button'


export default function Personalinfo() {
  return (
    <Box
      component="form"
      sx={{
        '& .MuiTextField-root': { m: 2, m: 3, width: '540px' }

      }}

      noValidate
      autoComplete="off"
    >

      <div>

        <TextField
          label="Person Name"
          id="outlined-size-small"

          size="small"
        />

        <TextField

          label=" Full Name"
          id="outlined-size-small"

          size="small"
        />

      </div>
      <div>
        <TextField
          label="Email id"
          id="outlined-size-small"

          size="small"
        />


        <TextField
          id="outlined-number"
          label="Number"
          type="text"
          InputLabelProps={{
            shrink: true,
          }}
          size="small"
        />

        <TextField
        
          label="Title"
          id="outlined-size-small"

          size="small"
        />
        <TextField
          label="Contact Country Name"
          id="outlined-size-small"

          size="small"
        />
        <TextField
          label="Unit Name"
          id="outlined-size-small"

          size="small"
        />
        <TextField
          label=" Full Name"
          id="outlined-size-small"

          size="small"
        />
        <TextField
          label="Nationality Name"
          id="outlined-size-small"

          size="small"
        />
        <TextField
          label="Religin"
          id="outlined-size-small"

          size="small"
        />
        <TextField
          id="outlined-number"
          label=" Unit Number"
          type="text"
          InputLabelProps={{
            shrink: true,
          }}
          size="small"
        />
        <TextField
          id="outlined-Number"
          label="Building Number"
          type="text"
          InputLabelProps={{
            shrink: true,
          }}
          size="small"
        />
        <TextField
          id="outlined-number"
          label="Poatal Code"
          type="text"
          InputLabelProps={{
            shrink: true,
          }}
          size="small"
        />
        <TextField
          label="Country Name"
          id="outlined-size-small"

          size="small"
        />
        <TextField
          label="City Name"
          id="outlined-size-small"

          size="small"

        />
        <TextField
          label="City Name"
          id="outlined-size-small"

          size="small"

        />
        <Button variant='contained' size='LARGE' sx={{ m: 2, m: 3, width: '10%' }}>Edit</Button>
        

  



      </div>

      


    </Box>
  );
}

