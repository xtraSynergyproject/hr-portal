import * as React from 'react'
import Box from '@mui/material/Box'
import Button from '@mui/material/Button'
import Typography from '@mui/material/Typography'
import Icon from 'src/@core/components/icon'
import Divider from '@mui/material/Divider'
import Modal from '@mui/material/Modal'
import FabIcon from './FabIcon'
import TextField from '@mui/material/TextField'
import EmployeeDataSelect from './EmployeeDataSelect'

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
  const [open, setOpen] = React.useState(false)

  const handleOpen = () => setOpen(true)

  const handleClose = () => setOpen(false)

  return (
    <div>
      <Button variant='contained' onClick={handleOpen} sx={{ width: '50%', ml: '50%', height: '20px' }}>
        create
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
                Create Service
              </Typography>

              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignContent: 'center' }}>
                <Button sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }} component='label'>
                  <input type='file' hidden />
                </Button>

                <Button
                  sx={{ borderRadius: '50px', display: 'flex', flexDirection: 'column' }}
                  onClick={handleClose}
                  component='label'
                >
                  <Icon icon='mdi:close' />
                  Close
                </Button>
              </Box>
            </Box>
            <hr />
            <Box sx={{ py: 3, px: 5 }}>
              <Box>
                <Box sx={{ display: 'flex', alignItems: 'center', '& svg': { mr: 1, color: 'text.secondary' } }}>
                  <Typography sx={{ ml: 1, color: 'text.secondary', fontWeight: 600 }}>
                    <b> General Request: </b>
                  </Typography>
                </Box>

                <Box sx={{ display: 'flex', alignItems: 'center', gap: '5px' }}>
                  <Typography sx={{ my: 3 }}>
                    <b> General Request: </b> <b>{}</b>
                  </Typography>
                  <Box sx={{ paddingLeft: '350px' }}>
                    <FabIcon />
                  </Box>
                </Box>
              </Box>

              <Divider />

              <Box marginTop={30}>
                <Box sx={{ display: 'flex', gap: '5px' }}>
                  <Button variant='contained'>Save As Draft</Button>
                  <Button variant='contained'>Submit</Button>
                </Box>
                <Divider />
                <Box marginTop={10}>
                  <EmployeeDataSelect />
                </Box>
                <Box marginTop={10}>
                  <TextField
                    sx={{ marginY: '5px' }}
                    required
                    fullWidth
                    id='outlined-basic'
                    label='Request Details'
                    variant='outlined'
                  />
                </Box>
                <Box
                  sx={{
                    mt: 6
                  }}
                ></Box>
              </Box>
              <Box sx={{ display: 'flex', mt: 1, mb: 2, justifyContent: 'flex-start ', gap: 3 }}>
                <Button variant='contained'>Save As Draft</Button>
                <Button variant='contained'>Submit</Button>
              </Box>
            </Box>
          </Box>
        </Box>
      </Modal>
          
    </div>
  )
}
