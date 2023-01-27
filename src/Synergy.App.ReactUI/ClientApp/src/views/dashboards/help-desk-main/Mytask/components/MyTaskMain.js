import React from 'react'
import { Box, Button } from '@mui/material'
import SearchBar from './SearchBar'
import Menu from './Menu'
import CreacteTask from './CreateTask'
import Searchbtn from './Searchbtn'
import CreateModel from './CreateModel'

function MyTaskMain() {
    return (
        <Box sx={{ m: 1, display: "flex", justifyContent: "space-between", height: "30px", width: "100px", gap: 8 }} >
            <CreateModel />
            <CreacteTask />
            <Searchbtn />
            
            <SearchBar />
            <Menu />
        </Box>
    )
}

export default MyTaskMain