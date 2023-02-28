import React from 'react';
import { Box, Button } from '@mui/material';
import Grid from '@mui/material/Grid';
import CardMedia from '@mui/material/CardMedia';




function Leave() {
    return (
        <div>
            <Box sx={{display:'column',my:'20px',ml:16}}>
                <Box sx={{ flexGrow: 1 }}>
                    <Grid container spacing={10}>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://th.bing.com/th/id/OIP.LciTNhZjw8eFQOLLtG4mawHaGG?pid=ImgDet&rs=1"
                                title="green iguana"

                            />
                            Employee Book
                            {/* <CompassionatelyLeave /> */}

                        </Grid>
                        <Grid item xs={3}>
                            
                        </Grid>
                        <Grid item xs={3}>
                           

                        </Grid>
                        <Grid item xs={3}>
                            

                        </Grid>
                    </Grid>
                </Box>
                             </Box>
        </div>
    )
}

export default Leave
