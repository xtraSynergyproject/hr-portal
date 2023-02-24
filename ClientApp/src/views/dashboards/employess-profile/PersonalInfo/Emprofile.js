import { useState, useEffect } from 'react'
// ** MUI Components
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import { styled } from '@mui/material/styles'
import CardMedia from '@mui/material/CardMedia'
import Typography from '@mui/material/Typography'
import CardContent from '@mui/material/CardContent'
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

// ** Third Party Imports
import axios from 'axios'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

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
    axios.get('/pages/profile-header').then(response => {
      setData(response.data)
    })
  }, [])


  // ** State
  const [datta, seetData] = useState(null)
  useEffect(() => {
    axios
      .get(
        'https://webapidev.aitalkx.com/chr/hrdirect/EmployeeProfile?userId=60da8f8f195197515042a1f2&portalName=HR&personId=0a11a928-4f66-41b4-aa44-150d1470ef7e'
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
    <Card>
      <CardMedia
        component='img'
        alt='profile-header'
        image="https://images.unsplash.com/photo-1542744173-8e7e53415bb0?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1170&q=80"
        sx={{
          height: { xs: 150, md: 200 }
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
        <ProfilePicture src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQKJQp8ndvEkIa-u1rMgJxVc7BBsR11uSLHGA&usqp=CAU" alt='profile-picture' />
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

            <Box
              sx={{
                display: 'flex',
                flexWrap: 'wrap',
                justifyContent: ['center', 'flex-start']
              }}
            >

            </Box>
          </Box>

        </Box>

      </CardContent>


        <Card sx={{py:"10px",display:"flex"}} className="user_profile_grid">
      <Box sx={{ minWidth: '6%', display:"flex", justifyContent:"center" }}>
       
      </Box>
      
      <Box className='user_profile_box box_one' sx={{  minWidth: '20%' ,mb:2}}>
        <Typography sx={{ display: 'flex', m: 2  }}>
          <PersonOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Person Full Name:
          </Box>
         
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          <WorkOutlineOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Job Name: 
          </Box>
      
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          <FmdGoodOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Location Name: 
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          <CallOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Contact:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          <MailOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Email:
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', mt: 2,mx:2 }}>
          <DoneOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Status:
          </Box>
        </Typography>
      </Box>
      <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 ,fontWeight:1}}>
          <b>{data.PersonFullName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 ,fontWeight:1}}>
          <b>{data.JobName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.LocationName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.Mobile} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.PersonalEmail} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
          <b>{data.GradeName} </b>
          </Box>
        </Typography>
      </Box>
      <Box className='user_profile_box box_two' sx={{  minWidth: '15%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>
          <BadgeOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Department Name:<b> {}</b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          <DriveFileRenameOutlineOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Position Name: 
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          <AssignmentTurnedInOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Assignment Status: 
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          <CalendarMonthOutlinedIcon />
          <Box sx={{ ml: 3 }}>
            Date Of Join: 
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
            Grade Name: 
          </Box>
        </Typography>
      </Box>

      <Box className='user_profile_box box_two' sx={{  minWidth: '20%' }}>
        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.PositionName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
           <b>{data.PositionName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          
          <Box sx={{ ml: 3 }}>
          <b>{data.GradeName} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
            <b>{data.DateOfJoin} </b>
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
            
          </Box>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
        
          <Box sx={{ ml: 3 }}>
            
          </Box>
        </Typography>
      </Box>

    </Card>
    </Card>
  ) : null
}

export default UserProfileHeader

