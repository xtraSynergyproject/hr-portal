import React from 'react'
import { Box } from '@mui/material'
import SearchBarTask from './SearchBarTask'
import TaskMenu from './TaskMenu'
import Diaglog from './Diaglog'
import Pagination from './Pagination'

function TaskCallPage() {
  return (
    <div>
      <Box sx={{ m: 3, display: 'flex', alignItems: 'center' }}>
        <Box sx={{ justifyContent: 'left' }}>
          <Diaglog />
        </Box>

        <SearchBarTask />
        <TaskMenu />
      </Box>

      <Box sx={{ width: '100%' }}>
        <Pagination />
      </Box>
    </div>
  )
}

export default TaskCallPage
