import React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
import { Divider } from '@mui/material'
import DialogActions from '@mui/material/DialogActions'
import Vuserform from './Vuserform'

const modalWrapper = {
  // overflow: 'auto',
  maxHeight: '80vh',
  display: 'flex',
  top: '70px'
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
  mt: 10,
  width: '30rem',
  mb: 3,
  borderRadius: '10px'
}

function TerminationRequestModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <Box>
      <Button variant='contained' sx={{ marginLeft: '35px' }} onClick={handleOpen}>
        +AddUser
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
            <Box
              sx={{ mt: '12px', display: 'flex', justifyContent: 'space-between' }}
              className='demo-space-x'
            >
              <Typography sx={{ pl: 6 }}>
                <b>Grant User Access</b>
              </Typography>

              <Box
                sx={{
                  width: '200px',
                  height: '20px',
                  display: 'flex',
                  justifyContent: 'flex-end',
                  alignContent: 'center',
                }}
              >
                <Typography
                  sx={{
                    borderRadius: '50px',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    fontSize: '13px',
                    cursor: 'pointer'
                  }}
                  onClick={handleClose}
                  component='label'
                >
                  <Icon icon='mdi:close' />
                </Typography>
              </Box>
            </Box>
            <br />
            <Divider />
            <Box sx={{mt:'15px'}}>
              <Vuserform />
            </Box>
            <Divider/>
            <DialogActions sx={{ mt: '10px', mr: '5px', display: 'flex', justifyContent: 'flex-end' }}>
              <Button variant='contained' href='#'>
                Save
              </Button>
            </DialogActions>
          </Box>
        </Box>
      </Modal>
    </Box>
  )
}

export default TerminationRequestModal
