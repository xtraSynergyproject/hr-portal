import React from 'react';
import { Box, Grid } from '@mui/material';
import InfoIcon from '@mui/icons-material/Info';
import CustomAvatar from 'src/@core/components/mui/avatar'
import ServiceAssigned from '../components/modals/servicemodal/servicemodal1/servicegridmodal/ServiceAssigned'
import ServiceAssigned1 from '../components/modals/servicemodal/servicemodal1/servicegridmodal/ServiceAssigned1'
import ServiceAssigned2 from '../components/modals/servicemodal/servicemodal1/servicegridmodal/ServiceAssigned2'



function WorklistService() {
    return (
        <>
                    <Box sx={{ justifyContent: 'space-between', width: '53rem' }}>
                <Grid container>
                    <Grid item xs={3}>
                        <Box sx={{ display: 'flex',mt:4,fontSize:14 }}>
                            Service
                            <InfoIcon sx={{fontSize:14,m:1}} />
                        </Box>


                    </Grid>
                    <Grid xs={3}>
                        <Box sx={{ display: 'flex',mt:4,fontSize:14 }}>
                            AssignedtoMe
                            <InfoIcon sx={{fontSize:14,m:1}} />
                        </Box>

                    </Grid>
                    <Grid xs={3}>
                        <Box sx={{ display: 'flex',mt:4,fontSize:14}}>
                            RequestedtoMe
                            <InfoIcon sx={{fontSize:14,m:1}}/>
                        </Box>


                    </Grid>
                    <Grid xs={3}>
                        <Box sx={{ display: 'flex',mt:4,fontSize:14}}>
                            SharedwithMe/Team
                            <InfoIcon sx={{fontSize:14,m:1}} />
                        </Box>


                    </Grid>
                </Grid>
                <Grid container>
                    <Grid xs={3}>

                    </Grid>
                    <Grid xs={3}>
                    <Box className='demo-space-x'  sx={{ display:'flex'}}>
                                  <ServiceAssigned/>
                                  <ServiceAssigned1/>
                                  <ServiceAssigned2/>



                                    {/* <CustomAvatar variant='circle'>204</CustomAvatar> */}
                                    {/* <CustomAvatar  variant='circle'>34</CustomAvatar> */}
                                    {/* <CustomAvatar skin='light' variant='circle'> 157</CustomAvatar> */}
                                    </Box>
                                    <Box>
                                    <ServiceAssigned2/>
   
                                    {/* <CustomAvatar sx={{mt:2}} skin='light' variant='circle'>14</CustomAvatar> */}
                                    </Box>

                   
                    </Grid>
                    <Grid xs={3}>
                                    <Box className='demo-space-x'  sx={{ display:'flex'}}>
                                    <ServiceAssigned/>
                                  <ServiceAssigned1/>
                                  <ServiceAssigned2/>

                                    {/* <CustomAvatar variant='circle'>204</CustomAvatar>
                                    <CustomAvatar  variant='circle'>34</CustomAvatar>
                                    <CustomAvatar skin='light' variant='circle'> 157</CustomAvatar> */}
                                    </Box>
                                   

                    </Grid>
                    <Grid xs={3}>
                    
                    </Grid>

                </Grid>
            </Box>
                    </>

    )
}

export default WorklistService
