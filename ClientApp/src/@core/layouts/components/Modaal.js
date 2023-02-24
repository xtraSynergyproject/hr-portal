import * as React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import DialogContent from '@mui/material/DialogContent'
import Modal from '@mui/material/Modal'
import IconButton from '@mui/material/IconButton'
import Icon from 'src/@core/components/icon'
import DialogTitle from '@mui/material/DialogTitle'
import DialogActions from '@mui/material/DialogActions'
import TextField from '@mui/material/TextField'
import { Divider } from '@mui/material'

const modalWrapper = {
  position: 'fixed',
  overflow: 'scroll',
  maxHeight: '60vh',
  display: 'flex',
  top: '25%'
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
  width: '40rem',
  boxShadow: 2,
  borderRadius: '10px'
}

export default function BasicModal() {
  const [open, setOpen] = React.useState(true)

  const handleOpen = () => setOpen(true)

  const handleClose = () => setOpen(false)

  return (
    <div>
      <Modal
        open={open}
        sx={modalWrapper}
        onClose={handleClose}
        aria-labelledby='modal-modal-title'
        aria-describedby='modal-modal-description'
      >
        <Box sx={modalBlock}>
          <Box sx={modalContentStyle}>
            <DialogContent>
              <DialogTitle variant='h5'>Change password</DialogTitle>
              <IconButton
                aria-label='close'
                onClick={handleClose}
                sx={{ top: 30, right: 10, position: 'absolute', color: 'grey.500' }}
              >
                <Icon icon='mdi:close' />
              </IconButton>
              <Divider />

              <form>
                
                <TextField
                  fullWidth
                  sx={{ mr: 10, top: 10 }}
                  id='outlined-password-input'
                  label='Current Password'
                  type='password'
                  autoComplete='current-password'
                />
                <br />
                <br />
                <TextField
                  fullWidth
                  sx={{ mr: 10, top: 10 }}
                  id='outlined-password-input'
                  label='New Password'
                  type='password'
                  autoComplete='current-password'
                />
                <br />
                <br />
                <TextField
                  fullWidth
                  sx={{ mr: 10, top: 10 }}
                  id='outlined-password-input'
                  label='Confirm Password'
                  type='password'
                  autoComplete='current-password'
                />
              </form>
              <br />

              <Box sx={{ display: 'flex', flexDirection: 'row' }}>
                <DialogActions>
                  <Button href='#' variant='contained'>
                    Change Password
                  </Button>
                </DialogActions>

                <DialogActions>
                  <Button onClick={handleClose} variant='outlined'>
                    Close
                  </Button>
                </DialogActions>
              </Box>
            </DialogContent>
          </Box>
        </Box>
      </Modal>
    </div>
  )
}
