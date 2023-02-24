import React from 'react'
import TextField from '@mui/material/TextField'
import Grid from '@mui/material/Grid'
import { styled } from '@mui/material/styles'
import Box from '@mui/material/Box'
import Paper from '@mui/material/Paper'
import Select1 from './Select1'
import Select2 from './Select2'

// import Item from "@mui/material/Item"

const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: 'center',
  color: theme.palette.text.secondary
}))

const handleChange = event => {
  const {
    target: { value }
  } = event
}

// grid sizing

function PersonalInfoForm() {
  return (
    <div>
      <Box >
        <Grid container spacing={2}>
          <Grid item xs={12}>
            <Box fullWidth sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
              <Select1 />
              <br />
              <TextField
                sx={{ marginBottom: '35px', width: '400px' }}
                id='date'
                label='Start date'
                type='date'
                defaultValue='YYYY-MM-DD'
                InputLabelProps={{
                  shrink: true
                }}
              />
              <TextField
                sx={{ marginBottom: '25px', width: '400px' }}
                id='date'
                label='End date'
                type='date'
                defaultValue='YYYY-MM-DD'
                InputLabelProps={{
                  shrink: true
                }}
              />

              <Select2 />
            </Box>
          </Grid>
        </Grid>
      </Box>
    </div>
  )
}

export default PersonalInfoForm
