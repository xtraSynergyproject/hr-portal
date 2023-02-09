import React from 'react'
import { Box, Button } from '@mui/material'
import SearchBarTask from './SearchBarTask'
import TaskMenu from './TaskMenu'
import CreateModelTask from './CreateModelTask'
function TaskCallPage() {
    return (

        <Box sx={{ m: 2, display: "flex", gap: '8px', alignItems: 'center' }} >
            <Box sx={{ justifyContent: "left" }}><CreateModelTask /></Box>

            <SearchBarTask />

            <TaskMenu />
        </Box>

    )
}

export default TaskCallPage