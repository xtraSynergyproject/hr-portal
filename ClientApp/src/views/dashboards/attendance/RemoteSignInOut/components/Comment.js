import * as React from 'react'
//import { Icon } from '@iconify/react';

import Box from '@mui/material/Box'

import Button from '@mui/material/Button'

import Typography from '@mui/material/Typography'
import Icon from 'src/@core/components/icon'

import Modal from '@mui/material/Modal'

import TextField from '@mui/material/TextField'
import { Divider, FormControl, InputLabel, Select, MenuItem, value } from '@mui/material'
import MenuButton from './MenuButton'
import Comment from './Comment'
import { comment } from 'stylis'
import { Save } from '@mui/icons-material'
// import MenuBtn from './MenuBtn'

const modalWrapper = {
  overflow: 'auto',

  maxHeight: '100vh',

  display: 'flex'
}

const modalBlock = {
  position: 'relative',

  zIndex: 0,

  display: 'flex',

  alignItems: 'center',

  justifyContent: 'center',

  margin: 'auto'
}

const modalContentStyle = {
  position: 'relative',

  background: '#fff',

  boxShadow: 24,

  mt: 3,

  width: '50rem',

  mb: 3,

  borderRadius: '10px'
}

export default function OtherReimbModal() {
  const [value, setValue] = React.useState('')
  const handleChanges = event => {
    setValue(event.target.value)
  }
  const [open, setOpen] = React.useState(false, Save)

  const handleOpen = () => setOpen(true)

  const handleClose = () => setOpen(false)
  const handleBack = () => setOpen(false)
  const handleComment = () => setOpen(false)

  return (
    <div>
      <Button onClick={handleOpen}>
        <Icon icon='akar-icons:comment-add' />
      </Button>

      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby='modal-modal-title'
        aria-describedby='modal-modal-description'
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>
            <Box sx={{ mb: 2, display: 'flex', justifyContent: 'space-between' }} className='demo-space-x'>
              <Typography sx={{ p: 4 }} variant='h4' component='h3'>
                Comment Box
              </Typography>

              <Button
                sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }}
                onClick={handleClose}
                component='label'
              >
                <Icon icon='mdi:close' />
                Close
              </Button>
            </Box>

            <Box sx={{ py: 3, px: 5 }}>
              <Box>
                <TextField required fullWidth sx={{ borderRadius: '50px' }} />
              </Box>
              <Box sx={{ display: 'flex', mt: 15, mb: 5, justifyContent: 'flex-end', gap: 3 }}>
                <Button variant='contained' onClick={handleComment}>
                  Comment
                </Button>
                <Button variant='contained' onClick={handleBack}>
                  Back
                </Button>
              </Box>
            </Box>
          </Box>
        </Box>
      </Modal>
    </div>
  )
}
