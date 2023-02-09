import React, { useState, useEffect } from 'react'
import axios from 'axios'
import Grid from '@mui/material/Grid'
import Box from '@mui/material/Box'
import Paper from '@mui/material/Paper'
import { Typography } from '@mui/material'
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

function createData(
  PersonFullName,
  JobName,
  LocationName,
  Mobile,
  PersonalEmail,
  Status,
  PositionName,
  AssignmentStatusName,
  DateOfJoin,
  GradeName
) {
  return {
    PersonFullName,
    JobName,
    LocationName,
    Mobile,
    PersonalEmail,
    Status,
    PositionName,
    AssignmentStatusName,
    DateOfJoin,
    GradeName
  }
}

function PayrollProfileDetails() {
  const [data, setData] = useState([]) 
  useEffect(() => {
    axios
      .get(
        'https://webapidev.aitalkx.com/chr/hrdirect/EmployeeProfile?userId=76e33a87-1e40-4767-9fc4-8107de4f6b2a&portalName=HR&personId=8393d114-f109-45ea-9fcc-ad63f1233264'
      )
      .then(response => {
        setData(response.data)
        console.log(response.data, 'profile data')
      }).catch(error => console.log(error));
  }, [])
  
  return (
    <div>
      <Grid container spacing={6} sx={{ mt: 1 }}>
        <Grid item xs={12}>
          <Paper elevation={0} sx={{ display: 'flex', justifyContent: 'space-between' }}>
            <Box sx={{width : "100%", display:"flex", justifyContent: "center"}}>
            <Box sx={{ p: 2, minWidth: '49%' }}>
              <Typography sx={{ display: 'flex', m: 2 }}>
                <PersonOutlinedIcon />
                <Box sx={{ ml: 3 }}>
                 Person Full Name: <b> {data.PersonFullName}</b>
                </Box>
              </Typography>

              <Typography sx={{ display: 'flex', m: 2 }}>
                <WorkOutlineOutlinedIcon />
                <Box sx={{ ml: 3 }}>
                  Job Name: <b>{data.JobName}</b>
                </Box>
              </Typography>

              <Typography sx={{ display: 'flex', m: 2 }}>
                <FmdGoodOutlinedIcon />
                <Box sx={{ ml: 3 }}>
                  Location Name:<b> {data.LocationName}</b>
                </Box>
              </Typography>

              <Typography sx={{ display: 'flex', m: 2 }}>
                <CallOutlinedIcon />
                <Box sx={{ ml: 3 }}>
                  Contact: <b>{data.Mobile}</b>
                </Box>
              </Typography>

              <Typography sx={{ display: 'flex', m: 2 }}>
                <MailOutlinedIcon />
                <Box sx={{ ml: 3 }}>
                  Email: <b>{data.PersonalEmail}</b>
                </Box>
              </Typography>

              <Typography sx={{ display: 'flex', m: 2 }}>
                <DoneOutlinedIcon />
                <Box sx={{ ml: 3 }}>
                  Status:<b> {data.Status}</b>
                </Box>
              </Typography>
            </Box>

            <Box sx={{ p: 2, minWidth: '49%' }}>
              <Typography sx={{ display: 'flex', m: 2 }}>
                <BadgeOutlinedIcon />
                <Box sx={{ ml: 3 }}>
                  Department Name:<b> {}</b>
                </Box>
              </Typography>

              <Typography sx={{ display: 'flex', m: 2 }}>
                <DriveFileRenameOutlineOutlinedIcon />
                <Box sx={{ ml: 3 }}>
                  Position Name:<b> {data.PositionName}</b>
                </Box>
              </Typography>

              <Typography sx={{ display: 'flex', m: 2 }}>
                <AssignmentTurnedInOutlinedIcon />
                <Box sx={{ ml: 3 }}>
                  Assignment Status:<b> {data.AssignmentStatusName}</b>
                </Box>
              </Typography>

              <Typography sx={{ display: 'flex', m: 2 }}>
                <CalendarMonthOutlinedIcon />
                <Box sx={{ ml: 3 }}>
                  Date Of Join:<b> {data.DateOfJoin}</b>
                </Box>
              </Typography>

              <Typography sx={{ display: 'flex', m: 2 }}>
                <DownloadDoneOutlinedIcon />
                <Box sx={{ ml: 3 }}>
                  Person Status:<b>{}</b> 
                </Box>
              </Typography>

              <Typography sx={{ display: 'flex', m: 2 }}>
                <GradingOutlinedIcon />
                <Box sx={{ ml: 3 }}>
                 Grade Name:  <b>{data.GradeName}</b>
                </Box>
              </Typography>
            </Box></Box>

          
            
          </Paper>
        </Grid>
      </Grid>
    </div>
  )
}

export default PayrollProfileDetails