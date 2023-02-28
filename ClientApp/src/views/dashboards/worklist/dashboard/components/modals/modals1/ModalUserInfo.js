import React from 'react'
import Box from '@mui/material/Box'

// Icons
import PersonIcon from '@mui/icons-material/Person'
import DnsIcon from '@mui/icons-material/Dns'
import FeedIcon from '@mui/icons-material/Feed'
import EventAvailableIcon from '@mui/icons-material/EventAvailable'
import ViewComfyAltIcon from '@mui/icons-material/ViewComfyAlt'
import TextField from '@mui/material/TextField';


function ModalUserInfo() {
    return (
        <div>
            <Box sx={{ display: "flex", gap: "50px", justifyContent: 'space-around' }}>
                <Box sx={{ height: "100px" }}>
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                        <PersonIcon /> Administrator/admin@synergy.com{' '}
                    </Box>
                    <Box>{ }</Box>
                </Box>

                <Box
                    sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}
                >
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                        <EventAvailableIcon /> T-14-20-2023-15{' '}
                    </Box>
                    <Box>{ }</Box>
                </Box>

                <Box
                    sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}
                >
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                        <DnsIcon />Draft{' '}
                    </Box>
                    <Box>{ }</Box>
                </Box>

                <Box
                    sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}
                >
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                        <FeedIcon />Tomorrow{' '}
                    </Box>
                    <Box>{ }</Box>
                </Box>

                <Box
                    sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}
                >
                    <Box sx={{ display: "flex", alignItems: "center" }}>
                        <ViewComfyAltIcon />1{' '}
                    </Box>
                    <Box>{ }</Box>
                </Box>
            </Box>

            {/* <Box sx={{ ml: 4}}>
                Subject
                {/* <Box component="form" sx={{ border: '1px solid grey', borderRadius:1,width: '58rem', height:40 }}> */}
               
               {/* <Box
                    component="form"
                    sx={{
                        '& .MuiTextField-root': { m: 1, width: '57rem',bgcolor: 'background.paper',
                        color: (theme) => theme.palette.getContrastText(theme.palette.background.paper), },
                    }}
                    noValidate
                    autoComplete="off"
                >
                    <div>
                        <TextField
                            
                            id="outlined-size-small"
                            size="small"
                        />
                    </div>

                </Box>

            </Box> */}
        </div>
    )
}

export default ModalUserInfo
