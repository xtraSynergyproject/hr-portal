
// ** React Imports
import { useState, useEffect } from 'react'

// ** MUI Components
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Button from '@mui/material/Button'
import { styled } from '@mui/material/styles'
import CardMedia from '@mui/material/CardMedia'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import { Grid } from '@mui/material'
import LoadLeave from './LoadLeave'

// ** Third Party Imports
import axios from 'axios'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

function DashBoard( PersonFullName,Status,NationalityName,DateOfJoin,Title,DateOfBirth) {
  return { PersonFullName,Status,NationalityName,DateOfJoin,Title,DateOfBirth}
}








const ProfilePicture = styled('img')(({ theme }) => ({
  width: 120,
  height: 120,
  borderRadius: theme.shape.borderRadius,
  border: `5px solid ${theme.palette.common.white}`,
  [theme.breakpoints.down('md')]: {
    marginBottom: theme.spacing(4)
  }
}))

const UserProfileHeader = () => {
  // // ** State
  // const [data, setData] = useState(null)
  // useEffect(() => {
  //   axios.get('/pages/profile-header').then(response => {
  //     setData(response.data)
  //   })
  // }, [])
  

  const [data, setData] = useState([])
  useEffect(() => {
    axios.get('https://webapidev.aitalkx.com/chr/hrdirect/EmployeeProfile?userId=76e33a87-1e40-4767-9fc4-8107de4f6b2a&portalName=HR&personId=8393d114-f109-45ea-9fcc-ad63f1233264').then(response => {
      setData(response.data)
      console.log(response.data, 'PersonFullName');
      
    })
  
  }, [])
  const designationIcon = data?.designationIcon || 'mdi:briefcase-outline'


  return  (
    <>
    <Card>
      <CardMedia
        component='img'
        alt='profile-header'
        image="https://www.influencive.com/wp-content/uploads/2021/03/it-firms-1536x1022.jpg"
        sx={{
          height: { xs: 150, md: 250 }
        }}


  
      />
      <CardContent
        sx={{
          pt: 0,
          mt: -8,
          display: 'flex',
          alignItems: 'flex-end',
          flexWrap: { xs: 'wrap', md: 'nowrap' },
          justifyContent: { xs: 'center', md: 'flex-start' }
        }}
      >
        <ProfilePicture src="https://bionordika.no/application/files/cache/thumbnails/649d1af142975fe6f429c75165136708.png" alt='profile-picture' />
        <Box
          sx={{
            width: '100%',
            display: 'flex',
            ml: { xs: 0, md: 6 },
            alignItems: 'flex-end',
            flexWrap: ['wrap', 'nowrap'],
            justifyContent: ['center', 'space-between']
          }}
        >
          <Box sx={{ mb: [6, 0], display: 'flex', flexDirection: 'column', alignItems: ['center', 'flex-start'] }}>
            <Typography variant='h5' sx={{ mb: 4 }}>
              <b>{data.PersonFullName}</b>
            </Typography>
            <Box
              sx={{
                display: 'flex',
                flexWrap: 'wrap',
                justifyContent: ['center', 'flex-start']
              }}
            >
              <Box sx={{ mr: 5, display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
                <Icon icon={designationIcon} />
                <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>{data.designation} Leave Balance</Typography>
              </Box>
              <Box sx={{ mr: 5, display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
                {/* <Icon icon='mdi:map-marker-outline' /> */}
                <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>{data.location}</Typography>
              </Box>
              <Box sx={{ display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
                <Icon icon='mdi:calendar-blank' />
                <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>
                  {/* Joined {data.joiningDate} */}
                  Annual Leave Balance Projections
                </Typography>
              </Box>
            </Box>
          </Box>
          
          <Button variant='contained' startIcon={<Icon icon='mdi:account-check-outline' fontSize={20} />}>
            <LoadLeave/>
          </Button>
          
        </Box>
      </CardContent>
      <Grid container spacing={4}>

         <Grid item xs={6}>
              
                <p>Title:<b>{data.Title}</b></p>
               <p>DateOfBirth: <b>       {data.DateOfBirth}</b></p>
              <p>NationalityName:<b>{data.NationalityName}</b></p>
              
              <p>User Status: <b>{data.Status}</b></p>
            
            </Grid>

            <Grid item xs={4}>
                

                <p>Department Name:<b>IT</b></p>
                
                <p>Date of Join:28:<b>                  {data.DateOfJoin}</b></p>
                <p>Person Status:<b>{data.Status}</b></p>
                <p>Grade Name:<b>A</b></p>
            
              </Grid>
              </Grid>

      





    </Card>
    </>
  ) 
}

export default UserProfileHeader

