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

            <Button variant='contained' sx={{ margin: '10px' }}>
                <RequestCreate />
            </Button>
            <Box>
                <RequestMenu />
            </Box>

        </Box>
    )
}

export default RequestMainPage