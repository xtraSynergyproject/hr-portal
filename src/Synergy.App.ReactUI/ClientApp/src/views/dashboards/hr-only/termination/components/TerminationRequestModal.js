import React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import TextField from '@mui/material/TextField'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Fab from '@mui/material/Fab'
import Icon from 'src/@core/components/icon'

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
  width: '20rem',
  mb: 3,
  borderRadius: '10px'
}

function TerminationRequestModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <Box>
      <Button variant='contained' onClick={handleOpen}>
        Termination Request
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
            <Box sx={{ mb: 6 }} className='demo-space-x'>
              <Typography sx={{ p: 2 }} variant='h4' component='h3'>
                Termination
              </Typography>




              
            </Box>
            <hr />


            <Button sx={{borderRadius:"50px"}} variant='contained' component='label'>
              <Icon icon='mdi:pencil' />
              <input type='file' hidden />
            </Button>

            <Box sx={{ p: 3 }}>
              <Typography>
                Service Owner/Requester: <b>{}</b>
              </Typography>
              <Typography>
                Service Number: <b>{}</b>
              </Typography>
              <Typography>
                Service Status: <b>{}</b>
              </Typography>
              <Typography>
                Due Date: <b>{}</b>
              </Typography>
              <Typography>
                Service Version: <b>{}</b>
              </Typography>
            </Box>
            <Box>
              <TextField
                required
                fullWidth
                sx={{ marginBottom: '8px' }}
                id='date'
                label='Last Working Day'
                type='date'
                defaultValue='YYYY-MM-DD'
                InputLabelProps={{
                  shrink: true
                }}
              />

              <TextField
                fullWidth
                sx={{ marginBottom: '8px' }}
                id='date'
                label='Resignation/Termination Date'
                type='date'
                defaultValue='YYYY-MM-DD'
                InputLabelProps={{
                  shrink: true
                }}
              />
            </Box>
          </Box>
        </Box>
      </Modal>
    </Box>
  )
}

export default TerminationRequestModal
