import React from 'react'
import { Box, Button } from '@mui/material'
import RequestSearchBar from './RequestSearchBar'
import RequestMenu from './RequestMenu'
import RequestCreate from './RequestCreate'

function RequestMainPage() {
    return (
        <Box sx={{ m: 5, display: "flex", justifyContent: "space-between", width: "700px" }}>
            <Box>
                <Button variant='contained'>
                    <RequestCreate />
                </Button>
            </Box>

            <Box>
                <RequestSearchBar />
            </Box>
            
            <Box>
                <RequestMenu />
            </Box>

        </Box>
    )
}

export default RequestMainPage