// ** React Imports
import React, { useState, useEffect } from 'react'

// ** MUI Components
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Grid from '@mui/material/Grid'
import Paper from '@mui/material/Paper'
import Button from '@mui/material/Button'
import { styled } from '@mui/material/styles'
import CardMedia from '@mui/material/CardMedia'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
import Menu from '@mui/material/Menu'
import MenuItem from '@mui/material/MenuItem'

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
import MoreVertIcon from '@mui/icons-material/MoreVert'
import MenuIconPage from './components/MenuIconPage'

const ProfilePicture = styled('img')(({ theme }) => ({
  width: 120,
  height: 120,
  borderRadius: theme.shape.borderRadius,
  border: `5px solid ${theme.palette.common.white}`,
  [theme.breakpoints.down('md')]: {
    marginBottom: theme.spacing(4)
  }
}))

const EmployeeDashboardEmployeeProfile = () => {


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
    <Card sx={{ py: "10px", display: "flex" }} className="user_profile_grid">

      {/* <Box sx={{ minWidth: '20%', display:"flex", justifyContent:"center" }}>
      <ProfilePicture
        src={data.PhotoName}
        alt='profile-picture'
        sx={{ width: '150px', height: '150px', border: '5px solid #f0f0f0' }}
      /> 
    </Box> */}
      <Box className='user_profile_box box_one' sx={{ minWidth: '25%' }}>
        <Typography sx={{ display: 'flex', m: 2}}>
          <PersonOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Grade Name:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          <WorkOutlineOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Date Of Join:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex',m:2}}>
          <PersonOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Department:
          </Box>
        </Typography>

      </Box>

      <Box className='user_profile_box box_two' sx={{ minWidth: '25%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>

          <Box sx={{ ml: 3 }}>
           <p> <b>{data.GradeName} </b></p>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>

          <Box sx={{ ml: 3 }}>
          <p><b>{data.DateOfJoin}</b></p>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2}}>

          <Box sx={{ ml: 3 }}>
          <p><b>{data.DependentId}</b></p>
          </Box>
        </Typography>
      </Box>

      <Box className='user_profile_box box_one' sx={{ minWidth: '25%' }}>

        <Typography sx={{ display: 'flex', m: 2 }}>
          <WorkOutlineOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Position Name:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          <PersonOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Status:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          <WorkOutlineOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            PositionName:
          </Box>
        </Typography>

      </Box>

      <Box className='user_profile_box box_two' sx={{ minWidth: '25%' }}>

        <Typography sx={{ display: 'flex', m: 2 }}>

          <Box sx={{ ml: 3 }}>
          <p> <b>{data.PositionName}</b></p>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>

          <Box sx={{ ml: 3 }}>
          <p><b>{data.Status}</b></p>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>

          <Box sx={{ ml: 3 }}>
          <p> <b>{data.PositionName}</b></p>
          </Box>
        </Typography>
      </Box>
    </Card>
  ) : null
}

export default EmployeeDashboardEmployeeProfile
