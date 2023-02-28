import React from 'react';
import { Box, Button } from '@mui/material';
import Grid from '@mui/material/Grid';
import CardMedia from '@mui/material/CardMedia';




function Reimbursement() {
    return (
        <div>
            <Box sx={{ display: 'column', my: '20px', ml:16 }}>
                <Box sx={{ flexGrow: 1 }}>
                    <Grid container spacing={10}>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1282806068/photo/email-marketing-concept-person-reading-e-mail-on-smartphone.jpg?s=1024x1024&w=is&k=20&c=I6lDz_y1Na-8UHLYXj1bI2cpABfhkKESZ2dD9wZp0XY="
                                title="green iguana"

                            />
                            {/* <CompassionatelyLeave /> */}
                             Educational Reimbursement

                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1282806068/photo/email-marketing-concept-person-reading-e-mail-on-smartphone.jpg?s=1024x1024&w=is&k=20&c=I6lDz_y1Na-8UHLYXj1bI2cpABfhkKESZ2dD9wZp0XY="
                                title="green iguana"

                            />
                            Other Reimbursement

                            {/* <AdjustmentLeave/> */}
                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1282806068/photo/email-marketing-concept-person-reading-e-mail-on-smartphone.jpg?s=1024x1024&w=is&k=20&c=I6lDz_y1Na-8UHLYXj1bI2cpABfhkKESZ2dD9wZp0XY="
                                title="green iguana"

                            />
                            Medical Reimbursement
                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1282806068/photo/email-marketing-concept-person-reading-e-mail-on-smartphone.jpg?s=1024x1024&w=is&k=20&c=I6lDz_y1Na-8UHLYXj1bI2cpABfhkKESZ2dD9wZp0XY="
                                title="green iguana"

                            />
                           Travel Reimbursement


                        </Grid>
                    </Grid>
                </Box>
                {/* </Grid>
                   </Box> */}


            </Box>
        </div>
    )
}

export default Reimbursement
