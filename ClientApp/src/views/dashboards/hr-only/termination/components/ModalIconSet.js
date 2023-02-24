import React from 'react'
import Box from '@mui/material';

// Icons
import PersonIcon from '@mui/icons-material/Person';
import DnsIcon from '@mui/icons-material/Dns';
import FeedIcon from '@mui/icons-material/Feed';
import EventAvailableIcon from '@mui/icons-material/EventAvailable';
import ViewComfyAltIcon from '@mui/icons-material/ViewComfyAlt';

function ModalIconSet() {
    return (
    <div>
      <Box>
        <Box 
        // sx={{display:"flex", flexDirection:"column", alignItems:"center"}}
        >
            <Box><PersonIcon/> Service Owner/Requester </Box>
            <Box>s</Box>
        </Box>
 
        <Box
        //  sx={{display:"flex", flexDirection:"column", alignItems:"center"}}
        >
            <Box><DnsIcon/> Service No. </Box>
            <Box>s</Box>
        </Box>
 
        <Box
        //  sx={{display:"flex", flexDirection:"column", alignItems:"center"}}
        >
            <Box><FeedIcon/> Service Status </Box>
            <Box>s</Box>
        </Box>
 
        <Box 
        // sx={{display:"flex", flexDirection:"column", alignItems:"center"}}
        >
            <Box><EventAvailableIcon/> Due Date </Box>
            <Box>s</Box>
        </Box>
 
        <Box
        //  sx={{display:"flex", flexDirection:"column", alignItems:"center"}}
        >
            <Box><ViewComfyAltIcon/> Service Version </Box>
            <Box>s</Box>
        </Box>
 

      </Box>
      </div>
  )
}

export default ModalIconSet





    
