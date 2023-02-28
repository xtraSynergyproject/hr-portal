import * as React from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import MenuItem from '@mui/material/MenuItem';
import { Typography } from '@mui/material';
import Button from '@mui/material/Button';



const currencies = [
  {
    value: 'USD',
    label: '..Select',
  },
  {
    value: 'EUR',
    label: 'User',
  },
  {
    value: 'BTC',
    label: 'Team',
  },
  {
    value: 'JPY',
    label: 'User Hierarcy',
  },
];

export default function SelectTextFields() {
  return (
    <>
      <Box>

        <Typography sx={{ mt: 5, ml: 4 }}>Assign To</Typography>
        <Box
          component="form"
          sx={{
            '& .MuiTextField-root': { ml: 4, width: '40rem' },
          }}
          noValidate
          autoComplete="off"
        >

          <TextField
            id="outlined-select-currency"
            select
            //   label="Select"
            size='small'
            defaultValue="EUR"
          >
            {currencies.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
              </MenuItem>
            ))}
          </TextField>
        </Box>
      </Box>
      <Box>
        <Typography sx={{ mt: 5, ml: 4 }}>Assign To Team User</Typography>
        <Box
          component="form"
          sx={{
            '& .MuiTextField-root': { ml: 4, width: '57rem' },
          }}
          noValidate
          autoComplete="off"
        >

          <TextField
            id="outlined-select-currency"
            select
            //   label="Select"
            size='small'
            defaultValue="EUR"
          >
            {currencies.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
              </MenuItem>
            ))}
          </TextField>
        </Box>
      </Box>
      <Box>
      <Typography sx={{ mt: 5, ml: 4 }}>From</Typography>

      <Box component="form" sx={{
        ml: 4,
        width: '57rem',
        height: '40vh',
        bgcolor: 'background.paper',
        color: (theme) => theme.palette.getContrastText(theme.palette.background.paper),
      }}>
        Profile picture
      </Box> 
      </Box>
      <Box sx={{ display: 'flex' }}>
        <Box>
          <Typography sx={{ mt: 5, ml: 4 }}>Priority</Typography>
          <Box
            component="form"
            sx={{
              '& .MuiTextField-root': { ml: 4, width: '25rem' },
            }}
            noValidate
            autoComplete="off"
          >

            <TextField
              id="outlined-select-currency"
              select
              //   label="Select"
              size='small'
              defaultValue="EUR"
            >
              {currencies.map((option) => (
                <MenuItem key={option.value} value={option.value}>
                  {option.label}
                </MenuItem>
              ))}
            </TextField>
          </Box>
        </Box>


        <Box sx={{ ml: 4 }}>

          <Typography sx={{ mt: 5, ml: 4 }}>SLA</Typography>

          <Box component="form"
            sx={{
              '& .MuiTextField-root': { ml:4, width: '10ch' },
            }}
            noValidate
            autoComplete="off"
          >
            <div>
              <TextField  id="outlined-size-small" defaultValue="D" size="small"/>
              <TextField id="outlined-size-small" defaultValue="H" size="small"/>
              <TextField  id="outlined-size-small" defaultValue="M" size="small"/>

            </div>
 
          </Box>
        </Box>
      </Box>
      <Box sx={{ ml: 4 }}>

          <Typography sx={{ mt: 5, ml: 4,mb:3}}>Date</Typography>

          <Box component="form"
            sx={{
              '& .MuiTextField-root': { ml:4, width: '25ch' },
            }}
            noValidate
            autoComplete="off"
          >
            <Box sx={{display:'flex'}}>
            <TextField
                      required
                      size='small'
                      sx={{ marginBottom: '20px' }}
                      id='date'
                      label='Plan Start Date*'
                      type='date'
                      defaultValue='YYYY-MM-DD'
                      InputLabelProps={{
                        shrink: true
                      }}
                    />
                    <TextField
                      required
                      
                      size='small'
                      id='date'
                      label='Plan End Date*'
                      type='date'
                      defaultValue='YYYY-MM-DD'
                      InputLabelProps={{
                        shrink: true
                      }}
                    />
                    <TextField
                      required
                      size='small'
                      sx={{ marginBottom: '20px' }}
                      id='date'
                      label='Reminder Date*'
                      type='date'
                      defaultValue='YYYY-MM-DD'
                      InputLabelProps={{
                        shrink: true
                      }}
                    />

            </Box>
 
          </Box>
        </Box>
        <Box sx={{display:'flex',ml:5}}>
        <Button variant='contained' >Save As Draft</Button>
        <Button variant='contained'sx={{ml:3}}>Submit</Button>


        </Box>
    </>
  );

}
