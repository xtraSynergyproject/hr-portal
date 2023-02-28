// MUI Imports
import React from 'react';
import { Grid, Box } from '@mui/material';
// Icon Imports
import Icon from 'src/@core/components/icon'
// Custom Components Imports
import CustomAvatar from 'src/@core/components/mui/avatar'
import Avatar from '@mui/material/Avatar'
import ServiceTab from '../components/modals/servicemodal/ServiceTab'
import AssignedModal from '../components/modals/taskmodal/AssignedModal'
import AssignedModal1 from '../components/modals/taskmodal/AssignedModal1'
import AssignedModal2 from '../components/modals/taskmodal/AssignedModal1'



import InfoIcon from '@mui/icons-material/Info';


export default function WorklistTask() {
    return (
        <>
            <Box className='box1' sx={{ display: 'column', width: '53rem' }}>
                {/* <Box className='box1' sx={{display:'flex'}}> */}
                <Grid   container>
                    <Grid item xs={3}>
                        <Box sx={{ display: 'flex', fontSize: 14, mt: 1 }}>
                            TASK
                            <InfoIcon sx={{ fontSize: 14, m: 1 }} />
                        </Box>


                    </Grid>
                    <Grid xs={3}>
                        <Box sx={{ display: 'flex', fontSize: 14, mt: 1 }}>
                            AssignedtoMe
                            <InfoIcon sx={{ fontSize: 14, m: 1 }} />
                        </Box>

                    </Grid>
                    <Grid xs={3}>
                        <Box sx={{ display: 'flex', fontSize: 14, mt: 1 }}>
                            RequestedtoMe
                            <InfoIcon sx={{ fontSize: 14, m: 1 }} />
                        </Box>


                    </Grid>
                    <Grid xs={3}>
                        <Box sx={{ display: 'flex', fontSize: 14, mt: 1 }}>
                            SharedwithMe/Team
                            <InfoIcon sx={{ fontSize: 14, m: 1 }} />
                        </Box>


                    </Grid>
                </Grid>
            {/* </Box> */}
            {/* <Box > */}
                <Grid container>
                    <Grid xs={3}>

                    </Grid>
                    <Grid xs={3}>
                        <Box className='demo-space-x' sx={{ display: 'flex' }}>
                            <AssignedModal/>
                            <AssignedModal1/>
                            <AssignedModal2/>


                            {/* <CustomAvatar variant='square'>
                                60
                            </CustomAvatar> */}
                            {/* <CustomAvatar variant='square'>

                                69
                            </CustomAvatar> */}
                           
                        </Box>

                    </Grid>
                    <Grid xs={3}>
                        <Box className='demo-space-x' sx={{ display: 'flex' }}>
                        <AssignedModal/>
                            <AssignedModal1/>
                            <AssignedModal2/>

                            

                            {/* <CustomAvatar variant='square'>204</CustomAvatar> 
                            <CustomAvatar variant='square'>34</CustomAvatar>
                        <CustomAvatar skin='light' variant='square'> 157</CustomAvatar> */}
                        </Box>
                        <Box sx={{mt:2}}>
                            <AssignedModal2/>

                            {/* <CustomAvatar sx={{ mt: 2 }} skin='light' variant='square'>14</CustomAvatar> */}
                        </Box>

                    </Grid>
                    <Grid xs={3}>
                        <Box className='demo-space-x' sx={{ display: 'flex' }}>
                            <AssignedModal/>
                            <AssignedModal1/>
                            <AssignedModal2/>

                            {/* <CustomAvatar variant='square'>0 </CustomAvatar>
                            <CustomAvatar skin='' variant='square'>0</CustomAvatar>
                            <CustomAvatar skin='light' variant='square'>1</CustomAvatar> */}
                        </Box>

                    </Grid>

                </Grid>
                </Box>
            {/* </Box> */}

        </>
    )
}



