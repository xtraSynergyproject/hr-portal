import React from 'react';
import { Box, Button } from '@mui/material';
import Grid from '@mui/material/Grid';
import CardMedia from '@mui/material/CardMedia';




function Leave() {
    return (
        <div>
            <Box sx={{display:'column',my:'20px',ml:7}}>
                <Box sx={{ flexGrow: 1 }}>
                    <Grid container spacing={10}>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1130833057/photo/close-up-real-estate-agent-with-house-model-hand-putting-signing-contract-have-a-contract-in.jpg?s=1024x1024&w=is&k=20&c=jvR8e5JgVxQ4IipkCwWeiohfAOeZN8_xcuFafhOTGQk="
                                title="green iguana"

                            />
                            {/* <CompassionatelyLeave /> */}
                            Other Document

                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1264230781/photo/concept-of-h1b-visa-for-foreign-workers-showing-wooden-letters-with-us-or-united-states-flag.jpg?s=1024x1024&w=is&k=20&c=3Nv0VXN7WrmrrcguKgJt95dYUlxzpRJ-5XoIwEyqomE="
                                title="green iguana"

                            />
                            Employee Visa
                            {/* <AdjustmentLeave/> */}
                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1363276509/photo/teacher-giving-computer-science-lecture-to-diverse-multiethnic-group-of-female-and-male.jpg?s=1024x1024&w=is&k=20&c=H5Dhh4QacrUmdxW7ncdWsB-1Xh1FOx8ivshDh7Y3sJc="
                                title="green iguana"

                            />
                            Employee Training Courses

                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1043738352/photo/internship-puzzle-concept.jpg?s=1024x1024&w=is&k=20&c=UuA0ivVnO_LgGgPVqHP7wyE2SAECOrRMd-I6zhpVMjw="
                                title="green iguana"

                            />
                            {/* <CompassionatelyLeave /> */}
                            Employee Work Experience

                        </Grid>

                   </Grid>
                </Box> 
                <Box sx={{ flexGrow: 1 }}>
                    <Grid container spacing={10}>
                                                <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/1318450241/photo/name-badge-in-people-hands-personal-security-identification-mockup.jpg?s=1024x1024&w=is&k=20&c=KJTbsgd2rj_Bg-uu2oWRJfKMep2IDP5r0JV-x7y7loY="
                                title="green iguana"

                            />
                            Employee ID
                            {/* <AdjustmentLeave/> */}
                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/959527164/photo/man-holding-passport-and-boarding-pass-at-airline-check-in-counter.jpg?s=1024x1024&w=is&k=20&c=j2EY-rIaeSNyib6SBE3gRHVk6LUfmt_oesqq9aztscY="
                                title="green iguana"

                            />
                            Employee Passport

                        </Grid>
                        <Grid item xs={3}>
                            <CardMedia
                                sx={{ borderRadius: 1, height: 140, width: '10rem' }}
                                image="https://media.istockphoto.com/id/500648082/photo/education-concepts.jpg?s=1024x1024&w=is&k=20&c=msFOJF8xWSlE78Xx736Mes664mi9986w4hnIooPp1DM="
                                title="green iguana"

                            />
                            {/* <CompassionatelyLeave /> */}
                            Employee Education  Qualification

                        </Grid>

                   </Grid>
                </Box> 
            
            </Box>
        </div>
    )
}

export default CoreHr
