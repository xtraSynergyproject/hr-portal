import React from 'react'
import { Box, Button } from '@mui/material'
import SearchBarTask from './SearchBarTask'
import TaskMenu from './TaskMenu'
import CreateInfoBtn from './CreateInfoBtn'
import Searchbtn from './Searchbtn'
import CreateModelTask from './CreateModelTask'

function TaskCallPage() {
    return (
        <Box sx={{ m: 1, display: "flex", justifyContent: "space-between", height: "30px", width: "100px", gap: 8 }} >
            <CreateModelTask />
            <CreateInfoBtn />
            <Searchbtn />
            <SearchBarTask />
            <TaskMenu />
        </Box>
    )
}

export default TaskCallPage