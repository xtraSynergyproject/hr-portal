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
import Tab from '@mui/material/Tab'
import TabList from '@mui/lab/TabList'
import TabPanel from '@mui/lab/TabPanel'
import TabContext from '@mui/lab/TabContext'
import Leave from './servicemodal/Leave'
import PersonDocuments from './servicemodal/PersonDocuments';
import Hr from './servicemodal/Hr';
import HRDocument from './servicemodal/HRDocument'
import CoreHr from './servicemodal/CoreHr';
import Reimbursement from "./servicemodal/Reimbursement";

// import ServiceTab from './servicemodal/ServiceTab'






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

            <Box component='span' sx={{ '& button': { mt:3,mr:2, backgroundColor: '#000000' } }}>

                <Button size="small" variant='contained' sx={{textTransform:'capitalize'}} onClick={handleOpen}>+Service Task</Button>

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

                        <DialogTitle>Create Service</DialogTitle>
                        <IconButton
                            aria-label='close'
                            onClick={handleClose}
                            sx={{ top: 10, right: 10, position: 'absolute', color: 'grey.500' }}
                        >
                            <Icon icon='mdi:close' />
                        </IconButton>

                        < Divider />

                        <TabContext value={value}>
            <Box sx={{ display: 'flex' }}>
                <TabList orientation='vertical' onChange={handleChange} aria-label='vertical tabs example' sx={{ml:1}} >
                    <Tab value='0' label='All Template' sx={{textTransform:'capitalize',color:'black',border:'1px solid grey'}} disabled/>
                    <Tab value='1' label='Leave' sx={{textTransform:'capitalize'}} />
                    <Tab value='2' label='Person Documents' sx={{textTransform:'capitalize'}}/>
                    <Tab value='3' label='HR' sx={{textTransform:'capitalize'}} />
                    <Tab value='4' label='HR Documents' sx={{textTransform:'capitalize'}} />
                    <Tab value='5' label='HR Core' sx={{textTransform:'capitalize'}} />
                    <Tab value='6' label='Reimbursement' sx={{textTransform:'capitalize'}} />


                </TabList>
                <TabPanel value='0' sx={{alignContent:'center'}}>
                <Reimbursement />
                <Leave />
                <PersonDocuments />
                <Hr />
                <HRDocument />
                <CoreHr /> 






                </TabPanel> 
                <TabPanel value='1'>
                    <Leave />
                </TabPanel>
                <TabPanel value='2'>
                    <PersonDocuments />
                </TabPanel>
                <TabPanel value='3'>
                <Hr />

                </TabPanel>
                <TabPanel value='4'>
                <HRDocument />

                </TabPanel>
                <TabPanel value='5'>
                <CoreHr />

                </TabPanel>
                <TabPanel value='6'>
                <Reimbursement />

                </TabPanel>
            </Box>
        </TabContext>









            </Box>

        </Box>

            </Modal >

        </div >

    );

}

