import * as React from 'react'
import Box from '@mui/material/Box'
import Fab from '@mui/material/Fab'
import EditIcon from '@mui/icons-material/Edit'
import { AttachFile } from '@mui/icons-material'
import CloseIcon from '@mui/icons-material/Close'
import MoreHorizIcon from '@mui/icons-material/MoreHoriz'
import EmailIcon from '@mui/icons-material/Email'

export default function FloatingActionButtons() {
  return (
    <Box sx={{ '& > :not(style)': { m: 1 } }}>
      <Fab size='small' color='primary' aria-label='edit'>
        <EditIcon />
      </Fab>
      <Fab size='small' color='secondary' aria-label='attach'>
        <AttachFile />
      </Fab>

      <Fab size='small' color='secondary' aria-label='email'>
        <EmailIcon />
      </Fab>

      <Fab size='small' disabled aria-label='more'>
        <MoreHorizIcon />
      </Fab>

      <Fab size='small' color='secondary' aria-label='close'>
        <CloseIcon />
      </Fab>
    </Box>
  )
}
