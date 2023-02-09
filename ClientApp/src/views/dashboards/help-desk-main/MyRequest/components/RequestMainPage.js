import React from 'react'
import { Box, Button } from '@mui/material'
import RequestSearchBar from './RequestSearchBar'
import RequestMenu from './RequestMenu'
import RequestCreate from './RequestCreate'

function RequestMainPage() {
    return (
        <Box sx={{ m: 5, display: "flex", justifyContent: "space-between", width: "700px" }}>


            <Box>
                <RequestSearchBar />
            </Box>

            
            <Box sx={{ display: 'flex', justifyContent: 'space-between', gap: '70px' }}>
                
                <Box sx={{ paddingLeft: '130px' }}>
                    <RequestMenu />
                </Box>
            </Box>
        </Box>
    )
}

export default RequestMainPage