import React from 'react'
import { Box, Button } from '@mui/material'
import SearchBarTask from './SearchBarTask'
import TaskMenu from './TaskMenu'
import Searchbtn from './Searchbtn'

function TaskCallPage() {
    return (
        <Box sx={{ m: 5, display: "flex", justifyContent: "space-between", height: "30px", width: "100px", gap: 8 }} >

            <Searchbtn />
            <SearchBarTask />
            <TaskMenu />
        </Box>
    )
}

export default TaskCallPage