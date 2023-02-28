import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import IconButton from '@mui/material/IconButton';
import Icon from 'src/@core/components/icon';
import DialogTitle from '@mui/material/DialogTitle';
import Typography from "@mui/material/Typography";
import Divider from '@mui/material/Divider';
import Modal from "@mui/material/Modal";
import GeneralEmailCreateTask from '../modals1/GeneralEmailCreateTask';
import WorkBoardTask from '../modals1/WorkBoardTask';
import Paper from '@mui/material/Paper';
import InputBase from '@mui/material/InputBase';
import MenuIcon from '@mui/icons-material/Menu';
import SearchIcon from '@mui/icons-material/Search';


const modalWrapper = {

    overflow: "auto",

    maxHeight: "70vh",

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

    width: "60rem",

    mb: 3,
    height: '500px',

    borderRadius: "10px",

};



export default function BasicModal() {

    const [open, setOpen] = React.useState(false);

    const handleOpen = () => setOpen(true);

    const handleClose = () => setOpen(false);



    return (

        <div>


                <Button size="small" sx={{ textTransform: 'capitalize' }} onClick={handleOpen}>+Add NOte</Button>





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

                        <Box sx={{ display: 'flex' }}>
                            <Box>
                                <Button variant='contained' size='small' sx={{ width: '15rem', ml: 3,mt:0 }}>
                                    All
                                </Button>

                            </Box>
                            <Box>
                                <Paper
                                    component="form"
                                    sx={{ ml:20, border: '1px solid grey', borderRadius: 10, alignItems: 'center', width: '35rem' }}
                                >
                                    <InputBase
                                        sx={{ ml:12,flex:2}}
                                        placeholder="Search "
                                        inputProps={{ 'aria-label': 'search' }}
                                    />
                                </Paper>

                                <Box sx={{ display: 'flex',justifyContent:'space-evenly' }}>
                                    <Box>
                                        <Box
                                            sx={{
                                                mt: 7,
                                                width:'11rem',
                                                height:'30vh',
                                                borderRadius:1,
                                                backgroundColor: '#307bbb',
                                                '&:hover': {
                                                    backgroundColor: '87CEEB',
                                                    opacity: [0.9, 0.8, 0.7],
                                                },
                                            }}


                                        >
                                            <Typography sx={{ color: '#DCDCDC', textAlign:'center' }}>GeneralEmailTask</Typography>
                                            {/* <Button variant="contained" sx={{mt:15,ml:14}}>Create</Button> */}
                                            <GeneralEmailCreateTask />
                                        </Box>
                                        <Typography sx={{textAlign:'center',mt:2 }}>GeneralEmailTask</Typography>

                                    </Box>
                                    <Box>
                                        <Box
                                            sx={{
                                                mt: 7,
                                                width:'11rem',
                                                height: '30vh',
                                                borderRadius:1,
                
                                                backgroundColor: '#307bbb',
                                                '&:hover': {
                                                    backgroundColor: '#87CEEB',
                                                    opacity: [0.9, 0.8, 0.7],
                                                },
                                            }}
                                        >
                                            <Typography sx={{ color: '#DCDCDC', textAlign:'center' }}>WorkBoardTask</Typography>
                                            {/* <Button variant="contained" sx={{mt:15,ml:14}}>Create</Button> */}
                                            <WorkBoardTask />
                                        </Box>
                                        <Typography sx={{ textAlign:'center',mt:2 }}>WorkBoardTask</Typography>

                                    </Box>

                                </Box>
                            </Box>

                        </Box>




                    </Box>

                </Box>

            </Modal>

        </div>

    );

}

