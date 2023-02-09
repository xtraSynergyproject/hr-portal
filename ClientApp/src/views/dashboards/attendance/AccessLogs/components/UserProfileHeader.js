// ** React Imports
import { useState, useEffect } from 'react'

// ** MUI Components
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Button from '@mui/material/Button'
import { styled } from '@mui/material/styles'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import { Grid } from '@mui/material'
import Selectpart from './Selectpart'
import CalenderPage from './CalenderPage'
import Personldetails from './Personldetails'
// ** Third Party Imports
import axios from 'axios'

function DashBoard(PersonFullName, Status, NationalityName, DateOfJoin, Title, DateOfBirth) {
  return { PersonFullName, Status, NationalityName, DateOfJoin, Title, DateOfBirth }
}

const ProfilePicture = styled('img')(({ theme }) => ({
  width: 160,
  height: 180,
  margin: 15,
  borderRadius: theme.shape.borderRadius,
  border: `5px solid ${theme.palette.common.white}`,
  [theme.breakpoints.down('md')]: {
    marginBottom: theme.spacing(4)
  }

}))

const UserProfileHeader = () => {

  const [data, setData] = useState([])
  useEffect(() => {
    axios.get('https://webapidev.aitalkx.com/chr/hrdirect/EmployeeProfile?userId=76e33a87-1e40-4767-9fc4-8107de4f6b2a&portalName=HR&personId=8393d114-f109-45ea-9fcc-ad63f1233264').then(response => {
      setData(response.data)
      console.log(response.data, 'PersonFullName');

    })

  }, [])
  const designationIcon = data?.designationIcon || 'mdi:briefcase-outline'

  return (
    <>
      <Card>

        <CardContent
          sx={{
            pt: 0,
            mt: -9,
            display: 'flex',
            alignItems: 'flex-end',
            flexWrap: { xs: 'wrap', md: 'nowrap' },
            justifyContent: { xs: 'center', md: 'flex-start' }
          }}
        >
          <Box sx={{ mb: [6, 0], display: 'flex', flexDirection: 'column', alignItems: ['center', 'flex-start'] }}>
            <Typography variant='h5' sx={{ mb: 3, margin: '10px' }}>
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

              </Box>
              <Box sx={{ mr: 5, display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>

                <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>{data.location}</Typography>
              </Box>
              <Box sx={{ display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
              </Box>
            </Box>
          </Box>




        </CardContent>
        <Grid container spacing={4}>

          <Grid item xs={3}>
            <ProfilePicture src="https://th.bing.com/th/id/OIP.SZcxiMTozvt_2F-s-3vyCQAAAA?pid=ImgDet&w=185&h=246&c=7&dpr=1.3" alt='profile-picture' />
            <Box
              sx={{
                width: '100%',
                display: 'flex',
                ml: { xs: 0, md: 6 },
                alignItems: 'flex-end',
                flexWrap: ['wrap', 'nowrap'],
                justifyContent: ['center', 'space-between']
              }}
            ></Box>
            <Typography variant='h5' sx={{ mb: 3, marginLeft: '20px' }}>
              <b>{data.PersonFullName}</b>
            </Typography>
          </Grid>
          <Grid item xs={4}>

            <p>Title:<b>{data.Title}</b></p>
            <p>DateOfBirth: <b>       {data.DateOfBirth}</b></p>
            <p>NationalityName:<b>{data.NationalityName}</b></p>

            <p>User Status: <b>{data.Status}</b></p>

          </Grid>

          <Grid item xs={4}>
            <p>Department Name:<b>IT</b></p>

            <p>Date of Join:28:<b> {data.DateOfJoin}</b></p>
            <p>Person Status:<b>{data.Status}</b></p>
            <p>Grade Name:<b>A</b></p>

          </Grid>
        </Grid>
      </Card>

      <h1>Access Log</h1>
      <Grid sx={{ display: "flex", alignItems: 'center', gap: '70px' }}>
        <p>Employee</p>
        <Selectpart />
        <CalenderPage />
        <Button variant="contained" size="large">Submit</Button>
      </Grid>
      <persondetails />
      <Box sx={{ display: "flex", gap: "20px", marginTop: '30px' }}>
        <Personldetails />
      </Box>
    </>

  )
}

export default UserProfileHeader