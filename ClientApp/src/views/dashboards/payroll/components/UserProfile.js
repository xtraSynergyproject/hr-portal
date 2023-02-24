// ** React Imports
import React, { useState, useEffect } from 'react'

// ** MUI Components
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Grid from '@mui/material/Grid'
import Paper from '@mui/material/Paper'
import Button from '@mui/material/Button'
import { styled } from '@mui/material/styles'
import Typography from '@mui/material/Typography'

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
import MenuIconPage from './MenuIconPage'

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
  // ** State
  const [data, setData] = useState(null)
  useEffect(() => {
    axios
      .get(
        'https://webapidev.aitalkx.com/chr/hrdirect/EmployeeProfile?userId=76e33a87-1e49-4767-9fc4-8107de4f6b2a&portalName=HR&personId=8393d114-f109-45ea-9fcc-ad63f1233264'
      )
      .then(response => {
        setData(response.data)
        localStorage.setItem('userProfile', JSON.stringify(response.data))
        console.log(response.data, 'profile data')
      })
  }, [])
  console.log(data, 'data')

  const designationIcon = data?.designationIcon || 'mdi:briefcase-outline'


  return data !== null ? (
    <Card sx={{ py: '10px', display: 'flex', alignItems: 'center' }} className='user_profile_grid'>
      <Box sx={{ minWidth: '20%', display: 'flex', justifyContent: 'center' }}>
        <ProfilePicture
          src={data.PhotoName}
          alt='profile-picture'
          sx={{ width: '150px', height: '150px', border: '5px solid #f0f0f0' }}
        />
      </Box>
      <Box
        className='user_profile_second_box'
        sx={{ width: '70%', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}
      >
        <Box
          className='user_profile_box box_one'
          //  sx={{  minWidth: '40%' }}
        >
          <Typography sx={{ display: 'flex', mx: 2, mb: 2 }}>
            <PersonOutlinedIcon />
            <Box sx={{ ml: 3 }}>
              Person Full Name:<b>{data.PersonFullName} </b>
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>
            <WorkOutlineOutlinedIcon />
            <Box sx={{ ml: 3 }}>
              Job Name:<b>{data.JobName} </b>
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>
            <FmdGoodOutlinedIcon />
            <Box sx={{ ml: 3 }}>
              Location Name: <b>{data.LocationName} </b>
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>
            <CallOutlinedIcon />
            <Box sx={{ ml: 3 }}>
              Contact: <b>{data.Mobile} </b>
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>
            <MailOutlinedIcon />
            <Box sx={{ ml: 3 }}>
              Email: <b>{data.PersonalEmail} </b>
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', mt: 2, mx: 2 }}>
            <DoneOutlinedIcon />
            <Box sx={{ ml: 3 }}>
              Status: <b>{data.Status} </b>
            </Box>
          </Typography>
        </Box>

        <Box
          className='user_profile_box box_two'
          //  sx={{  minWidth: '40%' }}
        >
          <Typography sx={{ display: 'flex', m: 2 }}>
            <BadgeOutlinedIcon />
            <Box sx={{ ml: 3 }}>
              Department Name:<b> {}</b>
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>
            <DriveFileRenameOutlineOutlinedIcon />
            <Box sx={{ ml: 3 }}>
              Position Name: <b>{data.PositionName} </b>
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>
            <AssignmentTurnedInOutlinedIcon />
            <Box sx={{ ml: 3 }}>
              Assignment Status: <b>{data.AssignmentStatusName} </b>
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>
            <CalendarMonthOutlinedIcon />
            <Box sx={{ ml: 3 }}>
              Date Of Join: <b>{data.DateOfJoin} </b>
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>
            <DownloadDoneOutlinedIcon />
            <Box sx={{ ml: 3 }}>
              Person Status: <b>{}</b>
            </Box>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>
            <GradingOutlinedIcon />
            <Box sx={{ ml: 3 }}>
              Grade Name: <b>{data.GradeName} </b>
            </Box>
          </Typography>
          
        </Box>
      </Box>
    </Card>
  ) : null
}

export default UserProfileHeader
