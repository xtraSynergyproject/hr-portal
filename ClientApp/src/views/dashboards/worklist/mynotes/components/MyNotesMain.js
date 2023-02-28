import React from 'react'
import Box from '@mui/material/Box'
// import Card from '@mui/material/Card'
// import CardMedia from '@mui/material/CardMedia'
import  Viewimg from '../../dashboard/components/modals/note/viewnotemodal/Viewimg'
import SideBarLeft from '../../dashboard/components/modals/note/viewnotemodal/SideBarLeft'
import Paper from '@mui/material/Paper';
import SearchBar from '../../dashboard/components/modals/note/viewnotemodal/SearchBar';
import Divider from '@mui/material/Divider';

import { Typography } from '@mui/material'
import PaginationButton from '../../dashboard/components/modals/note/viewnotemodal/PaginationButton'
// import RequestMainPage from '../../dashboard/components/modals/notemodal/RequestMainPage'

function MyNotesMain() {
  return (
    <>
    <Box sx={{width:'75rem',mt:-10}}>
         <Box sx={{ display: 'column' }}>

                            <Box sx={{mt:15,width:'70rem'}}>
                            <Typography variant='h6' component='h6'>NoteHome</Typography>    

                                <Box sx={{mt:7}}>
                                <Viewimg/>


                                </Box>
                                <Box sx={{ display: 'flex' }}>
                                    <Box sx={{position:'absolute'}}>
                                        <SideBarLeft />

                                    </Box>
                                    <Box>
                                        <Box
                                            sx={{
                                                // display: 'column',
                                                // flexWrap: 'wrap',
                                                 '& > :not(style)': {
                                                    position:'absolute',
                                                    left:'39%',
                                                    mr: 20,
                                                    width: '50rem',
                                                    height: '29vh',
                                                },
                                            }}
                                        >
                                            <Paper elevation={1}>
                                                <Box sx={{mr:'20%'}}>
                                                <SearchBar />
                                                 <Divider/>

                                                </Box>
                                                 
                                                <Box

                                                    sx={{
                                                        position:'absolute',
                                                        // mt:6,
                                                        // ml:3,
                                                        width: '48rem',
                                                        height: '9vh',
                                                        backgroundColor: '#DCDCDC',
                                                        border: '1px solid grey',
                                                    }}

                                                >
                                               <Box sx={{}}>
                                               <PaginationButton />


                                               </Box>


                                        </Box>

                                    </Paper>
                                </Box>






                            </Box>
                        </Box>

                    </Box>

                </Box>
                </Box>

      
    </>
  )
}

export default MyNotesMain
