import React from 'react'
import { Box, Button } from '@mui/material'
import SearchBar from './SearchBar'
import Menu from './Menu'
import CreacteRequest from './CreateRequest'

function MyTaskMain() {
    return (
        <Box sx={{ m: 2, display: "flex", justifyContent: "space-between", width: "700px" }}>
            <Box>
                <Button variant='contained'>
                    <CreacteRequest />
                </Button>
            </Box>

            <Box>
                <SearchBar />
            </Box>
            <Box>
                <Menu />
            </Box>

        </Box>
    )
}

export default MyTaskMain