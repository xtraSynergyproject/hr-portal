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
import Viewimg from "./viewimg";
import PaginationButton from "./PaginationButton";
import SideBarLeft from "./SideBarLeft";
import SearchBar from "./SearchBar";
import Paper from '@mui/material/Paper';









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



export default function MainViewModal() {

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

                <Button size="small" variant='contained' sx={{ textTransform: 'capitalize' }} onClick={handleOpen}>+ViewAllNotes</Button>


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

                        <DialogTitle>Note</DialogTitle> 
                        <IconButton
                            aria-label='close'
                            onClick={handleClose}
                            sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.500' }}
                        >
                            <Icon icon='mdi:close' />
                        </IconButton>

                        < Divider />
                        <Box sx={{ display: 'column' }}>
                            <Box>
                                <Viewimg />
                                <Box sx={{ display: 'flex' }}>
                                    <Box sx={{ borderRight: '3px  solid cyan' }}>
                                        <SideBarLeft />

                                    </Box>
                                    <Box>
                                        <Box
                                            sx={{
                                                // display: 'column',
                                                // flexWrap: 'wrap',
                                                '& > :not(style)': {
                                                    ml: 7,
                                                    mr: 20,
                                                    width: '51rem',
                                                    height: '29vh',
                                                },
                                            }}
                                        >
                                            <Paper elevation={1}>
                                                <SearchBar />
                                                <Divider />
                                                <Box

                                                    sx={{
                                                        mt:6,
                                                        ml:3,
                                                        width: '48rem',
                                                        height: '9vh',
                                                        backgroundColor: '#DCDCDC',
                                                        border: '1px solid grey',
                                                    }}

                                                >

                                                <PaginationButton />


                                        </Box>

                                    </Paper>
                                </Box>






                            </Box>
                        </Box>

                    </Box>

                </Box>


            </Box>

        </Box>

            </Modal >

        </div >

    );

}

