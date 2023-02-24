import React from 'react'
import Box from '@mui/material/Box'

// Icons
import PersonIcon from '@mui/icons-material/Person'
import DnsIcon from '@mui/icons-material/Dns'
import FeedIcon from '@mui/icons-material/Feed'
import EventAvailableIcon from '@mui/icons-material/EventAvailable'
import ViewComfyAltIcon from '@mui/icons-material/ViewComfyAlt'

function ModalUserInfo() {
  return (
    <div>
      <Box sx={{display:"flex",gap:"50px"}}>
        <Box sx={{height:"100px"}}>
          <Box sx={{display:"flex", alignItems:"center"}}>
            <PersonIcon /> Service Owner/Requester{' '}
          </Box>
          <Box>{}</Box>
        </Box>

        <Box
         sx={{display:"flex", flexDirection:"column", alignItems:"center"}}
        >
          <Box sx={{display:"flex", alignItems:"center"}}>
            <DnsIcon /> Service No.{' '}
          </Box>
          <Box>{}</Box>
        </Box>

        <Box
         sx={{display:"flex", flexDirection:"column", alignItems:"center"}}
        >
          <Box sx={{display:"flex", alignItems:"center"}}>
            <FeedIcon /> Service Status{' '}
          </Box>
          <Box>{}</Box>
        </Box>

        <Box
        sx={{display:"flex", flexDirection:"column", alignItems:"center"}}
        >
          <Box sx={{display:"flex", alignItems:"center"}}>
            <EventAvailableIcon /> Due Date{' '}
          </Box>
          <Box>{}</Box>
        </Box>

        <Box
         sx={{display:"flex", flexDirection:"column", alignItems:"center"}}
        >
          <Box sx={{display:"flex", alignItems:"center"}}>
            <ViewComfyAltIcon /> Service Version{' '}
          </Box>
          <Box>{}</Box>
        </Box>
      </Box>
    </div>
  )
}

export default ModalUserInfo
