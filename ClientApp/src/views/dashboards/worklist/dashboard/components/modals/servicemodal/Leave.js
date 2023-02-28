import React from 'react';
import { Box, Button } from '@mui/material';
import Grid from '@mui/material/Grid';
import CardMedia from '@mui/material/CardMedia';
import UnAbsent from './servicemodal1/UnAbsent'
import Unpaid from './servicemodal1/UnPaid'
import SickLeave from './servicemodal1/SickLeave';




function Leave() {
    return (
        <div>
            <Box sx={{display:'column',my:'30px',ml:16}}>
                <Box sx={{ flexGrow: 1 }}>
                    <Grid container spacing={12}>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://th.bing.com/th/id/OIP.LciTNhZjw8eFQOLLtG4mawHaGG?pid=ImgDet&rs=1"
                                title="green iguana"

                            >
                        </CardMedia>
                        <UnAbsent/>
                           
                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://th.bing.com/th/id/OIP.BvzRC46VL0qmmwlbIA0WtAHaE7?w=254&h=180&c=7&r=0&o=5&pid=1.7"
                                title="green iguana"

                            />
                           < SickLeave/>
                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1169477405/photo/confetti-throwing-on-happy-newlywed-couple.jpg?s=1024x1024&w=is&k=20&c=7KgbY7QX9_9gdh4BG-PzzkP16K4R75CS6eFdS4Ympvs="
                                title="green iguana"

                            />
                            Marriage Leave

                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1314404524/photo/father-consoling-his-baby-at-home.jpg?s=1024x1024&w=is&k=20&c=yWFjnGzwLf3-fLTnYE6Qx3oQnl0fmWdTlDRJzKYuc94="
                                title="green iguana"

                            />
                            Paternity Leave


                        </Grid>
                    </Grid>
                </Box>
                 <Box sx={{ flexGrow: 1,my:'50px'}}>
                    <Grid container spacing={10}>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1359696179/photo/asian-chinese-mother-bonding-time-with-her-baby-boy-toddler-at-home.jpg?s=1024x1024&w=is&k=20&c=dL6oiRbhzTX9BWl1meCeFVOEkMjGVzzakiPbEfzYUL8="
                                title="green iguana"

                            />
                             {/* <CompassionatelyLeave />  */}
                            <Button> Maternity Leave</Button>

                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 2, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1450789053/photo/businesswoman-going-downstairs-and-waving-to-colleague-at-creative-office.jpg?s=1024x1024&w=is&k=20&c=prpPcCjhpgLOBvhzCIcbu2mkdNAT8pywn-BMuL0267E="
                                title="green iguana"

                            />
                            Examination Leave
                             {/* <AdjustmentLeave/>  */}
                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 2, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1065781792/photo/woman-entering-residential-building.jpg?s=1024x1024&w=is&k=20&c=MO34yqhXOfaHy_Vt4Penxhyi-thCKkjmwBFqAi8b_lc="
                                title="green iguana"

                            />
                           Leave Cancel

                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 2, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1332176299/photo/business-woman-arriving-to-the-office-and-greeting-a-coworker-at-her-desk.jpg?s=1024x1024&w=is&k=20&c=D0owIsP-ptHJMsTnHt1_4lwF8cMl_zMBcZzfZpoI6ow="
                                title="green iguana"

                            />
                            Planned Unpaid Leave


                        </Grid>
                    </Grid>
                </Box> 
                <Box sx={{ flexGrow: 1,my:'50px'}}>
                    <Grid container spacing={3}>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/962224288/photo/confident-young-businesswoman-talking-on-mobile.jpg?s=1024x1024&w=is&k=20&c=1X5WseA8T3pjxYgtU_QJBjLI7bEAIC2KzMk-_otWKJE="
                                title="green iguana"

                            />
                             {/* <CompassionatelyLeave />  */}
                            Leave Handover

                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 2, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1039473780/photo/young-african-man-leaving-after-working-on-laptop.jpg?s=1024x1024&w=is&k=20&c=O0gNWwGGNe-Eh2om_5npokZ3kzpZAy8prDy-hdnQxR4="
                                title="green iguana"

                            />
                            Undertime Leave
                             
                             {/* <AdjustmentLeave/>  */}
                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 2, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1217114478/photo/executive-hands-leaving-office-closing-laptop.jpg?s=1024x1024&w=is&k=20&c=Hds0QKs3TdyJF8BIBB9zNaeyMh3DOkeYQq3yiaDGpFY="
                                title="green iguana"

                            />
                            Annual Leave 

                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 2, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1154589936/photo/female-worker-hand-giving-resignation-latter-to-boss-quitting-a-job.jpg?s=1024x1024&w=is&k=20&c=E5c6_8ImiGBMN1Hnj1av9B_2rS_I6ptZmqFLT1mcUf8="
                                title="green iguana"

                            />
                             Sample Leave

                        </Grid>
                    </Grid>
                </Box> 
                <Box sx={{ flexGrow: 1,my:'50px'}}>
                    <Grid container spacing={3}>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1278596978/photo/woman-bids-goodbye-to-her-colleagues.jpg?s=1024x1024&w=is&k=20&c=VDzf0-IQ7ClqPkCB2tvg9CbMAqY0wKLlVZRap33eyys="
                                title="green iguana"

                            />
                             {/* <CompassionatelyLeave />  */}
                            Compassionately Leave 

                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 2, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1346929970/photo/business-men-in-a-meeting-closing-a-deal-with-a-handshake.jpg?s=1024x1024&w=is&k=20&c=vAwxwH-Z4tfcSICfJ2EayaZ3PvlLBkAVzB0jrlmjQas="
                                title="green iguana"

                            />
                            Leave Adjustment
                             
                             {/* <AdjustmentLeave/>  */}
                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 2, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/994101788/photo/excited-happy-employee-looking-at-wristwatch-satisfied-with-meeting-deadline.jpg?s=1024x1024&w=is&k=20&c=_rAyvQmiSD52KjS6UTQ006ZaQDN3PMmMe3ZYCWJqVIY="
                                title="green iguana"

                            />
                            Annual Leave Encashment

                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 2, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/532264092/photo/man-in-the-city.jpg?s=1024x1024&w=is&k=20&c=Sfyv4RAMZQ9_xqrm3qJdGDkNXiqRADwduGjLfVh40OE="
                                title="green iguana"

                            />
                             <Unpaid/>

                        </Grid>
                    </Grid>
                </Box> 
                <Box sx={{ flexGrow: 1,my:'50px'}}>
                    <Grid container spacing={3}>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1319412251/photo/unemployed-hold-cardboard-box-and-laptop-bag-dossier-and-drawing-tube-in-box-quitting-a-job.jpg?s=1024x1024&w=is&k=20&c=j1PoZeGmMGEASgjmvdV3iWKB96LBCn2AURHlO2251QM="
                                title="green iguana"

                            />
                             {/* <CompassionatelyLeave />  */}
                             Leave Accrual

                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 2, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1068637142/photo/leave-on-paper-note-stick-on-the-calendar-of-december-for-year-end-holidays-concept.jpg?s=1024x1024&w=is&k=20&c=NcFGrof7QSVWXXTVs-QkrluCBslNujChjm3Kg-98AXw="
                                title="green iguana"

                            />
                             PLS Leave
                             
                             {/* <AdjustmentLeave/>  */}
                        </Grid>

                   </Grid>
                   </Box>


            </Box>
        </div>
    )
}

export default Leave
