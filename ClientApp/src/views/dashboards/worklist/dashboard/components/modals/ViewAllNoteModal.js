import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import IconButton from '@mui/material/IconButton';
import Icon from 'src/@core/components/icon';
import DialogTitle from '@mui/material/DialogTitle';
import Typography from "@mui/material/Typography";
import Divider from '@mui/material/Divider';
import Modal from "@mui/material/Modal";
import GeneralEmailCreateTask from '../modals/modals1/GeneralEmailCreateTask';
import WorkBoardTask from '../modals/modals1/WorkBoardTask';
import Paper from '@mui/material/Paper';
import InputBase from '@mui/material/InputBase';
import MenuIcon from '@mui/icons-material/Menu';
import SearchIcon from '@mui/icons-material/Search';






const modalWrapper = {

    overflow: "auto",

     maxHeight: "100vh",

    display: "flex",

};



const modalBlock = {

    position: "relative",

    zIndex: 0,

    display: "flex",

    alignItems: "center",

    justifyContent: "center",

    margin: "auto",

}



const modalContentStyle = {

    position: "relative",

    background: "#fff",

    boxShadow: 24,

    mt: 3,

    width: "80rem",

    mb: 3,
    // height: '500px',

    borderRadius: "10px",

};



export default function BasicModal() {

    const [open, setOpen] = React.useState(false);

    const handleOpen = () => setOpen(true);

    const handleClose = () => setOpen(false);



    return (

        <div>

            <Box component='span' sx={{ '& button': { mb: 1, backgroundColor: '#000000' } }}>

                <Button size="medium" variant='contained' onClick={handleOpen}>+ViewAllNote</Button>

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

                        <DialogTitle>Create Task</DialogTitle>
                        <IconButton
                            aria-label='close'
                            onClick={handleClose}
                            sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.500' }}
                        >
                            <Icon icon='mdi:close' />
                        </IconButton>

                        < Divider />

                        <Box sx={{ display: 'flex', }}>
                            <Box>
                                <Button variant='contained' size='small' sx={{ width: '8rem', ml: 3 }}>
                                    All
                                </Button>

                            </Box>
                            <Box>
                                {/* <Box component="form" sx={{ p: 2, border: '1px solid grey', width: '40rem', ml: 10, borderRadius: 10 }}>
                                        Search
                                    </Box> */}
                                <Paper
                                    component="form"
                                    sx={{ ml: 10, border: '1px solid grey', display: 'flex', borderRadius: 10, alignItems: 'center', width: '40rem' }}
                                >
                                    <IconButton sx={{ p: '10px' }} aria-label="menu">
                                        <MenuIcon />
                                    </IconButton>
                                    <InputBase
                                        sx={{ ml: 1, flex: 2 }}
                                        placeholder="Search "
                                        inputProps={{ 'aria-label': 'search' }}
                                    />
                                    <IconButton type="button" sx={{ p: '10px' }} aria-label="search">
                                        <SearchIcon />
                                    </IconButton>
                                    <Divider sx={{ height: 28, m: 0.5 }} orientation="vertical" />
                                    <IconButton color="primary" sx={{ p: '10px' }} aria-label="directions">
                                    </IconButton>
                                </Paper>

                            </Box>

                        </Box>




                    </Box>

                </Box>

            </Modal>

        </div>

    );

}

