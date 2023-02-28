import React from 'react';
import Box from "@mui/material/Box";
import Typography from '@mui/material/Typography';
import Icon from 'src/@core/components/icon';
import DescriptionAssignTo from '../modals1/DescriptionAssignTo'
import Paper from '@mui/material/Paper';
import InputBase from '@mui/material/InputBase';


function DescriptionModal() {
    return (
        <>
            <Box sx={{ display: 'column', mt: 5, boxShadow: '25px' }}>
                <Typography sx={{ ml: 3 }}>Description</Typography>
                <Box sx={{ display: 'flex', mb: 2, mt: 3 }}>

                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            ml: 3,
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:format-bold' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            ml: 3,
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:format-italic' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            ml: 3,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:format-underline' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            ml: 3,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:format-align-left' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            ml: 3,
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:format-align-center' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            ml: 3,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:format-align-right' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            flexDirection: 'column',
                            ml: 3,
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:format-align-justify' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            ml: 3,
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:format-list-bulleted' fontSize='18px' />
                    </Box>
                    {/* <Box
                  sx={{
                    borderRadius: '50px',
                    display: 'flex',
                    ml:3,
                    flexDirection: 'column',
                    alignItems: 'center',
                    fontSize: '13px',
                    cursor:"pointer"
                  }} >
                <Icon icon='mdi:table-of-contents' fontSize='18px' />              
                  </Box> */}
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            ml: 3,
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:format-text-variant' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            ml: 3,
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:format-text' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            ml: 3,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:alpha-h' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            ml: 3,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:brush' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            ml: 3,
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:image-outline' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            ml: 3,
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:file-outline' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            flexDirection: 'column',
                            ml: 3,
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:video' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            ml: 3,
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:github-circle' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            ml: 3,
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:table-large' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            ml: 3,
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:recycle' fontSize='18px' />
                    </Box>
                    <Box
                        sx={{
                            borderRadius: '50px',
                            display: 'flex',
                            ml: 3,
                            flexDirection: 'column',
                            alignItems: 'center',
                            fontSize: '13px',
                            cursor: "pointer"
                        }} >
                        <Icon icon='mdi:code-tags' fontSize='18px' />
                    </Box>


                </Box>
                {/* <Box
                    component="form"
                    sx={{
                        '& .MuiTextField-root': { m: 1, width: '55rem',height:'90rem' },
                    }}
                    noValidate
                    autoComplete="off"
                >

                    <TextField
                        id="outlined-multiline-flexible"
                        // label="Multiline"
                        multiline
                        maxRows={80}
                    />

                </Box> */}
                     <Box
                     
                      component="form" sx={{
                     ml:12,
                     flex:2,

                    border: '1px solid grey',
                    ml: 4,
                    width: '57rem',
                    height: 350,
                    bgcolor: 'background.paper',
                    color: (theme) => theme.palette.getContrastText(theme.palette.background.paper),
                }}> 
                <Box sx={{ display: 'flex',mt:83, backgroundColor: '#DCDCDC' }}> 
                 <Box
                    sx={{
                        borderRadius: '50px',
                        ml: 4,

                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        fontSize: '13px',
                        cursor: "pointer"
                    }} >
                    <Icon icon='mdi:undo-variant' fontSize='18px' />
                </Box>
                <Box
                    sx={{
                        borderRadius: '50px',
                        ml: 4,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        fontSize: '13px',
                        cursor: "pointer"
                    }} >
                    <Icon icon='mdi:redo-variant' fontSize='18px' />
                </Box>  
            </Box> 

                </Box>


                </Box >
                <DescriptionAssignTo />


            </>
            )
}

            export default DescriptionModal
