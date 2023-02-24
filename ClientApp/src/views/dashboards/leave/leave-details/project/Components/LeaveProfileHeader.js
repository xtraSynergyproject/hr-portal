
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
import LeaveRequest from './LeaveRequest'
import LeaveBalance from '../Components/LeaveBalance'

// ** Third Party Imports
import axios from 'axios'

// ** Icon Imports
import Icon from 'src/@core/components/icon'
import PersonOutlinedIcon from '@mui/icons-material/PersonOutlined'
import DoneOutlinedIcon from '@mui/icons-material/DoneOutlined'
import CallOutlinedIcon from '@mui/icons-material/CallOutlined'
import MailOutlinedIcon from '@mui/icons-material/MailOutlined'
import WorkOutlineOutlinedIcon from '@mui/icons-material/WorkOutlineOutlined'
import FmdGoodOutlinedIcon from '@mui/icons-material/FmdGoodOutlined'
import AssignmentTurnedInOutlinedIcon from '@mui/icons-material/AssignmentTurnedInOutlined'
import BadgeOutlinedIcon from '@mui/icons-material/BadgeOutlined'
import DriveFileRenameOutlineOutlinedIcon from '@mui/icons-material/DriveFileRenameOutlineOutlined'
import DownloadDoneOutlinedIcon from '@mui/icons-material/DownloadDoneOutlined'
import CalendarMonthOutlinedIcon from '@mui/icons-material/CalendarMonthOutlined'
import GradingOutlinedIcon from '@mui/icons-material/GradingOutlined'






function DashBoard(PersonFullName, Status, NationalityName, DateOfJoin, Title, DateOfBirth, JobName, LocationName, AssignmentStatusName, PositionName) {
  return { PersonFullName, Status, NationalityName, DateOfJoin, Title, DateOfBirth, JobName, LocationName, AssignmentStatusName, PositionName }
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



  const [data, setData] = useState([])
  useEffect(() => {
    axios.get('https://webapidev.aitalkx.com/chr/hrdirect/EmployeeProfile?userId=60da8f8f195197515042a1f2&portalName=HR&personId=0a11a928-4f66-41b4-aa44-150d1470ef7e').then(response => {
      setData(response.data)
      console.log(response.data, 'PersonFullName');

    })

  }, [])
  const designationIcon = data?.designationIcon || 'mdi:briefcase-outline'


  return (
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
          <ProfilePicture src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e' alt='profile-picture' />
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
                  <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>{data.designation} Leave Balance:</Typography>
                  <Typography></Typography>
                </Box>
                <Box sx={{ mr: 5, display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
                  {/* <Icon icon='mdi:map-marker-outline' /> */}
                  <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>{data.location}</Typography>
                </Box>

                <Box sx={{ display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
                  <Icon icon='mdi:calendar-blank' />
                  <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>
                    {/* Joined {data.joiningDate} */}
                    <LeaveBalance />
                  </Typography>

                </Box>


              </Box>
            </Box>

            <LeaveRequest />

          </Box>
        </CardContent>

        <Grid container spacing={4} sx={{ margin: '5px' }}>


          <Grid item xs={7}>
            <Typography sx={{ display: 'flex', m: 2, fontSize: 18 }}>

              <Box sx={{ ml: 3 }}>

                PositionName:
              </Box>
              <Box sx={{ ml: 15 }}>
                <b>{data.PositionName} </b>
              </Box>
            </Typography>

            <Typography sx={{ display: 'flex', m: 2, fontSize: 18 }}>



              <Box sx={{ ml: 3 }}>
                Date Of Birth:
              </Box>
              <Box sx={{ ml: 16 }}>

                <b>{data.DateOfBirth}</b>
              </Box>
            </Typography>

            <Typography sx={{ display: 'flex', m: 2, fontSize: 18 }}>

              <Box sx={{ ml: 3 }}>
                Nationality Name:
              </Box>
              <Box sx={{ ml: 7 }}>
                <b>{data.NationalityName}</b>
              </Box>
            </Typography>

            <Typography sx={{ display: 'flex', m: 2, fontSize: 18 }}>

              <Box sx={{ ml: 3 }}>
                Job Name:
              </Box>
              <Box sx={{ ml: 22 }}>
                <b>{data.JobName}</b>
              </Box>
            </Typography>


            <Typography sx={{ display: 'flex', m: 2, fontSize: 18 }}>

              <Box sx={{ ml: 3 }}>
                User Status:
              </Box>
              <Box sx={{ ml: 19 }}>
                <b>{data.Status}</b>
              </Box>
            </Typography>

          </Grid>

          <Grid item xs={5}>

            <Typography sx={{ display: 'flex', m: 2, fontSize: 18 }}>

              <Box sx={{ ml: 3 }}>
                Department Name:
              </Box>
              <Box sx={{ ml: 5 }}>
                <b>{data.DepartmentName}</b>
              </Box>
            </Typography>

            <Typography sx={{ display: 'flex', m: 2, fontSize: 18 }}>

              <Box sx={{ ml: 3 }}>
                Date of Join:
              </Box>
              <Box sx={{ ml: 15 }}>
                <b>{data.DateOfJoin}</b>
              </Box>
            </Typography>


            <Typography sx={{ display: 'flex', m: 2, fontSize: 18 }}>

              <Box sx={{ ml: 3 }}>
                Person Status:
              </Box>
              <Box sx={{ ml: 12 }}>
                <b>{data.Status}</b>
              </Box>
            </Typography>

            <Typography sx={{ display: 'flex', m: 2, fontSize: 18 }}>

              <Box sx={{ ml: 3 }}>
                Location Name:
              </Box>
              <Box sx={{ ml: 10 }}>
                <b>{data.LocationName}</b>
              </Box>
            </Typography>

            <Typography sx={{ display: 'flex', m: 2, fontSize: 18 }}>

              <Box sx={{ ml: 3 }}>
                Assignment Status:
              </Box>
              <Box sx={{ ml: 3 }}>
                <b>{data.AssignmentStatusName}</b>
              </Box>
            </Typography>







          </Grid>
        </Grid>
      </Card>
    </>
  )
}

export default UserProfileHeader





