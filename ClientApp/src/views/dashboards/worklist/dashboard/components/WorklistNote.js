import React from 'react';
import { Box, Grid } from '@mui/material';
import InfoIcon from '@mui/icons-material/Info';
import CustomAvatar from 'src/@core/components/mui/avatar';


function WorklistNote() {
  return (
    <>
    <Box sx={{ justifyContent: 'space-between', width: '53rem' }}>
                <Grid container>
                    <Grid item xs={3}>
                        <Box sx={{ display: 'flex', mt:4,fontSize:14}}>
                            Note
                            <InfoIcon sx={{fontSize:14,m:1}}/>
                        </Box>


                    </Grid>
                    <Grid xs={3}>
                        <Box sx={{ display: 'flex',mt:4,fontSize:14 }}>
                            CreatedbyMe
                            <InfoIcon sx={{fontSize:14,m:1}} />
                        </Box>

                    </Grid>
                    <Grid xs={3}>
                        <Box sx={{ display: 'flex',mt:4,fontSize:14 }}>
                            SharedbyMe
                            <InfoIcon sx={{fontSize:14,m:1}} />
                        </Box>


                    </Grid>
                    <Grid xs={3}>
                        <Box sx={{ display: 'flex',mt:4,fontSize:14 }}>
                            SharedwithMe/Team
                            <InfoIcon sx={{fontSize:14,m:1}}/>
                        </Box>


                    </Grid>
                </Grid>
                <Grid container>
                    <Grid xs={3}>

                    </Grid>
                    <Grid xs={3}>
                    <Box className='demo-space-x' sx={{ display: 'flex' }}>
                                   <Box
                                    cursor='pointer'>
                                   <CustomAvatar variant='square'>
                                        60 
                                    </CustomAvatar>

                                   </Box>
                                    
                                     <CustomAvatar  variant='square'> 
                                    
                                         69
                                     </CustomAvatar> 
                                    <CustomAvatar skin='light' variant='square'>
                                        110
                                    </CustomAvatar>
                                    
                                </Box>

                    </Grid>
                    <Grid xs={3}>
                                    <Box className='demo-space-x'  sx={{ display:'flex'}}>
                                    <CustomAvatar variant='square'>204</CustomAvatar>
                                    <CustomAvatar  variant='square'>34</CustomAvatar>
                                    <CustomAvatar skin='light' variant='square'> 157</CustomAvatar>
                                    </Box>
                                    

                    </Grid>
                    <Grid xs={3}>
                    <Box className='demo-space-x' sx={{ display: 'flex' }}>
                                    <CustomAvatar variant='square'>0 </CustomAvatar>
                                    <CustomAvatar  variant='square'>0</CustomAvatar>
                                    <CustomAvatar skin='light' variant='square'>1</CustomAvatar>
                                </Box>

                    </Grid>

                </Grid>
            </Box>
     
    </>
  )
}

export default WorklistNote
