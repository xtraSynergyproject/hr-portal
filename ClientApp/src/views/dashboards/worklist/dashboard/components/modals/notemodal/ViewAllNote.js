import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import IconButton from '@mui/material/IconButton';
import Icon from 'src/@core/components/icon';
import DialogTitle from '@mui/material/DialogTitle';
import Typography from "@mui/material/Typography";
import Divider from '@mui/material/Divider';
import Modal from "@mui/material/Modal";
import { useState } from 'react';
import RequestMainPage from "./RequestMainPage";






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



export default function ViewAllNote() {

    const [open, setOpen] = React.useState(false);

    const handleOpen = () => setOpen(true);

    const handleClose = () => setOpen(false);

    const [value, setValue] = useState('1')

    const handleChange = (event, newValue) => {
        setValue(newValue)

    }


    return (

        <div>

            <Box component='span' sx={{ '& button': { mb: 1, backgroundColor: '#000000' } }}>

                <Button size="medium" variant='contained' onClick={handleOpen}>AllViewNote</Button>

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

                        <DialogTitle>Create Service</DialogTitle>
                        <IconButton
                            aria-label='close'
                            onClick={handleClose}
                            sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.500' }}
                        >
                            <Icon icon='mdi:close' />
                        </IconButton>

                        < Divider />

                      

            <RequestMainPage/>

                

            </Box>

        </Box>

            </Modal >

        </div >

    );

}

