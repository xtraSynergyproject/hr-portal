import React from 'react'
import Box from '@mui/material/Box'
import { Divider, Typography } from '@mui/material'
import Stack from '@mui/material/Stack'
import Avatar from '@mui/material/Avatar'
import ThumbUpIcon from '@mui/icons-material/ThumbUp'
import InsertCommentIcon from '@mui/icons-material/InsertComment'
import Button from '@mui/material/Button'
import IconButton from '@mui/material/IconButton'

// ** Icon Imports
import Icon from 'src/@core/components/icon'

function AdministratorCommentsCard() {
    const handleClick = event => {
        setAnchorEl(event.currentTarget)
      }
    return (
        <div>
            <Box
                sx={{
                    height: '24vh',
                    width: '44rem',
                    // backgroundColor: 'aliceblue',
                    ml: 1,
                    mt: 1
                }}
            >
                {/* image/adminitrator/date */}
                <Box sx={{ display: 'flex', flexDirection: 'row', gap: 2, ml: 3, mt: 5 }}>
                    {/* image box  */}
                    <Box>
                        <Stack direction='row'>
                            <Avatar
                                alt='Remy Sharp'
                                src='https://synergydev.aitalkx.com/Cms/document/getimagemongo/92133c2a-6a6c-4422-989f-c900eae6992e'
                            />
                        </Stack>
                    </Box>
                    <Box sx={{ display: 'flex', flexDirection: 'column' }}>
                        <Typography fontSize='15px'>
                            <b> Administrator To Self</b>
                        </Typography>
                        <Typography fontSize='12px'>August 18 2022 at 1:19 PM</Typography>
                    </Box>
                </Box>
                {/* <Box sx={{ ml: 4, mt: 2 }}>
                    <Typography>sd</Typography>
                </Box>
                <br /> */}
                <Box sx={{ ml: 4, mt: 2, display: 'flex', flexDirection: 'row', wordSpacing: '1px' }}>
                    <Typography sx={{m:2}}>0 Likes 1 Comments</Typography>
                </Box>
                <Divider />
                <Box sx={{ ml: 4, display: 'flex', flexDirection: 'row', wordSpacing: '15px' }}>
                    <Box sx={{display: 'flex'}}>
                        
                        <Box sx={{ '& svg': { m: 2 },display:"flex", alignItems:"center" }}>
                            <ThumbUpIcon />
                          <Typography>Update</Typography>  
                        </Box>

                        <Box sx={{ '& svg': { m: 2},display:"flex", alignItems:"center" }}>
                        <InsertCommentIcon />
                           <Typography>Comment </Typography>  
                        </Box>
                     
                    </Box>
                </Box>
            </Box>
        </div>
    )
}

export default AdministratorCommentsCard