// // // ** React Imports
// // import { useState, useEffect } from 'react'

// // // ** MUI Components
// // import Box from '@mui/material/Box'
// // import Card from '@mui/material/Card'
// // import Button from '@mui/material/Button'
// // import { styled } from '@mui/material/styles'
// // import CardMedia from '@mui/material/CardMedia'
// // import Typography from '@mui/material/Typography'
// // import CardContent from '@mui/material/CardContent'
// // import { Grid } from '@mui/material'



// // // ** Third Party Imports
// // import axios from 'axios'

// // // ** Icon Imports
// // import Icon from 'src/@core/components/icon'

// // function DashBoard(PersonFullName, Status, NationalityName, DateOfJoin, JobName, DateOfBirth) {
// //   return { PersonFullName, Status, NationalityName, DateOfJoin, JobName, DateOfBirth }
// // }


// // const ProfilePicture = styled('img')(({ theme }) => ({
// //   width: 160,
// //   height: 180,
// //   margin: 15,
// //   borderRadius: theme.shape.borderRadius,
// //   border: `5px solid ${theme.palette.common.white}`,
// //   [theme.breakpoints.down('md')]: {
// //     marginBottom: theme.spacing(4)
// //   }

// // }))

// // const Emprofile = () => {
// //   const [data, setData] = useState([])
// //   useEffect(() => {
// //     axios.get('https://webapidev.aitalkx.com/chr/hrdirect/EmployeeProfile?userId=76e33a87-1e40-4767-9fc4-8107de4f6b2a&portalName=HR&personId=8393d114-f109-45ea-9fcc-ad63f1233264').then(response => {
// //       setData(response.data)
// //       console.log(response.data, 'PersonFullName');

// //     })

// //   }, [])
// //   const designationIcon = data?.designationIcon || 'mdi:briefcase-outline'
// //   return (
// //     <>
// //       <Card>
// //         <CardContent
// //           sx={{
// //             pt: 0,
// //             mt: -9,
// //             display: 'flex',
// //             alignItems: 'flex-end',
// //             flexWrap: { xs: 'wrap', md: 'nowrap' },
// //             justifyContent: { xs: 'center', md: 'flex-start' }
// //           }}
// //         ><Box sx={{ mb: [6, 0], display: 'flex', flexDirxection: 'column', alignItems: ['center', 'flex-start'] }}>
// //             <Typography variant='h5' sx={{ mb: 3, margin: '10px' }}>
// //               <b>{data.PersonFullName}</b>
// //             </Typography>
// //             <Box
// //               sx={{
// //                 display: 'flex',
// //                 flexWrap: 'wrap',
// //                 justifyContent: ['center', 'flex-start']
// //               }}
// //             >
// //             </Box>
// //           </Box>
// //         </CardContent>
// //         <Grid container spacing={4}>
// //           <Grid item xs={3}>
// //             <ProfilePicture src="https://media.istockphoto.com/id/1327034671/photo/business-technology-internet-and-network-concept-human-resources-hr-management-recruitment.jpg?b=1&s=170667a&w=0&k=20&c=pnYBCp2d5qZoR2fnbEIBcJV9n0JeNrVL6K-zTOBWz6I=" alt='profile-picture' sx={{ mb: 10, marginLeft: '10px',   width: '50%', }}/>
// //             <Box
// //               sx={{
// //                 width: '100%',
// //                 display: 'flex',
// //                 ml: { xs: 0, md: 6 },
// //                 alignItems: 'flex-end',
// //                 flexWrap: ['wrap', 'nowrap'],
// //                 justifyContent: ['center', 'space-between']
// //               }}
// //             ></Box>
// //             <Typography variant='h5' sx={{ mb: 3, marginLeft: '20px' }}>
// //               <b>{data.PersonFullName}</b>
// //             </Typography>
// //           </Grid>
// //           <Grid item xs={4}>
// //             <p>Person Full Name :<b>{data.PersonFullName}</b></p>
// //             <p>Job Name:<b>       {data.JobName}</b></p>
// //             <p>Location Name: <b>{data.LocationName}</b></p>
// //             <p>Assignment Status: <b>{data.AssignmentStatusName}</b></p>
// //             <p>User Status: <b>{data.UserStatus}</b></p>
// //           </Grid>
// //           <Grid item xs={4}>
// //             <p>Department Name:<b>IT</b></p>
// //             <p>Position Name:<b>{data.PositionName}</b></p>
// //             <p>Date of Join:28:<b>{data.DateOfJoin}</b></p>
// //             <p>Person Status:<b>{data.Status}</b></p>
// //             <p>Grade Name:<b>A</b></p>

// //           </Grid>
// //         </Grid>
// //       </Card>
// //     </>
// //   )
// // }

// // export default Emprofile