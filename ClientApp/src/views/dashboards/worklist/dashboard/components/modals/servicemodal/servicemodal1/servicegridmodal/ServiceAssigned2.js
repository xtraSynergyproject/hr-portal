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
// import ServiceTab from './servicemodal/ServiceTab';
import TextField from '@mui/material/TextField';
import CustomAvatar from 'src/@core/components/mui/avatar'
import Assignedtome from '../../../taskmodal/Assignedtome'
import Servicegrid from './Serivicegrid'
import Viewimg from  '../../../note/viewnotemodal/Viewimg'
// import TaskSearchBar from '../../../taskmodal/TaskSearchBar'








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


        <Box size='small' sx={{cursor:'pointer'}}onClick={handleOpen}>
        <CustomAvatar skin='light' variant='circle'> 157</CustomAvatar>

        
        </Box>




      <Modal

        open={open}

        sx={modalWrapper}

        onClose={handleClose}

        aria-labelledby="modal-modal-title"

        aria-describedby="modal-modal-description"

      >

        <Box sx={modalBlock}>

          <Box sx={modalContentStyle}>

            <DialogTitle>Service Home</DialogTitle>
            <IconButton
              aria-label='close'
              onClick={handleClose}
              sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.500' }}
            >
              <Icon icon='mdi:close' />
            </IconButton>

            < Divider />
            <Box sx={{mt:4}}>
            <Viewimg/>
            </Box>

            <Box sx={{display:'flex'}}>
            <Assignedtome/>
            <Box sx={{ml:4}}>
            <Servicegrid/>
            {/* <TaskSearchBar/> */}


            </Box>


            </Box>
            
          </Box>


        </Box>

      </Modal >

    </div >

  );

}

