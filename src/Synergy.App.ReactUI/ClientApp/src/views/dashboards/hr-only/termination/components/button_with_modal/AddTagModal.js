import * as React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import Modal from '@mui/material/Modal'
import Icon from 'src/@core/components/icon'
import TerminationTable from '../TerminationTable'

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
  width: '55rem',
  height: '35rem',
  mb: 3,
  borderRadius: '10px'
}

export default function AddTagModal() {
  const [open, setOpen] = React.useState(false)
  const handleOpen = () => setOpen(true)
  const handleClose = () => setOpen(false)

  return (
    <div>
      <Button
        onClick={handleOpen}
        sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }}
        component='label'
      >
         <Icon icon='mdi:pencil-plus' />
                  Add Tag
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
              sx={{ display: 'flex', justifyContent: 'space-between', alignContent: 'center', alignItems: 'center' }}
            >
              <Typography sx={{ p: 5 }} variant='h4' component='h3'>
              Add Tags
              </Typography>

              <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                <Button sx={{ display: 'flex', flexDirection: 'column', borderRadius:"50px" }}>
                  <Icon icon='mdi:plus' />
                  Add 
                </Button>
                <Button
                  sx={{ mr: 3, borderRadius: '50px', display: 'flex', flexDirection: 'column' }}
                  onClick={handleClose}
                  component='label'
                >
                  <Icon icon='mdi:close' />
                  Close
                </Button>
              </Box>
            </Box>
            <hr />

            
          </Box>
        </Box>
      </Modal>
    </div>
  )
}
