import * as React from "react";
import { Grid, Box } from "@mui/material";
import Button from "@mui/material/Button";
import IconButton from '@mui/material/IconButton';
import Icon from 'src/@core/components/icon';
import DialogTitle from '@mui/material/DialogTitle';
import Typography from "@mui/material/Typography";
import Divider from '@mui/material/Divider';
import Modal from "@mui/material/Modal";
import { useState } from 'react';
import TextField from '@mui/material/TextField';
import NoteTab from './NoteTab';







const modalWrapper = {
  overflow: 'auto',
  maxHeight: '100vh',
  display: 'flex'
}

const modalBlock = {
  position: 'relative',
  zIndex: 0,
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  margin: 'auto'
}

const modalContentStyle = {
  position: 'relative',
  background: '#fff',
  boxShadow: 24,
  mt: 3,
  width: '75rem',
  mb: 3,
  borderRadius: '10px'
}



export default function BasicModal() {

  const [open, setOpen] = React.useState(false);

  const handleOpen = () => setOpen(true);

  const handleClose = () => setOpen(false);

  const [value, setValue] = useState('1')

  const handleChange = (event, newValue) => {
    setValue(newValue)

  }


  return (

    <div>

      <Box component='span' sx={{ '& button': { mb: 1 } }}>

        <Button size="small"  sx={{textTransform:'capitalize'}} onClick={handleOpen}>+Add Note</Button>

        {/* <Button component='span' variant='contained' sx={{ backgroundColor: '#000000', p: 3, ml: 5 }} onClick={handleOpen}>
                Create Task 

            </Button> */}</Box>



      <Modal

        open={open}

        sx={modalWrapper}

        onClose={handleClose}

        aria-labelledby="modal-modal-title"

        aria-describedby="modal-modal-description"

      >

        <Box sx={modalBlock}>

          <Box sx={modalContentStyle}>

            <DialogTitle>Note Templates</DialogTitle>
            <IconButton
              aria-label='close'
              onClick={handleClose}
              sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.500' }}
            >
              <Icon icon='mdi:close' />
            </IconButton>

            < Divider />
            <Box >
              <Box sx={{my:1}}>
                <Grid container spacing={3}>
                  <Grid item xs={4}>
                    <Button variant='contained' size='small' sx={{width:'11rem',ml:2}}>All</Button>
                  </Grid>
                  <Grid item xs={8}>
                    <TextField
                      required
                      size='small'
                      sx={{ marginBottom: '20px', width: '40rem', borderRadius:20}}
                      id='date'

                      defaultValue='Search'

                    />


                  </Grid>
                </Grid>
              </Box>
              <Box>
                <NoteTab />
              </Box>
            </Box>







          </Box>

        </Box>

      </Modal >

    </div >

  );

}

