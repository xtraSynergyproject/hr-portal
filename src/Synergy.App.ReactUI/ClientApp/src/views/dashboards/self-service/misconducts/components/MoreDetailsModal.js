import React, { useState } from 'react'
import Box from '@mui/material/Box'
import Icon from 'src/@core/components/icon'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import DateTime from './DateTime'

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

  width: '30rem',

  mb: 3,

  borderRadius: '10px'
}

export default function MoreDetailsModal() {
  const [open, setOpen] = useState(false)

  const handleOpen = () => setOpen(true)

  const handleClose = () => setOpen(false)

  return (
    <div>
      <Button variant='contained' onClick={handleOpen}>
        More Details
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
                More Details
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
            <hr/>
            <DateTime />
          </Box>
        </Box>
      </Modal>
    </div>
  )
}
